using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkinDetection;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace HandMotionDetection
{
    public partial class Form1 : Form
    {
        //Emgu CV
        private Capture grabber;

        //Frame
        private long time;
        private Image<Bgr, Byte> currentFrame;
        private Image<Gray, Byte> grayFrame;
        private Image<Bgr, Byte> editedSkinFrame;
        private List<Image<Bgr, Byte>> currentFrameList;
        private List<Image<Gray, Byte>> grayFrameList;
        private int frameHeight;
        private int frameWidth;

        //Temp Memory Storage
        private MemStorage contourStorage;

        //Face Detection
        private CascadeClassifier faceHaar;
        private Rectangle[] faceDetect;
        private Point facePoint;

        //Skin Detection
        private Image<Gray, Byte> skin;
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
        private List<LineSegment2D> fingers;

        //Hand Motion Detection
        private List<Image<Gray, Byte>> handMotionList;
        private List<PointF> motionPoints;
        private List<string> gestures;
        private List<long> realTimes;

        public Form1()
        {
            InitializeComponent();

            string videoPath = "C:/Users/L33549.CITI/Desktop/a.avi";
            //string videoPath = "C:/Users/hzhig_000/Dropbox/FYPJ 2013 P3/Video/c.avi";
            //string videoPath = "C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/Video/c.avi";
            grabVideo(videoPath);

            faceHaar = new CascadeClassifier("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/HandMotionDetection/HandMotionDetection/HandMotionDetection/haar/haarcascade_frontalface.xml");
            //faceHaar = new CascadeClassifier("C:/Users/hzhig_000/Desktop/AbuseAnalysis/HandMotionDetection/HandMotionDetection/HandMotionDetection/haar/haarcascade_frontalface.xml");

            hsv_min = new Hsv(0, 45, 0);
            hsv_max = new Hsv(20, 255, 255);
            //YCrCb_min = new Ycc(0, 131, 80);
            YCrCb_min = new Ycc(0, 140, 0);
            YCrCb_max = new Ycc(255, 185, 135);

            currentFrameList = new List<Image<Bgr, byte>>();
            grayFrameList = new List<Image<Gray, byte>>();
            handMotionList = new List<Image<Gray, byte>>();

            contourStorage = new MemStorage();
            box = new MCvBox2D();
            fingers = new List<LineSegment2D>();
            motionPoints = new List<PointF>();
            gestures = new List<string>();
            realTimes = new List<long>();

            //Application.Idle += new EventHandler(frameAnalysis);
        }

        private void grabVideo(string path)
        {
            grabber = new Emgu.CV.Capture(path);
            frameHeight = grabber.Height;
            frameWidth = grabber.Width;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            videoAnalysis();
        }

        private void frameAnalysis(object sender, EventArgs e)
        {
            currentFrame = grabber.QueryFrame();
            if (currentFrame != null)
            {
                //Detect face
                grayFrame = currentFrame.Convert<Gray, Byte>();
                detectFace();

                //Remove face
                Bitmap remove = currentFrame.ToBitmap();
                removeFace(remove);
                editedSkinFrame = new Image<Bgr, Byte>(remove);

                //Use skin detection to detect hand
                skinDetector = new YCrCbSkinDetector();
                Image<Gray, Byte> skin = skinDetector.DetectSkin(editedSkinFrame, YCrCb_min, YCrCb_max);

                //Find 2 largest contour (left and right hand)
                List<Contour<Point>> handContours = findHandContours();

                //defaultFrame.Image = editedFrame;
                blackFrame.Image = skin;
            }

            //Face Analysis
            faceAnalysis();
        }

        private void videoAnalysis()
        {
            bool reading = true;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (reading)
            {
                currentFrame = grabber.QueryFrame();
                if (currentFrame != null)
                {
                    time = sw.ElapsedMilliseconds;
                    //Detect face
                    grayFrame = currentFrame.Convert<Gray, Byte>();
                    detectFace();

                    //Remove face
                    Bitmap remove = currentFrame.ToBitmap();
                    removeFace(remove);
                    editedSkinFrame = new Image<Bgr, Byte>(remove);

                    //Use skin detection to detect hand
                    skinDetector = new YCrCbSkinDetector();
                    skin = skinDetector.DetectSkin(editedSkinFrame, YCrCb_min, YCrCb_max);

                    //Find 2 largest contour (left and right hand)
                    List<Contour<Point>> handContours = findHandContours();

                    defaultFrame.Image = editedSkinFrame;
                    blackFrame.Image = skin;
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000 / 10);
                }
                else
                {
                    reading = false;
                }
            }

            //Face Analysis
            faceAnalysis();

            //Hand Motion Analysis
            handMotionAnalysis();
        }

        private void detectFace()
        {
            faceDetect = faceHaar.DetectMultiScale(grayFrame, 1.1, 20, new Size(20, 20), Size.Empty);
            foreach (Rectangle ac in faceDetect)
            {
                grayFrameList.Add(grayFrame.Copy(ac).Resize(64, 64, INTER.CV_INTER_CUBIC));
            }
            currentFrameList.Add(currentFrame);
        }

        private void faceAnalysis()
        {
            //Data for face emotion when data base is not ready.
            Image<Gray, byte>[] imageList = new Image<Gray, Byte>[10];
            //imageList[0] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/netural1.jpg");
            //imageList[1] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/sad1.jpg");
            //imageList[2] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/angry1.jpg");
            //imageList[3] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy1.jpg");
            //imageList[4] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy2.jpg");
            //imageList[5] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy3.jpg");
            //imageList[6] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy4.jpg");
            //imageList[7] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy5.jpg");
            //imageList[8] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy6.jpg");
            //imageList[9] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy7.jpg");

            imageList[0] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/netural1.jpg");
            imageList[1] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/sad1.jpg");
            imageList[2] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/angry1.jpg");
            imageList[3] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/happy1.jpg");
            imageList[4] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/happy2.jpg");
            imageList[5] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/happy3.jpg");
            imageList[6] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/happy4.jpg");
            imageList[7] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/happy5.jpg");
            imageList[8] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/happy6.jpg");
            imageList[9] = new Image<Gray, byte>("C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/motionAnalyse/Images/emotionData/happy7.jpg");

            String[] emoList = new String[10];
            emoList[0] = "Netural";
            emoList[1] = "Sad";
            emoList[2] = "Angry";
            emoList[3] = "Smile";
            emoList[4] = "Smile";
            emoList[5] = "Smile";
            emoList[6] = "Smile";
            emoList[7] = "Smile";
            emoList[8] = "Smile";
            emoList[9] = "Smile";

            int[] label = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            //Use the variable to detect the number of times these emotion appear in the video by frames;
            int smile = 0;
            int angry = 0;
            int netural = 0;
            int sad = 0;
            int noDetect = 0;
            int frames = grayFrameList.Count;

            List<Image<Gray, byte>> trainfaceList = new List<Image<Gray, byte>>();

            Rectangle[] trainFace;
            for (int i = 0; i < imageList.Length; i++)
            {
                trainFace = faceHaar.DetectMultiScale(imageList[i], 1.1, 20, new Size(20, 20), Size.Empty);
                foreach (Rectangle f in trainFace)
                {
                    trainfaceList.Add(imageList[i].Copy(f).Resize(64, 64, INTER.CV_INTER_CUBIC));
                }
            }

            //Detect the face in the frames and do emotion detect using fisher eigen face recognizer
            double maxDistance = 1000; //-> Range from 1000 to 5000

            FisherFaceRecognizer fisher = new FisherFaceRecognizer(imageList.Length, maxDistance);
            fisher.Train(trainfaceList.ToArray(), label);
            FaceRecognizer.PredictionResult result = new FaceRecognizer.PredictionResult();

            for (int i = 0; i < frames; i++)
            {
                result = fisher.Predict(grayFrameList.ElementAt(i));
                lbmsg.Text = frames.ToString();
                int num = result.Label;

                if (num == -1)
                {
                    noDetect++;
                }
                else if (emoList[num].Equals("Smile"))
                {
                    smile++;
                }
                else if (emoList[num] == "Sad")
                {
                    sad++;
                }
                else if (emoList[num] == "Angry")
                {
                    angry++;
                }
                else if (emoList[num] == "Netural")
                {
                    netural++;
                }
                else
                {
                    // it should not come here . 
                }
            }

            //get total sum of the 5 emotion data.
            int total = smile + sad + angry + netural + noDetect;
            //put them into the percentage (emotion/frames * 100)
            double smilePer = (Convert.ToDouble(smile) / frames) * 100;
            double sadPer = (Convert.ToDouble(sad) / frames) * 100;
            double angryPer = (Convert.ToDouble(angry) / frames) * 100;
            double neturalPer = (Convert.ToDouble(netural) / frames) * 100;
            double noDetectPer = (Convert.ToDouble(noDetect) / frames) * 100;

            lbmsg.Text = smile.ToString() + " " + sad.ToString() + " " + angry.ToString() + " " + netural.ToString() + " " + noDetect.ToString();
            String[] emotionList = { "Smile", "Sad", "Angry", "Netural", "No Detect" };
            double[] emotionPer = { smilePer, sadPer, angryPer, neturalPer, noDetectPer };

            //Designing the Pie Chart 
            chart1.Series.Add("Emotion");
            chart1.Series["Emotion"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie; // set the chart to pie
            chart1.Series["Emotion"].ChartArea = "ChartArea1";

            //Add data to pie chart
            for (int i = 0; i < emotionList.Count(); i++)
            {
                chart1.Series["Emotion"].Points.AddXY(emotionList[i], emotionPer[i]);
            }

            chart1.Series["Emotion"].BorderWidth = 1;
            chart1.Series["Emotion"].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart1.Series["Emotion"].Points[0].Color = System.Drawing.Color.LightGreen;
            chart1.Series["Emotion"].Points[1].Color = System.Drawing.Color.Blue;
            chart1.Series["Emotion"].Points[2].Color = System.Drawing.Color.Red;
            chart1.Series["Emotion"].Points[3].Color = System.Drawing.Color.Gray;
            chart1.Series["Emotion"].Points[4].Color = System.Drawing.Color.Black;

            chart1.Titles.Add("Anaylsis of Face Expression (Frame By Frame)");
            chart1.Series["Emotion"]["PieLabelStyle"] = "Outside";
            chart1.Series["Emotion"].Label = "#VALX";

            chart1.Legends.Add("Legend1");
            chart1.Legends["Legend1"].Enabled = true;
            chart1.Legends["Legend1"].Docking = Docking.Bottom;
            chart1.Legends["Legend1"].Alignment = System.Drawing.StringAlignment.Center;
            chart1.Series["Emotion"].LegendText = "#PERCENT{P2} ";
            chart1.DataManipulator.Sort(PointSortOrder.Ascending, chart1.Series["Emotion"]);
        }

        private void removeFace(Bitmap edit)
        {
            SolidBrush black = new SolidBrush(System.Drawing.Color.Black);
            System.Drawing.Pen bPen = new System.Drawing.Pen(black, 2);
            Graphics g = Graphics.FromImage(edit);
            if (faceDetect.Count() > 0)
            {
                Rectangle f1 = faceDetect.ElementAt(0);
                Size rec = f1.Size;
                int h = rec.Height + 150;
                int w = rec.Width + 40;
                int x = f1.X - 20;
                int y = f1.Y - 10;
                facePoint = new Point(x, y);
                Size newSize = new Size(w, h);
                Rectangle editRec = new Rectangle(facePoint, newSize);
                g.FillRectangle(black, editRec);
                //g.DrawLine(bPen, new Point((x + h) / 2, f1.Y + 200), new Point((x + h) / 2, f1.Y - 200));
            }
        }

        private List<Contour<Point>> findHandContours()
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
                    if (detectAndDrawHand(contours) == true)
                    {
                        leftSize = currentSize;
                        left = contours;
                    }
                }
                else if (currentSize > rightSize)
                {
                    if (detectAndDrawHand(contours) == true)
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

        private bool detectAndDrawHand(Contour<Point> hand)
        {
            bool realHand = false;
            double first = 0;

            Contour<Point> currentContour = hand.ApproxPoly(hand.Perimeter * 0.0025, contourStorage);

            hull = currentContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            box = currentContour.GetMinAreaRect();

            //Calculate the center of the hand
            PointF center = new PointF(box.center.X, box.center.Y);
            float centerX = box.center.X;
            float centerY = box.center.Y;

            //Find all defects in the contour
            defects = currentContour.GetConvexityDefacts(contourStorage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            defectArray = defects.ToArray();

            if (defects != null)
            {
                List<double> tipDistance = new List<double>();

                for (int i = 0; i < defects.Total; i++)
                {
                    float sX = defectArray[i].StartPoint.X;
                    float sY = defectArray[i].StartPoint.Y;
                    double distance = (Math.Pow(sX - centerX, 2) + Math.Pow(sY - centerY, 2));
                    tipDistance.Add(distance);
                }

                for (int i = 0; i < tipDistance.Count(); i++)
                {
                    if (i == 0)
                    {
                        first = tipDistance.ElementAt(i);
                    }
                    else if (i > 0)
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
                    //Draw hand
                    drawHand(currentContour, center);

                    //Analysis hand gesture
                    string gesture = analysisHandGesture(center);
                    if (gesture.Equals("Palm") || (gesture.Equals("Fist")))
                    {
                        PointF newCenter = new PointF(centerX, centerY);
                        handMotionList.Add(skin);
                        motionPoints.Add(newCenter);
                        gestures.Add(gesture);
                        realTimes.Add(time);
                    }
                }
            }
            return realHand;
        }

        private void drawHand(Contour<Point> currentContour, PointF center)
        {
            //Draw outline of hand
            editedSkinFrame.Draw(currentContour, new Bgr(System.Drawing.Color.LimeGreen), 2);
            //Draw Mid Point of hand
            editedSkinFrame.Draw(new CircleF(center, 3), new Bgr(200, 125, 75), 2);

            fingers.Clear();
            for (int i = 0; i < defects.Total; i++)
            {
                PointF startPoint = new PointF((float)defectArray[i].StartPoint.X, (float)defectArray[i].StartPoint.Y);
                Point newStartPoint = new Point(defectArray[i].StartPoint.X + 10, defectArray[i].StartPoint.Y);

                PointF depthPoint = new PointF((float)defectArray[i].DepthPoint.X, (float)defectArray[i].DepthPoint.Y);
                Point newDepthPoint = new Point(defectArray[i].DepthPoint.X + 10, defectArray[i].DepthPoint.Y);

                PointF endPoint = new PointF((float)defectArray[i].EndPoint.X, (float)defectArray[i].EndPoint.Y);

                LineSegment2D startDepthLine = new LineSegment2D(newStartPoint, newDepthPoint);

                LineSegment2D depthEndLine = new LineSegment2D(defectArray[i].DepthPoint, defectArray[i].EndPoint);

                CircleF startCircle = new CircleF(startPoint, 5f);

                CircleF depthCircle = new CircleF(depthPoint, 5f);

                CircleF endCircle = new CircleF(endPoint, 5f);

                //Custom heuristic based on some experiment, double check it before use
                if ((startCircle.Center.Y < box.center.Y || depthCircle.Center.Y < box.center.Y) && (startCircle.Center.Y < depthCircle.Center.Y) && (Math.Sqrt(Math.Pow(startCircle.Center.X - depthCircle.Center.X, 2) + Math.Pow(startCircle.Center.Y - depthCircle.Center.Y, 2)) > box.size.Height / 6.5))
                {
                    editedSkinFrame.Draw(startDepthLine, new Bgr(System.Drawing.Color.Green), 2);
                    fingers.Add(startDepthLine);
                }
            }
        }

        private string analysisHandGesture(PointF center)
        {
            string gesture = "empty";
            float centerX = box.center.X;
            int size = fingers.Count();
            if (facePoint.X + 70 < centerX)
            {
                lbLeftHand.Text = "Left found";
                if (size == 0)
                {
                    lbLeftHand.Text = "Fist";
                    gesture = "Fist";
                }
                else if (size == 5)
                {
                    lbLeftHand.Text = "Palm";
                    gesture = "Palm";
                }
                else if (size > 0)
                {
                    lbLeftHand.Text = size + " fingers";
                }
            }
            if (facePoint.X - 10 > centerX)
            {
                lbRightHand.Text = "Right found";
                if (size == 0)
                {
                    lbRightHand.Text = "Fist";
                    gesture = "Fist";
                }
                else if (size == 5)
                {
                    lbRightHand.Text = "Palm";
                    gesture = "Palm";
                }
                else if (size > 0)
                {
                    lbRightHand.Text = size + " fingers";
                }
            }
            if ((facePoint.X + 70 > centerX) && (facePoint.X - 10 < centerX))
            {
                lbLeftHand.Text = "NULL";
                lbRightHand.Text = "NULL";
            }
            return gesture;
        }

        private void handMotionAnalysis()
        {
            MessageBox.Show(realTimes.Count().ToString());
            MessageBox.Show((realTimes.ElementAt(realTimes.Count() - 1) / 1000).ToString());
            if (handMotionList != null)
            {
                //Loop through all detected motions
                for (int i = 0; i < handMotionList.Count(); i++)
                {
                    string gesture = "";
                    if (i == 0)
                    {
                        gesture = gestures.ElementAt(i).ToString();
                    }
                    else
                    {
                        if (gesture.Equals(gestures.ElementAt(i).ToString()))
                        {

                        }
                        else
                        {
                            gesture = gestures.ElementAt(i).ToString();
                        }
                    }
                }
            }
        }
    }
}