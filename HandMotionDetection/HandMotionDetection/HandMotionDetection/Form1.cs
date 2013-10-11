using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkinDetection;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;

namespace HandMotionDetection
{
    public partial class Form1 : Form
    {
        //Emgu CV
        private Capture grabber;

        //Frame
        private Image<Bgr, Byte> currentFrame;
        private Image<Bgr, Byte> editedFrame;
        private int frameHeight;
        private int frameWidth;

        //Temp Memory Storage
        private MemStorage contourStorage;

        //Face Detector
        private CascadeClassifier faceHaar;

        //Skin Detector
        private IColorSkinDetector skinDetector;
        private Hsv hsv_min;
        private Hsv hsv_max;
        private Ycc YCrCb_min;
        private Ycc YCrCb_max;

        //Finger Detection
        private Seq<Point> hull;
        private MCvBox2D box;
        private Seq<MCvConvexityDefect> defects;
        private MCvConvexityDefect[] defectArray;

        public Form1()
        {
            InitializeComponent();

            string videoPath = "C:/Users/L33549.CITI/Desktop/a.avi";
            grabVideo(videoPath);

            faceHaar = new CascadeClassifier("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/HandMotionDetection/HandMotionDetection/HandMotionDetection/haar/haarcascade_frontalface.xml");

            hsv_min = new Hsv(0, 45, 0);
            hsv_max = new Hsv(20, 255, 255);
            YCrCb_min = new Ycc(0, 129, 0);
            YCrCb_max = new Ycc(255, 185, 135);

            contourStorage = new MemStorage();
            box = new MCvBox2D();

            Application.Idle += new EventHandler(frameAnalysis);
        }

        private void grabVideo(string path)
        {
            grabber = new Emgu.CV.Capture(path);
            frameHeight = grabber.Height;
            frameWidth = grabber.Width;
        }

        private void frameAnalysis(object sender, EventArgs e)
        {
            currentFrame = grabber.QueryFrame();
            if (currentFrame != null)
            {
                //Remove face from frame
                Bitmap remove = currentFrame.ToBitmap();
                detectAndRemoveFace(remove);
                editedFrame = new Image<Bgr, Byte>(remove);

                //Use skin detection to detect hand
                skinDetector = new YCrCbSkinDetector();
                Image<Gray, Byte> skin = skinDetector.DetectSkin(editedFrame, YCrCb_min, YCrCb_max);

                //Find 2 largest contour (left and right hand)
                List<Contour<Point>> handContours = findHandContours(skin);

                //Find finger tips to ensure it is a hand
                foreach (Contour<Point> hand in handContours)
                {

                }

                defaultFrame.Image = editedFrame;
                blackFrame.Image = skin;
            }
        }

        private void detectAndRemoveFace(Bitmap edit)
        {
            Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
            Rectangle[] faceDetected = faceHaar.DetectMultiScale(grayFrame, 1.2, 20, Size.Empty, Size.Empty);
            SolidBrush black = new SolidBrush(System.Drawing.Color.Black);
            System.Drawing.Pen bPen = new System.Drawing.Pen(black, 2);
            Graphics g = Graphics.FromImage(edit);
            if (faceDetected.Count() > 0)
            {
                Rectangle f1 = faceDetected.ElementAt(0);
                Size rec = f1.Size;
                int h = rec.Height + 100;
                int w = rec.Width + 100;
                int x = f1.X - 50;
                int y = f1.Y - 50;
                Point newPoint = new Point(x, y);
                Size newSize = new Size(w, h);
                Rectangle editRec = new Rectangle(newPoint, newSize);
                g.FillRectangle(black, editRec);
                //g.DrawLine(bPen, new Point((x + h) / 2, f1.Y + 200), new Point((x + h) / 2, f1.Y - 200));
            }
        }

        private List<Contour<Point>> findHandContours(Image<Gray, Byte> skin)
        {
            List<Contour<Point>> handList = new List<Contour<Point>>();
            Contour<Point> contours = skin.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, contourStorage);
            Contour<Point> left = null;
            Contour<Point> right = null;

            Double currentSize = 0;
            Double leftSize = 0;
            Double rightSize = 0;

            while (contours != null)
            {
                currentSize = contours.Area;
                if (currentSize > leftSize)
                {
                    if (detectFingerTips(contours) == true)
                    {
                        leftSize = currentSize;
                        left = contours;
                    }
                }
                else if (currentSize > rightSize)
                {
                    if (detectFingerTips(contours) == true)
                    {
                        rightSize = currentSize;
                        right = contours;
                    }
                }
                contours = contours.HNext;
            }

            if (left != null)
            {
                handList.Add(left);
            }
            if (right != null)
            {
                handList.Add(right);
            }
            return handList;
        }

        private bool detectFingerTips(Contour<Point> hand)
        {
            bool realHand = false;
            double first = 0;

            Contour<Point> currentContour = hand.ApproxPoly(hand.Perimeter * 0.0025, contourStorage);

            hull = currentContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            box = currentContour.GetMinAreaRect();

            PointF center = new PointF(box.center.X, box.center.Y);
            float centerX = box.center.X;
            float centerY = box.center.Y;

            defects = currentContour.GetConvexityDefacts(contourStorage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            defectArray = defects.ToArray();

            if (defects != null)
            {
                List<double> tipDistance = new List<double>();
                List<PointF> points = new List<PointF>();
                List<PointF> depthPoints = new List<PointF>();
                for (int i = 0; i < defects.Total; i++)
                {
                    PointF startPoint = new PointF((float)defectArray[i].StartPoint.X, (float)defectArray[i].StartPoint.Y);
                    PointF depthPoint = new PointF((float)defectArray[i].DepthPoint.X, (float)defectArray[i].DepthPoint.Y);
                    float sX = defectArray[i].StartPoint.X;
                    float sY = defectArray[i].StartPoint.Y;
                    double distance = (Math.Pow(sX - centerX, 2) + Math.Pow(sY - centerY, 2));
                    tipDistance.Add(distance);
                    points.Add(startPoint);
                    depthPoints.Add(depthPoint);
                }

                for (int i = 0; i < tipDistance.Count(); i++)
                {
                    if (i == 0)
                    {
                        first = tipDistance.ElementAt(i);
                    }
                    else
                    {
                        double difference = first - tipDistance.ElementAt(i);
                        if ((difference < 10) && (tipDistance.ElementAt(i) > -10))
                        {
                            realHand = true;
                        }
                    }
                }

                if (realHand == true)
                {
                    //Draw outline of hand
                    editedFrame.Draw(currentContour, new Bgr(System.Drawing.Color.LimeGreen), 2);
                    //Draw Mid Point of hand
                    editedFrame.Draw(new CircleF(center, 3), new Bgr(200, 125, 75), 2);
                    /*foreach (PointF startPoint in points)
                    {
                        editedFrame.Draw(new CircleF(startPoint, 3), new Bgr(System.Drawing.Color.BlueViolet), 2);
                    }
                    foreach (PointF depthPoint in depthPoints)
                    {
                        editedFrame.Draw(new CircleF(depthPoint, 3), new Bgr(System.Drawing.Color.DarkRed), 2);
                    }*/
                    for (int i = 0; i < defects.Total; i++)
                    {
                        PointF startPoint = new PointF((float)defectArray[i].StartPoint.X, (float)defectArray[i].StartPoint.Y);

                        PointF depthPoint = new PointF((float)defectArray[i].DepthPoint.X, (float)defectArray[i].DepthPoint.Y);

                        PointF endPoint = new PointF((float)defectArray[i].EndPoint.X, (float)defectArray[i].EndPoint.Y);

                        LineSegment2D startDepthLine = new LineSegment2D(defectArray[i].StartPoint, defectArray[i].DepthPoint);

                        LineSegment2D depthEndLine = new LineSegment2D(defectArray[i].DepthPoint, defectArray[i].EndPoint);

                        CircleF startCircle = new CircleF(startPoint, 5f);

                        CircleF depthCircle = new CircleF(depthPoint, 5f);

                        CircleF endCircle = new CircleF(endPoint, 5f);

                        //Custom heuristic based on some experiment, double check it before use
                        if ((startCircle.Center.Y < box.center.Y || depthCircle.Center.Y < box.center.Y) && (startCircle.Center.Y < depthCircle.Center.Y) && (Math.Sqrt(Math.Pow(startCircle.Center.X - depthCircle.Center.X, 2) + Math.Pow(startCircle.Center.Y - depthCircle.Center.Y, 2)) > box.size.Height / 6.5))
                        {
                            editedFrame.Draw(startDepthLine, new Bgr(System.Drawing.Color.Green), 2);
                        }
                    }
                }
            }

            return realHand;
        }
    }
}
