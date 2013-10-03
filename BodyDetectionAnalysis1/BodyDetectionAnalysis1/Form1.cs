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
        private Capture _capture;

        private HandDetector handDec = null;   // for detecting hand and fingers
        private static int WIDTH = 640;  
        private static int HEIGHT = 480;

        private CascadeClassifier hand = new CascadeClassifier("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/haar/OS/handcascade.xml");
        List<Image<Bgr, Byte>> imgs = new List<Image<Bgr, Byte>>();

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
                    handDec = new HandDetector("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/gloveHSV.txt", WIDTH, HEIGHT);
                    handDec.update(currentFrame);

                    //Draw the image, the detected hand and finger info, and the average ms snap time at the bottom left of the panel.
                    Graphics g = Graphics.FromImage(currentFrame.ToBitmap());
                    handDec.draw(g);
                    capturedImageBox.Image = currentFrame.Clone();

                    System.Threading.Thread.Sleep(50);

                    /*Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
                    Rectangle[] handDetected = hand.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty, Size.Empty);
                    foreach (Rectangle hands in handDetected)
                    {
                        currentFrame.Draw(hands, new Bgr(Color.Yellow), 2);
                        imgs.Add(currentFrame.Clone());
                    }
                    capturedImageBox.Image = currentFrame.Clone();*/
                }
            }
        }
    }
}
