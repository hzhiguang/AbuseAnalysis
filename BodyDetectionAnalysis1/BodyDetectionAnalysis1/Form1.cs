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
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;
using Emgu.Util;

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

        private HandDetector handDec = null;   // for detecting hand and fingers
        private static int WIDTH = 640;  
        private static int HEIGHT = 480;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string path = "C:/Users/L33549.CITI/Desktop/c.avi";
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
                    CascadeClassifier fullBodyCascade = new CascadeClassifier(h);
                    Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
                    Rectangle[] fullBodyDetected = fullBodyCascade.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty, Size.Empty);
                    foreach (Rectangle fullBody in fullBodyDetected)
                    {
                        currentFrame.Draw(fullBody, new Bgr(Color.Yellow), 2);
                        imgs.Add(currentFrame.Clone());
                    }
                    capturedImageBox.Image = currentFrame.Clone();
                    imageBox1.Image = grayFrame.Clone();

                    //handDec = new HandDetector("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/gloveHSV.txt", WIDTH, HEIGHT);
                    //handDec.update(currentFrame);
                    capturedImageBox.Image = currentFrame.Clone();

                    //Draw the image, the detected hand and finger info, and the average ms snap time at the bottom left of the panel.
                    //Graphics g = Graphics.FromImage(currentFrame.ToBitmap());
                    //handDec.draw(g);

                    //System.Threading.Thread.Sleep(50);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(imgs.Count().ToString());
        }
    }
}
