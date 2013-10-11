using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;
using Emgu.Util;
using HandGestureRecognition.SkinDetector;
using System.IO;

namespace HandGestureRecognition
{
    public partial class Form1 : Form
    {

        IColorSkinDetector skinDetector;

        Image<Bgr, Byte> currentFrame;
        Image<Bgr, Byte> currentFrameCopy;
                
        Capture grabber;
        AdaptiveSkinDetector detector;
        
        int frameWidth;
        int frameHeight;
        
        Hsv hsv_min;
        Hsv hsv_max;
        Ycc YCrCb_min;
        Ycc YCrCb_max;
        
        Seq<Point> hull;
        Seq<Point> filteredHull;
        Seq<MCvConvexityDefect> defects;
        MCvConvexityDefect[] defectArray;
        Rectangle handRect;
        MCvBox2D box;
        Ellipse ellip;

        int MAX_POINTS = 20;
        int MIN_FINGER_DEPTH = 20;
        int MAX_FINGER_ANGLE = 60;
        int MIN_THUMB = 120;
        int MAX_THUMB = 200;
        int MIN_INDEX = 60;
        int MAX_INDEX = 120;

        MemStorage contourStorage, approxStorage, hullStorage, defectsStorage;

        Point[] tipPts, foldPts;
        float[] depths;
        List<Point> fingerTips;
        Point cogPt;
        int contourAxisAngle;

        private KinectSensor sensor;
        private WriteableBitmap colorBitmap;
        private byte[] colorPixels;

        private CascadeClassifier face;
        
        public Form1()
        {
            InitializeComponent();

            grabber = new Emgu.CV.Capture("C:/Users/L33549.CITI/Desktop/a.avi");            
            grabber.QueryFrame();
            frameWidth = grabber.Width;
            frameHeight = grabber.Height;            
            //detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);
            hsv_min = new Hsv(0, 45, 0);
            hsv_max = new Hsv(20, 255, 255);
            YCrCb_min = new Ycc(0, 129, 40);
            YCrCb_max = new Ycc(255, 185, 135);
            box = new MCvBox2D();
            ellip = new Ellipse();

            contourStorage = new MemStorage();
            approxStorage = new MemStorage();
            hullStorage = new MemStorage();
            defectsStorage = new MemStorage();

            tipPts = new Point[MAX_POINTS];   // coords of the finger tips
            foldPts = new Point[MAX_POINTS];  // coords of the skin folds between fingers
            depths = new float[MAX_POINTS];   // distances from tips to folds
            cogPt = new Point();
            fingerTips = new List<Point>();
            face = new CascadeClassifier("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/HandGestureRecognition/HandGestureRecognition/HandGestureRecognition/haar/Original/haarcascade_hand.xml");

            Application.Idle += new EventHandler(FrameGrabber);

            /*foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the color stream to receive color frames
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Allocate space to put the pixels we'll receive
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                // This is the bitmap we'll display on-screen
                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Set the image we display to point to the bitmap where we'll put the image data
                //this.Image.Source = this.colorBitmap;

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.ColorFrameReady += this.SensorColorFrameReady;

                // Start the sensor!
                this.sensor.Start();
            }*/
        }

        private Bitmap convertFromColorFrame(ColorImageFrame colorFrame)
        {
            Bitmap convertBit = null;

            // Copy the pixel data from the image to a temporary array
            colorFrame.CopyPixelDataTo(this.colorPixels);

            // Write the pixel data into our bitmap
            this.colorBitmap.WritePixels(
                new System.Windows.Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                this.colorPixels,
                this.colorBitmap.PixelWidth * sizeof(int),
                0);

            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(colorBitmap));
            MemoryStream ms = new MemoryStream();
            encoder.Save(ms);
            convertBit = new Bitmap(ms);

            return convertBit;
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                //System.Threading.Thread.Sleep(50);
                if (colorFrame != null)
                {
                    Bitmap b = convertFromColorFrame(colorFrame);
                    frameHeight = b.Height;
                    frameWidth = b.Width;
                    currentFrame = new Image<Bgr, Byte>(b);
                    removeFace(b);
                    if (currentFrame != null)
                    {
                        skinDetector = new YCrCbSkinDetector();
                        Image<Gray, Byte> skin = skinDetector.DetectSkin(currentFrame, YCrCb_min, YCrCb_max);
                        Contour<Point> bContour = biggestContour(skin);
                        if (bContour != null)
                        {
                            box = bContour.GetMinAreaRect();
                            currentFrame.Draw(new CircleF(new PointF(box.center.X, box.center.Y), 3), new Bgr(200, 125, 75), 2);
                            currentFrame.Draw(bContour, new Bgr(System.Drawing.Color.LimeGreen), 2);
                            extractContourInfo(bContour, 5, currentFrame);
                            findFingerTips(bContour, 5);
                            drawTips(tipPts, foldPts);
                        }
                        imageBoxSkin.Image = skin;
                        imageBoxFrameGrabber.Image = currentFrame;
                    }
                }
            }
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            currentFrame = grabber.QueryFrame();
            System.Threading.Thread.Sleep(50);
            if (currentFrame != null)
            {
                currentFrameCopy = currentFrame.Copy();
                
                // Uncomment if using opencv adaptive skin detector
                //Image<Gray,Byte> skin = new Image<Gray,byte>(currentFrameCopy.Width,currentFrameCopy.Height);                
                //detector.Process(currentFrameCopy, skin);                

                skinDetector = new YCrCbSkinDetector();
                Image<Gray, Byte> skin = skinDetector.DetectSkin(currentFrameCopy,YCrCb_min,YCrCb_max);
                /*List<Contour<Point>> handContours = findHandContour(skin);
                if (handContours != null)
                {
                    for (int i = 0; i < handContours.Count(); i++)
                    {
                        if (handContours.ElementAt(i) != null)
                        {
                            extractContourInfo(handContours.ElementAt(i), 5, currentFrame); // find the COG and angle to horizontal of the contour
                            findFingerTips(handContours.ElementAt(i), 5); // detect the fingertips positions in the contour
                            //nameFingers(cogPt, contourAxisAngle, fingerTips);
                        }
                    }
                }*/
                ExtractContourAndHull(skin);
                DrawAndComputeFingersNum();

                imageBoxSkin.Image = skin;
                imageBoxFrameGrabber.Image = currentFrame;
            }
        }

        private void removeFace(Bitmap b)
        {
            Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
            Rectangle[] faceDetected = face.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty, Size.Empty);
            SolidBrush black = new SolidBrush(System.Drawing.Color.Black);
            Graphics g = Graphics.FromImage(b);
            if (faceDetected.Count() > 0)
            {
                g.FillRectangles(black, faceDetected);
            }
        }

        private double toDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private int angleBetween(Point tip, Point next, Point prev) // calculate the angle between the tip and its neighboring folds (in integer degrees)
        {
            return Math.Abs((int)Math.Round(toDegree(Math.Atan2(next.X - tip.X, next.Y - tip.Y) - Math.Atan2(prev.X - tip.X, prev.Y - tip.Y))));
        }

        private int calculateTilt(double m11, double m20, double m02)
        {
            double diff = m20 - m02;
            if (diff == 0)
            {
                if (m11 == 0)
                    return 0;
                else if (m11 > 0)
                    return 45;
                else // m11 < 0
                    return -45;
            }

            double theta = 0.5 * Math.Atan2(2 * m11, diff);
            int tilt = (int)Math.Round(toDegree(theta));
            if ((diff > 0) && (m11 == 0))
                return 0;
            else if ((diff < 0) && (m11 == 0))
                return -90;
            else if ((diff > 0) && (m11 > 0)) // 0 to 45 degrees
                return tilt;
            else if ((diff > 0) && (m11 < 0)) // -45 to 0
                return (180 + tilt); // change to counter-clockwise angle
            else if ((diff < 0) && (m11 > 0)) // 45 to 90
                return tilt;
            else if ((diff < 0) && (m11 < 0)) // -90 to -45
                return (180 + tilt); // change to counter-clockwise angle
            return 0;
        }

        private List<Contour<Point>> findHandContours(Image<Gray, Byte> skin)
        {
            List<Contour<Point>> contourList = new List<Contour<Point>>();
            Contour<Point> contours = skin.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, contourStorage);
            Contour<Point> firstHand = null;
            Contour<Point> secondHand = null;

            Double current = 0;
            Double largest = 200.0;
            Double fhand = 100.0;
            Double shand = 100.0;

            while (contours != null)
            {
                current = contours.Area;
                if (current > largest)
                {
                    largest = current;
                }
                else if (current < largest)
                {
                    if (current > fhand)
                    {
                        fhand = current;
                        firstHand = contours;
                    }
                    else if (current > shand)
                    {
                        shand = current;
                        secondHand = contours;
                    }
                }
                contours = contours.HNext;
            }
            if (firstHand != null)
                contourList.Add(firstHand);
            if (secondHand != null)
                contourList.Add(secondHand);
            return contourList;
        }

        public Contour<Point> biggestContour(Image<Gray, Byte> skin)
        {
            Contour<Point> contours = skin.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, contourStorage);
            Contour<Point> biggestContour = null;
            Double current = 0;
            Double largest = 0;

            while (contours != null)
            {
                current = contours.Area;
                if (current > largest)
                {
                    largest = current;
                    biggestContour = contours;
                }
                contours = contours.HNext;
            }

            return biggestContour;
        }

        private void extractContourInfo(Contour<Point> bigContour, int scale, Image<Bgr, Byte> im)
        {
            MCvMoments moments = new MCvMoments();
            CvInvoke.cvMoments(bigContour, ref moments, 1);

            // center of gravity
            double m00 = CvInvoke.cvGetSpatialMoment(ref moments, 0, 0);
            double m10 = CvInvoke.cvGetSpatialMoment(ref moments, 1, 0);
            double m01 = CvInvoke.cvGetSpatialMoment(ref moments, 0, 1);

            if (m00 != 0)
            { // calculate center
                int xCenter = (int)Math.Round(m10 / m00) * scale;
                int yCenter = (int)Math.Round(m01 / m00) * scale;
                currentFrame.Draw(new CircleF(new PointF(xCenter, yCenter), 3), new Bgr(200, 125, 75), 2);
                cogPt = new Point(xCenter, yCenter);
                //Size s = new Size(bigContour.MCvContour.rect.Height, bigContour.MCvContour.rect.Width);
                //Rectangle rect = new Rectangle(cogPt, s);
                //im.Draw(rect, new Bgr(System.Drawing.Color.Red), 2);
            }

            double m11 = CvInvoke.cvGetCentralMoment(ref moments, 1, 1);
            double m20 = CvInvoke.cvGetCentralMoment(ref moments, 2, 0);
            double m02 = CvInvoke.cvGetCentralMoment(ref moments, 0, 2);
            contourAxisAngle = calculateTilt(m11, m20, m02); // deal with hand contour pointing downwards

            // uses fingertips information generated on the last update of the hand, so will be out-of-date
            if (fingerTips.Count() > 0)
            {
                int yTotal = 0;
                for (int i = 0; i < fingerTips.Count(); i++)
                {
                    yTotal += fingerTips.ElementAt(i).Y;
                }
                int avgYFinger = yTotal / fingerTips.Count();
                if (avgYFinger > cogPt.Y)
                { // fingers below COG
                    contourAxisAngle += 180;
                }
            }
        }

        private void findFingerTips(Contour<Point> bigContour, int scale)
        {
            Contour<Point> appContour = bigContour.ApproxPoly(bigContour.Perimeter * 0.0025, contourStorage);
            hull = bigContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            defects = bigContour.GetConvexityDefacts(contourStorage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            defectArray = defects.ToArray();

            currentFrame.DrawPolyline(hull.ToArray(), true, new Bgr(0, 0, 255), 2);

            int defectsTotal = defectArray.Count();
            if (defectsTotal > MAX_POINTS)
            {
                defectsTotal = MAX_POINTS;
            }

            // copy defect information from defects sequence into arrays
            for (int i = 0; i < defectsTotal; i++)
            {
                MCvConvexityDefect cdf = defectArray.ElementAt(i);
                Point startPt = cdf.StartPoint;
                Point endPt = cdf.EndPoint;
                Point depthPt = cdf.DepthPoint;

                double sx = startPt.X;
                double sy = startPt.Y;
                tipPts[i] = new Point((int)Math.Round(sx * scale), (int)Math.Round(sy * scale)); // array contains coords of the fingertips

                //PointF depthPoint = new PointF((float)depthPt.X, (float)depthPt.Y);
                //currentFrame.Draw(new CircleF(depthPoint, 3), new Bgr(System.Drawing.Color.Blue), 2);

                double dx = depthPt.X;
                double dy = depthPt.Y;
                foldPts[i] = new Point((int)Math.Round(dx * scale), (int)Math.Round(dy * scale)); //array contains coords of the skin fold between fingers
                depths[i] = cdf.Depth * scale; // array contains distances from tips to folds

                reduceTips(defectsTotal, tipPts, foldPts, depths);
            }
        }

        private void reduceTips(int numPoints, Point[] tipPts, Point[] foldPts, float[] depths)
        {
            fingerTips.Clear();
            for (int i = 0; i < numPoints; i++)
            {
                if (depths[i] < MIN_FINGER_DEPTH)
                {
                    continue;
                }
                // look at fold points on either side of a tip
                int pdx = (i == 0) ? (numPoints - 1) : (i - 1); // predecessor of i
                int sdx = (i == numPoints - 1) ? 0 : (i + 1); // successor of i
                int angle = angleBetween(tipPts[i], foldPts[pdx], foldPts[sdx]);
                if (angle >= MAX_FINGER_ANGLE)
                {
                    continue; // angle between finger and folds too wide
                }
                // this point is probably a fingertip, so add to list
                fingerTips.Add(tipPts[i]);
            }
        }

        private void drawTips(Point[] tipPts, Point[] foldPts)
        {
            foreach (Point p in foldPts)
            {
                float x = p.X;
                float y = p.Y;
                currentFrame.Draw(new CircleF(new PointF(x, y), 4), new Bgr(System.Drawing.Color.Black), 2);
            }
        }

        private void ExtractContourAndHull(Image<Gray, byte> skin)
        {
            List<Contour<Point>> contourList = new List<Contour<Point>>();
            Contour<Point> contours = skin.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, contourStorage);
            Contour<Point> biggestContour = null;

            Double current = 0;
            Double largest = 0;

            while (contours != null)
            {
                current = contours.Area;
                if (current > largest)
                {
                    largest = current;
                    biggestContour = contours;
                }
                contours = contours.HNext;
            }

            if (biggestContour != null)
            {
                //currentFrame.Draw(biggestContour, new Bgr(Color.DarkViolet), 2);
                Contour<Point> currentContour = biggestContour.ApproxPoly(biggestContour.Perimeter * 0.0025, contourStorage);
                currentFrame.Draw(currentContour, new Bgr(System.Drawing.Color.LimeGreen), 2);
                biggestContour = currentContour;

                hull = biggestContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
                box = biggestContour.GetMinAreaRect();
                PointF[] points = box.GetVertices();
                //handRect = box.MinAreaRect();
                //currentFrame.Draw(handRect, new Bgr(200, 0, 0), 1);

                Point[] ps = new Point[points.Length];
                for (int i = 0; i < points.Length; i++)
                    ps[i] = new Point((int)points[i].X, (int)points[i].Y);

                currentFrame.DrawPolyline(hull.ToArray(), true, new Bgr(200, 125, 75), 2);
                currentFrame.Draw(new CircleF(new PointF(box.center.X, box.center.Y), 3), new Bgr(200, 125, 75), 2);

                //ellip.MCvBox2D= CvInvoke.cvFitEllipse2(biggestContour.Ptr);
                //currentFrame.Draw(new Ellipse(ellip.MCvBox2D), new Bgr(Color.LavenderBlush), 3);

                PointF center;
                float radius;
                //CvInvoke.cvMinEnclosingCircle(biggestContour.Ptr, out  center, out  radius);
                //currentFrame.Draw(new CircleF(center, radius), new Bgr(System.Drawing.Color.Gold), 2);

                //currentFrame.Draw(new CircleF(new PointF(ellip.MCvBox2D.center.X, ellip.MCvBox2D.center.Y), 3), new Bgr(100, 25, 55), 2);
                //currentFrame.Draw(ellip, new Bgr(Color.DeepPink), 2);

                //CvInvoke.cvEllipse(currentFrame, new Point((int)ellip.MCvBox2D.center.X, (int)ellip.MCvBox2D.center.Y), new System.Drawing.Size((int)ellip.MCvBox2D.size.Width, (int)ellip.MCvBox2D.size.Height), ellip.MCvBox2D.angle, 0, 360, new MCvScalar(120, 233, 88), 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);
                //currentFrame.Draw(new Ellipse(new PointF(box.center.X, box.center.Y), new SizeF(box.size.Height, box.size.Width), box.angle), new Bgr(0, 0, 0), 2);


                filteredHull = new Seq<Point>(contourStorage);
                for (int i = 0; i < hull.Total; i++)
                {
                    if (Math.Sqrt(Math.Pow(hull[i].X - hull[i + 1].X, 2) + Math.Pow(hull[i].Y - hull[i + 1].Y, 2)) > box.size.Width / 10)
                    {
                        filteredHull.Push(hull[i]);
                    }
                }

                defects = biggestContour.GetConvexityDefacts(contourStorage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);

                defectArray = defects.ToArray();
            }
        }

        private void DrawAndComputeFingersNum()
        {
            int fingerNum = 0;

            #region hull drawing
            //for (int i = 0; i < filteredHull.Total; i++)
            //{
            //    PointF hullPoint = new PointF((float)filteredHull[i].X,
            //                                  (float)filteredHull[i].Y);
            //    CircleF hullCircle = new CircleF(hullPoint, 4);
            //    currentFrame.Draw(hullCircle, new Bgr(Color.Aquamarine), 2);
            //}
            #endregion

            #region defects drawing
            if (defects != null)
            {
                for (int i = 0; i < defects.Total; i++)
                {
                    PointF startPoint = new PointF((float)defectArray[i].StartPoint.X,
                                                    (float)defectArray[i].StartPoint.Y);

                    PointF depthPoint = new PointF((float)defectArray[i].DepthPoint.X,
                                                    (float)defectArray[i].DepthPoint.Y);

                    PointF endPoint = new PointF((float)defectArray[i].EndPoint.X,
                                                    (float)defectArray[i].EndPoint.Y);

                    LineSegment2D startDepthLine = new LineSegment2D(defectArray[i].StartPoint, defectArray[i].DepthPoint);

                    LineSegment2D depthEndLine = new LineSegment2D(defectArray[i].DepthPoint, defectArray[i].EndPoint);

                    CircleF startCircle = new CircleF(startPoint, 5f);

                    CircleF depthCircle = new CircleF(depthPoint, 5f);

                    CircleF endCircle = new CircleF(endPoint, 5f);

                    //Custom heuristic based on some experiment, double check it before use
                    if ((startCircle.Center.Y < box.center.Y || depthCircle.Center.Y < box.center.Y) && (startCircle.Center.Y < depthCircle.Center.Y) && (Math.Sqrt(Math.Pow(startCircle.Center.X - depthCircle.Center.X, 2) + Math.Pow(startCircle.Center.Y - depthCircle.Center.Y, 2)) > box.size.Height / 6.5))
                    {
                        fingerNum++;
                        currentFrame.Draw(startDepthLine, new Bgr(System.Drawing.Color.Green), 2);
                        //currentFrame.Draw(depthEndLine, new Bgr(Color.Magenta), 2);
                    }


                    currentFrame.Draw(startCircle, new Bgr(System.Drawing.Color.Red), 2);
                    currentFrame.Draw(depthCircle, new Bgr(System.Drawing.Color.Yellow), 5);
                    //currentFrame.Draw(endCircle, new Bgr(Color.DarkBlue), 4);
                }
            }
            #endregion

            MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_DUPLEX, 5d, 5d);
            currentFrame.Draw(fingerNum.ToString(), ref font, new Point(50, 150), new Bgr(System.Drawing.Color.White));
        }
                                      
    }
}