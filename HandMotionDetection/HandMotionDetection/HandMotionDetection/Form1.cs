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

        public Form1()
        {
            InitializeComponent();

            string videoPath = "C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/Video/c.avi";
            grabVideo(videoPath);

            faceHaar = new CascadeClassifier("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/HandMotionDetection/HandMotionDetection/HandMotionDetection/haar/haarcascade_frontalface.xml");

            hsv_min = new Hsv(0, 45, 0);
            hsv_max = new Hsv(20, 255, 255);
            YCrCb_min = new Ycc(0, 129, 40);
            YCrCb_max = new Ycc(255, 185, 135);

            contourStorage = new MemStorage();

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
                    detectFingerTips(hand);
                }

                defaultFrame.Image = editedFrame;
                blackFrame.Image = skin;
            }
        }

        private void detectAndRemoveFace(Bitmap edit)
        {
            Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
            Rectangle[] faceDetected = faceHaar.DetectMultiScale(grayFrame, 1.2, 2, Size.Empty, Size.Empty);
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
                    leftSize = currentSize;
                    left = contours;
                }
                else if (currentSize > rightSize)
                {
                    rightSize = currentSize;
                    right = contours;
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

        private void detectFingerTips(Contour<Point> hand)
        {
            Contour<Point> currentContour = hand.ApproxPoly(hand.Perimeter * 0.0025, contourStorage);
            editedFrame.Draw(currentContour, new Bgr(System.Drawing.Color.LimeGreen), 2);
        }
    }
}
