using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.FFMPEG;
using AForge.Vision.Motion;
using AForge.Imaging;
using AForge.Imaging.Filters;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;
using Emgu.Util;
using Emgu.CV.CvEnum;

namespace BodyDetectionAnalysis1
{
    public partial class Form1 : Form
    {
        private MotionDetector detector = new MotionDetector(new SimpleBackgroundModelingDetector(), new BlobCountingObjectsProcessing());
        private string f = "C:/Users/L33549.CITI/Desktop/ChildAbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/haar/Original/haarcascade_frontalface_default.xml";
        private string ub = "C:/Users/L33549.CITI/Desktop/ChildAbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/haar/Original/haarcascade_mcs_upperbody.xml";
        private string lb = "C:/Users/L33549.CITI/Desktop/ChildAbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/haar/Original/haarcascade_lowerbody.xml";
        private string fb = "C:/Users/L33549.CITI/Desktop/ChildAbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/haar/Original/haarcascade_fullbody.xml";
        private string h = "C:/Users/L33549.CITI/Desktop/ChildAbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/haar/Original/haarcascade_hand.xml";

        private Capture _capture;
        List<Bitmap> images = new List<Bitmap>();
        List<Image<Bgr, Byte>> imgs = new List<Image<Bgr, Byte>>();

        private static int IMG_SCALE = 2;
        private int hueLower, hueUpper, satLower, satUpper, briLower, briUpper;
        private MIplImage scaleImg; // for resizing the webcam image
        private MIplImage hsvImg; // HSV version of webcam image
        private MIplImage imgThreshed; // threshold for HSV settings

        // hand details
        private Point cogPt; // center of gravity (COG) of contour
        private int contourAxisAngle; // contour's main axis angle relative to the horiz (in degrees)
        private List<Point> fingerTips;
        private static float SMALLEST_AREA = 600.0f; // ignore smaller contour areas
        private MemStorage contourStorage;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string path = "C:/Users/L33549.CITI/Desktop/b.avi";
            _capture = new Capture(path);
            _capture.ImageGrabbed += loadAndProcess;
            _capture.Start();
        }

        private void loadAndProcess(object sender, EventArgs e)
        {
            using (Image<Bgr, Byte> currentFrame = _capture.RetrieveBgrFrame().Clone())
            {
                if (currentFrame != null)
                {
                    CascadeClassifier fullBodyCascade = new CascadeClassifier(fb);
                    Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
                    Rectangle[] fullBodyDetected = fullBodyCascade.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty, Size.Empty);
                    foreach (Rectangle fullBody in fullBodyDetected)
                    {
                        currentFrame.Draw(fullBody, new Bgr(Color.Yellow), 2);
                        imgs.Add(currentFrame.Clone());
                    }
                    capturedImageBox.Image = currentFrame.Clone();
                    imageBox1.Image = grayFrame.Clone();

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(imgs.Count().ToString());
        }

        private void update(MIplImage im)
        {
            IntPtr image = im.roi;
            IntPtr scaleImage = scaleImg.roi;
            IntPtr hsvImage = hsvImg.roi;
            IntPtr imageThreshed = imgThreshed.roi;

            CvInvoke.cvResize(image, scaleImage, INTER.CV_INTER_CUBIC); // reduce the size of the image to make processing faster

            // convert image format to HSV
            CvInvoke.cvCvtColor(scaleImage, hsvImage, COLOR_CONVERSION.CV_BGR2HSV);

            // threshold image using loaded HSV settings for user's glove
            CvInvoke.cvInRangeS(hsvImage, new MCvScalar(hueLower, satLower, briLower, 0),
                                        new MCvScalar(hueUpper, satUpper, briUpper, 0),
                                        imageThreshed);

            CvInvoke.cvMorphologyEx(imageThreshed, imageThreshed, imageThreshed, imageThreshed, CV_MORPH_OP.CV_MOP_OPEN, 1);

            // erosion followed by dilation on the image to remove 
            // specks of white while retaining the image size
            MCvSeq bigContour = findBiggestContour(imgThreshed);
            IntPtr bigC = bigContour.ptr;
            if (bigC == null)
            {
                return;
            }
            extractContourInfo(bigContour, IMG_SCALE); // find the COG and angle to horizontal of the contour
            //findFingerTips(bigContour, IMG_SCALE); // detect the fingertips positions in the contour
            //nameFingers(cogPt, contourAxisAngle, fingerTips);
        }

        private MCvSeq findBiggestContour(MIplImage imgThreshed)
        {
            IntPtr imageThreshed = imgThreshed.roi;
            MCvSeq bigContour = new MCvSeq(); // generate all the contours in the threshold image as a list
            MCvSeq contours = new MCvSeq();
            IntPtr con = contours.ptr;
            CvInvoke.cvFindContours(imageThreshed, contourStorage, ref con, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MCvContour)),
                RETR_TYPE.CV_RETR_LIST,
                CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                new Point(0, 0));
            // find the largest contour in the list based on bounded box size
            float maxArea = SMALLEST_AREA;
            while (con != null)
            {
                if (contours.elem_size > 0)
                {
                    MCvBox2D box = CvInvoke.cvMinAreaRect2(con, contourStorage);
                    SizeF size = box.size;
                    float area = size.ToPointF().X * size.ToPointF().Y;
                    if (area > maxArea)
                    {
                        maxArea = area;
                        bigContour = contours;
                    }
                }
                con = contours.h_next;
            }
            return bigContour;
        }

        private void extractContourInfo(MCvSeq bigContour, int scale)
        {
            MCvMoments moments = new MCvMoments();
            IntPtr bigC = bigContour.ptr;
            CvInvoke.cvMoments(bigC, ref moments, 1);

            // center of gravity
            double m00 = CvInvoke.cvGetSpatialMoment(ref moments, 0, 0);
            double m10 = CvInvoke.cvGetSpatialMoment(ref moments, 1, 0);
            double m01 = CvInvoke.cvGetSpatialMoment(ref moments, 0, 1);

            if (m00 != 0)
            { // calculate center
                int xCenter = (int)Math.Round(m10 / m00) * scale;
                int yCenter = (int)Math.Round(m01 / m00) * scale;
                cogPt = new Point(xCenter, yCenter);
            }

            double m11 = CvInvoke.cvGetCentralMoment(ref moments, 1, 1);
            double m20 = CvInvoke.cvGetCentralMoment(ref moments, 2, 0);
            double m02 = CvInvoke.cvGetCentralMoment(ref moments, 0, 2);
            contourAxisAngle = calculateTilt(m11, m20, m02);

            // deal with hand contour pointing downwards
            // uses fingertips information generated on the last update of the hand, so will be out-of-date
            if (fingerTips.Count() > 0) {
                int yTotal = 0;
                for (int i = 0; i < fingerTips.Count(); i++ )
                {
                    yTotal += fingerTips.ElementAt(i).Y;
                }
                int avgYFinger = yTotal/fingerTips.Count();
                if (avgYFinger > cogPt.Y) { // fingers below COG
                    contourAxisAngle += 180;
                }
            }
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
            int tilt = (int)Math.Round(Math.toDegrees(theta));
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
    }
}
