using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
using Microsoft.Kinect;
using System.IO;

namespace BodyDetectionAnalysis1
{
    public partial class Form1 : Form
    {
        private Capture _capture;

        private HandDetector handDec = null;   // for detecting hand and fingers
        private static int WIDTH = 640;  
        private static int HEIGHT = 480;

        private CascadeClassifier hand = new CascadeClassifier("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/haar/OS/handcascade.xml");
        private List<Image<Bgr, Byte>> imgs = new List<Image<Bgr, Byte>>();

        private KinectSensor sensor;
        private WriteableBitmap colorBitmap;
        private byte[] colorPixels;
        private Image<Bgr, Byte> currentFrame;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            /*string path = "C:/Users/L33549.CITI/Desktop/Dropbox/FYPJ 2013 P3/Video/d.avi";
            _capture = new Capture(path);
            _capture.ImageGrabbed += loadAndProcess;
            _capture.Start();*/

            foreach (var potentialSensor in KinectSensor.KinectSensors)
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
            }
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
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
                    Bitmap b = new Bitmap(ms);
                    currentFrame = new Image<Bgr, Byte>(b);

                    if (currentFrame != null)
                    {
                        //handDec = new HandDetector("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/gloveHSV.txt", WIDTH, HEIGHT);
                        //handDec.update(currentFrame);
                        //Draw the image, the detected hand and finger info, and the average ms snap time at the bottom left of the panel.
                        //Graphics g = Graphics.FromImage(currentFrame.ToBitmap());
                        //handDec.draw(g);
                        Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
                        Rectangle[] handDetected = hand.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty, Size.Empty);
                        foreach (Rectangle hands in handDetected)
                        {
                            currentFrame.Draw(hands, new Bgr(System.Drawing.Color.Yellow), 2);
                            imgs.Add(currentFrame.Clone());
                        }
                        capturedImageBox.Image = currentFrame.Clone();
                    }
                }
            }
        }

        private void loadAndProcess(object sender, EventArgs e)
        {
            using (Image<Bgr, Byte> currentFrame = _capture.RetrieveBgrFrame().Clone())
            {
                if (currentFrame != null)
                {
                    //handDec = new HandDetector("C:/Users/L33549.CITI/Desktop/AbuseAnalysis/BodyDetectionAnalysis1/BodyDetectionAnalysis1/gloveHSV.txt", WIDTH, HEIGHT);
                    //handDec.update(currentFrame);

                    //Draw the image, the detected hand and finger info, and the average ms snap time at the bottom left of the panel.
                    //Graphics g = Graphics.FromImage(currentFrame.ToBitmap());
                    //handDec.draw(g);
                    //capturedImageBox.Image = currentFrame.Clone();

                    //System.Threading.Thread.Sleep(50);

                    Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
                    Rectangle[] handDetected = hand.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty, Size.Empty);
                    foreach (Rectangle hands in handDetected)
                    {
                        currentFrame.Draw(hands, new Bgr(System.Drawing.Color.Yellow), 2);
                        imgs.Add(currentFrame.Clone());
                    }
                    capturedImageBox.Image = currentFrame.Clone();
                }
            }
        }
    }
}
