﻿using System;
using System.IO;
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
    class HandDetector
    {
        private static int IMG_SCALE = 2;  // scaling applied to webcam image

        private static float SMALLEST_AREA = 500.0f;    // was 100.0f; ignore smaller contour areas

        private static int MAX_POINTS = 20;   // max number of points stored in an array

        // used for simiplifying the defects list
        private static int MIN_FINGER_DEPTH = 20;
        private static int MAX_FINGER_ANGLE = 60;   // degrees

        // angle ranges of thumb and index finger of the left hand relative to its COG
        private static int MIN_THUMB = 120;
        private static int MAX_THUMB = 200;

        private static int MIN_INDEX = 60;
        private static int MAX_INDEX = 120;

        // HSV ranges defining the glove colour
        private int hueLower, hueUpper, satLower, satUpper, briLower, briUpper;

        // EmguCV elements
        private Image<Bgr, Byte> scaleImg;     // for resizing the webcam image
        private Image<Hsv, Byte> hsvImg;       // HSV version of webcam image
        private Image<Gray, Byte> imgThreshed;  // threshold for HSV settings
        private MemStorage contourStorage, approxStorage, hullStorage, defectsStorage;

        private Font msgFont;

        // hand details
        private Point cogPt;           // center of gravity (COG) of contour
        private int contourAxisAngle;  // contour's main axis angle relative to the horizontal (in degrees)

        // defects data for the hand contour
        private Point[] tipPts, foldPts;
        private float[] depths;
        private List<Point> fingerTips;
        private Seq<Point> hull;
        private Seq<MCvConvexityDefect> defects;
        private MCvConvexityDefect[] defectArray;

        // finger identifications
        private List<FingerNameClass.FingerName> namedFingers;

        public HandDetector(string hsvFnm, int width, int height)
        {
            Size scale = new Size(width/IMG_SCALE, height/IMG_SCALE);
            scaleImg = new Image<Bgr, Byte>(scale);
            hsvImg = new Image<Hsv, Byte>(scale);
            imgThreshed = new Image<Gray, Byte>(scale);

            // storage for contour, hull, and defect calculations by OpenCV
            contourStorage = new MemStorage();
            approxStorage = new MemStorage();
            hullStorage = new MemStorage();
            defectsStorage = new MemStorage();

            msgFont = new Font("SansSerif", 18, FontStyle.Bold, GraphicsUnit.Pixel);

            cogPt = new Point();
            fingerTips = new List<Point>();
            namedFingers = new List<FingerNameClass.FingerName>();

            tipPts = new Point[MAX_POINTS];   // coords of the finger tips
            foldPts = new Point[MAX_POINTS];  // coords of the skin folds between fingers
            depths = new float[MAX_POINTS];   // distances from tips to folds

            hueLower = 0;
            hueUpper = 20;
            satLower = 50;
            satUpper = 255;
            briLower = 0;
            briUpper = 255;
        }

        public void update(Image<Bgr, Byte> im)
        {
            CvInvoke.cvResize(im, scaleImg, INTER.CV_INTER_CUBIC); // reduce the size of the image to make processing faster

            // convert image format to HSV
            CvInvoke.cvCvtColor(scaleImg, hsvImg, COLOR_CONVERSION.CV_BGR2HSV);

            // threshold image using loaded HSV settings for user's glove
            CvInvoke.cvInRangeS(hsvImg, new MCvScalar(hueLower, satLower, briLower, 0),
                                    new MCvScalar(hueUpper, satUpper, briUpper, 0),
                                    imgThreshed);

            CvInvoke.cvMorphologyEx(imgThreshed, imgThreshed, imgThreshed, imgThreshed, CV_MORPH_OP.CV_MOP_OPEN, 1);

            // erosion followed by dilation on the image to remove specks of white while retaining the image size
            /*List<Contour<Point>> bigContour = findBiggestContour(imgThreshed);
            if (bigContour == null)
            {
                return;
            }
            for (int i = 0; i < 1; i++)
            {
                if (bigContour.ElementAt(i) != null)
                {
                    extractContourInfo(bigContour.ElementAt(i), IMG_SCALE, im); // find the COG and angle to horizontal of the contour
                    findFingerTips(bigContour.ElementAt(i), IMG_SCALE); // detect the fingertips positions in the contour
                    nameFingers(cogPt, contourAxisAngle, fingerTips);
                }
            }*/

            Contour<Point> bigContour = findBiggestContour(imgThreshed);
            if (bigContour == null)
            {
                return;
            }
            extractContourInfo(bigContour, IMG_SCALE, im); // find the COG and angle to horizontal of the contour
            findFingerTips(bigContour, IMG_SCALE); // detect the fingertips positions in the contour
            nameFingers(cogPt, contourAxisAngle, fingerTips);
        }

        private Bitmap scaleImage(Bitmap im, int scale) // scaling makes the image faster to process
        {
            int nWidth = im.Width / scale;
            int nHeight = im.Height / scale;
            Bitmap smallIm = new Bitmap(nWidth, nHeight);
            Graphics g2 = Graphics.FromImage(smallIm);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            Rectangle rect = new Rectangle(0, 0, nWidth, nHeight);
            g2.DrawImage(im, rect, 0, 0, im.Width, im.Height, GraphicsUnit.Pixel);
            g2.Dispose();
            return smallIm;
        }

        private Contour<Point> findBiggestContour(Image<Gray, Byte> imgThreshed)
        {
            Contour<Point> contours = imgThreshed.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_LIST, contourStorage);
            Contour<Point> bigContour = null;
            Double current = 0;
            Double largest = 0;

            while (contours != null)
            {
                current = contours.Area;
                if (current > largest)
                {
                    largest = current;
                    bigContour = contours;
                }
                contours = contours.HNext;
            }

            return bigContour;
        }

        private List<Contour<Point>> findDiffContour(Image<Gray, Byte> imgThreshed)
        {
            List<Contour<Point>> contourList = new List<Contour<Point>>();
            Contour<Point> bigContour = null;
            Contour<Point> secondContour = null;
            Contour<Point> thirdContour = null;
            float maxArea = 500.0f;
            float nxtMaxArea = 300.0f;
            float lastMaxArea = 250.0f;
            MCvBox2D box = new MCvBox2D();
            // generate all the contours in the threshold image as a list
            for (Contour<Point> contours = imgThreshed.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_LIST, contourStorage); contours != null; contours = contours.HNext)
            {
                // find the largest contour in the list based on bounded box size
                if (contours.MCvSeq.elem_size > 0)
                {
                    box = CvInvoke.cvMinAreaRect2(contours, contourStorage);
                    SizeF size = box.size;
                    float area = size.ToPointF().X * size.ToPointF().Y;
                    if (area > maxArea)
                    {
                        contourList.Remove(bigContour);
                        maxArea = area;
                        bigContour = contours;
                        contourList.Add(bigContour);
                    }
                    if ((area < maxArea) && (area > nxtMaxArea) & (area > lastMaxArea))
                    {
                        contourList.Remove(secondContour);
                        nxtMaxArea = area;
                        secondContour = contours;
                        contourList.Add(secondContour);
                    }
                    if ((area < maxArea) && (area < nxtMaxArea) && (area > lastMaxArea))
                    {
                        contourList.Remove(thirdContour);
                        lastMaxArea = area;
                        thirdContour = contours;
                        contourList.Add(thirdContour);
                    }
                }
            }
            return contourList;
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
                cogPt = new Point(xCenter, yCenter);
                Size s = new Size(bigContour.MCvContour.rect.Height, bigContour.MCvContour.rect.Width);
                Rectangle rect = new Rectangle(cogPt, s);
                im.Draw(rect, new Bgr(Color.Red), 2);
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

        private double toDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
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

        private void findFingerTips(Contour<Point> bigContour, int scale)
        {
            Contour<Point> appContour = bigContour.ApproxPoly(bigContour.Perimeter * 0.0025, contourStorage);
            hull = bigContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            defects = bigContour.GetConvexityDefacts(contourStorage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            defectArray = defects.ToArray();

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

        private int angleBetween(Point tip, Point next, Point prev) // calculate the angle between the tip and its neighboring folds (in integer degrees)
        {
            return Math.Abs((int)Math.Round(toDegree(Math.Atan2(next.X - tip.X, next.Y - tip.Y) - Math.Atan2(prev.X - tip.X, prev.Y - tip.Y))));
        }

        private void nameFingers(Point cogPt, int contourAxisAngle, List<Point> fingerTips)
        {
            namedFingers.Clear();
            for (int i = 0; i < fingerTips.Count; i++)
            {
                namedFingers.Add(FingerNameClass.FingerName.UNKNOWN);
            }

            labelThumbIndex(fingerTips, namedFingers);
            labelUnknowns(namedFingers);
        }

        private void labelThumbIndex(List<Point> fingerTips, List<FingerNameClass.FingerName> nms)
        {
            bool foundThumb = false;
            bool foundIndex = false;
            int i = fingerTips.Count - 1;
            while ((i >= 0))
            {
                int angle = angleToCOG(fingerTips.ElementAt(i), cogPt, contourAxisAngle);
                //check for thumb
                if ((angle <= MAX_THUMB) && (angle > MIN_THUMB) && !foundThumb)
                {
                    nms.RemoveAt(i);
                    nms.Insert(i, FingerNameClass.FingerName.THUMB);
                    foundThumb = true;
                }

                // check for index
                if ((angle <= MAX_INDEX) && (angle > MIN_INDEX) && !foundIndex)
                {
                    nms.RemoveAt(i);
                    nms.Insert(i, FingerNameClass.FingerName.INDEX);
                    foundIndex = true;
                }
                i--;
            }
        }

        private int angleToCOG(Point tipPt, Point cogPt, int contourAxisAngle)
        {
            int yOffset = cogPt.Y - tipPt.Y; // make y positive up screen
            int xOffset = tipPt.X - cogPt.X;
            double theta = Math.Atan2(yOffset, xOffset);
            int angleTip = (int)Math.Round(toDegree(theta));
            return angleTip + (90 - contourAxisAngle);
            // this ensures that the hand is orientated straight up
        }

        private void labelUnknowns(List<FingerNameClass.FingerName> nms)
        {
            // find first named finger
            int i = 0;
            while ((i < nms.Count) && (nms.ElementAt(i) == FingerNameClass.FingerName.UNKNOWN))
            {
                i++;
                if (i == nms.Count)
                { // no named fingers found, so give up
                    return;
                }
                FingerNameClass.FingerName name = nms.ElementAt(i);
                labelPrev(nms, i, name); // fill-in backwards
                labelFwd(nms, i, name); // fill-in forwards
            }
        }

        private void labelPrev(List<FingerNameClass.FingerName> nms, int i, FingerNameClass.FingerName name)// move backwards through fingers list labeling unknown fingers
        {
            FingerNameClass fnc = new FingerNameClass();
            i--;
            while ((i >= 0) && (name != FingerNameClass.FingerName.UNKNOWN))
            {
                if (nms.ElementAt(i) == FingerNameClass.FingerName.UNKNOWN)
                { // unknown finger
                    name = fnc.getPrev(name);
                    if (!usedName(nms, name))
                    {
                        nms.RemoveAt(i);
                        nms.Insert(i, name);
                    }
                }
                else
                { // finger is named already
                    name = nms.ElementAt(i);
                }
                i--;
            }
        }

        private void labelFwd(List<FingerNameClass.FingerName> nms, int i, FingerNameClass.FingerName name) // move forward through fingers list labelling unknown fingers
        {
            FingerNameClass fnc = new FingerNameClass();
            i++;
            while ((i < nms.Count) && (name != FingerNameClass.FingerName.UNKNOWN))
            {
                if (nms.ElementAt(i) == FingerNameClass.FingerName.UNKNOWN)
                {  // unknown finger
                    name = fnc.getNext(name);
                    if (!usedName(nms, name))
                    {
                        nms.RemoveAt(i);
                        nms.Insert(i, name);
                    }
                }
                else
                {    // finger is named already
                    name = nms.ElementAt(i);
                }
                i++;
            }
        }

        private bool usedName(List<FingerNameClass.FingerName> nms, FingerNameClass.FingerName name) // does the fingers list contain name already?
        {
            foreach (FingerNameClass.FingerName fn in nms)
                if (fn == name)
                {
                    return true;
                }
            return false;
        }

        public void draw(Graphics g)
        {
            if (fingerTips.Count == 0)
            {
                return;
            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            SolidBrush yellow = new SolidBrush(Color.Yellow);
            SolidBrush red = new SolidBrush(Color.Red);
            SolidBrush green = new SolidBrush(Color.Green);
            Pen penYellow = new Pen(yellow, 4);
            Pen penRed = new Pen(red, 4);
            Pen penGreen = new Pen(green, 4);
            // label tips in red or green, and draw lines to named tips
            for (int i = 0; i < fingerTips.Count; i++)
            {
                Point pt = fingerTips.ElementAt(i);
                if (namedFingers.ElementAt(i) == FingerNameClass.FingerName.UNKNOWN)
                {
                    g.DrawEllipse(penRed, cogPt.X, cogPt.Y, pt.X, pt.Y);
                    g.DrawString("" + i, msgFont, red, pt.X, pt.Y - 10);
                }
                else // draw yellow line to the named fingertip from COG
                {
                    g.DrawLine(penYellow, cogPt, pt);
                    g.DrawEllipse(penGreen, pt.X - 8, pt.Y - 8, 16, 16);
                    g.DrawString(namedFingers.ElementAt(i).ToString().ToLower(), msgFont, green, pt.X, pt.Y - 10);
                }
            }
            g.FillEllipse(green, cogPt.X - 8, cogPt.Y - 8, 16, 16);
        }
    }
}