using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.Drawing.Imaging;
using System.Text;
using FYP_ChildAbuseGIS2013.SkinDetection;
using FYP_ChildAbuseGIS2013.ServiceInterface;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;

namespace FYP_ChildAbuseGIS2013
{
    public partial class share : System.Web.UI.Page
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
        private int totalFrame;

        //Temp Memory Storage
        private MemStorage contourStorage;

        //Face Detection
        private CascadeClassifier faceHaar;
        private Rectangle[] faceDetect;
        private Point facePoint;
        private List<string> faceEmotions;

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
        private List<long> runTimes;

        //initialise and save file path variable
        string strVideoPath, savePath;

        //Get the geocorrindates of image or video variables.
        byte[] byte_property_id;
        string ascii_string_property_id, prop_type;
        int[] property_ids;
        int counter;
        double degrees, minutes, seconds, lat_dd, long_dd;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool result = Convert.ToBoolean(Session["result"]);

            faceHaar = new CascadeClassifier("C:/Emgu/opencv_attic/opencv/data/haarcascades/haarcascade_frontalface_default.xml");

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
            runTimes = new List<long>();
            faceEmotions = new List<string>();
        }

        private void grabVideo(string path)
        {
            grabber = new Capture(path);
            frameHeight = grabber.Height;
            frameWidth = grabber.Width;
        }
        
        protected void upload_Click(object sender, EventArgs e)
        {
            string title = tbTitle.Text.ToString();
            DateTime date = DateTime.Now;
            string filepath = txt_fileUpLoad.Text;
            string desc = tbDescription.Text.ToString();
            string type = "";

            /*CreateAnalysis cAna = new CreateAnalysis(200, 20, 50, 30, 100, 20, 10, 15, 5);
            CreateLocation cLoc = new CreateLocation("Testing", 29830.4695669479, 40135.9793048648);
            CreateFile cFile = new CreateFile("testing", date, "testing.avi", "ascasc", "Image");

            insertRecord(cAna, cLoc, cFile);*/

            if (fileUpLoad.HasFile)
            {
                string fileType = Path.GetExtension(filepath);
                if (fileType == ".avi")
                {
                    type = "Video";
                    strVideoPath = fileUpLoad.PostedFile.FileName.ToString();
                    savePath = Server.MapPath("~\\video\\");
                    fileUpLoad.PostedFile.SaveAs(savePath + strVideoPath);
                    grabVideo(savePath + strVideoPath);
                    videoAnalysis();
                }
                else if (fileType == ".jpg")
                {
                    type = "Image";
                    txt_fileUpLoad.Text = "";
                    strVideoPath = fileUpLoad.PostedFile.FileName.ToString();
                    savePath = Server.MapPath("~\\video\\");
                    fileUpLoad.PostedFile.SaveAs(savePath + strVideoPath);
                    Bitmap pic = new Bitmap(savePath + strVideoPath);
                    property_ids = pic.PropertyIdList;

                    foreach (int scan_property in property_ids)
                    {
                        byte_property_id = pic.GetPropertyItem(scan_property).Value;
                        prop_type = pic.GetPropertyItem(scan_property).Type.ToString();

                        if (scan_property == 1)
                        {
                            //Latitude North or South
                            ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id);
                            //resultDisplay.InnerText = ascii_string_property_id +",Latitude";
                        }
                        else if (scan_property == 2)
                        {
                            //Latitude degrees minutes and seconds (rational)
                            degrees = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4);
                            minutes = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12);
                            seconds = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20);

                            lat_dd = degrees + (minutes / 60) + (seconds / 3600); //->Latitude

                            //resultDisplay.InnerText += degrees + " " + minutes + " " + seconds + " " + lat_dd + ", ";
                        }
                        else if (scan_property == 3)
                        {
                            //Longitude East or West
                            ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id);

                            //resultDisplay.InnerText += ascii_string_property_id + ",Longitude";
                        }
                        else if (scan_property == 4)
                        {

                            //Longitude degrees minutes and seconds (rational)
                            degrees = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4);
                            minutes = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12);
                            seconds = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20);
                            long_dd = degrees + (minutes / 60) + (seconds / 3600); //-->longtitude

                            //resultDisplay.InnerText += degrees + " " + minutes + " " + seconds + " " + long_dd;
                        }

                        else if (scan_property == 18)
                        {
                            //Datum used at GPS acquisition (ascii)
                            ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id);
                            // resultDisplay.InnerText += "GPS Datum= " + ascii_string_property_id;
                        }
                        else
                        {
                            if (scan_property == 24)
                            {
                                //Magnetic bearing of subject to photographer (rational)
                                //do nothing
                            }
                            //scan_property ++;
                        }
                    }
                    //save data
                }
            }
        }

        private void insertRecord(CreateAnalysis cAna, CreateLocation cLoc, CreateFile cFile)
        {
            AnalysisService analysis = new AnalysisService();
            CreateAnalysisResult anaResult = analysis.Post(cAna);
            int analysisID = anaResult.Analysis.ID;
            cFile.analysisid = analysisID;

            LocationsService location = new LocationsService();
            CreateLocationResult locResult = location.Post(cLoc);
            int locationID = locResult.Location.ID;
            cFile.locationid = locationID;

            FileService file = new FileService();
            file.Post(cFile);
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

                    //defaultFrame.Image = editedSkinFrame;
                    //blackFrame.Image = skin;
                    System.Windows.Forms.Application.DoEvents();
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
            Image<Gray, byte>[] imageList = new Image<Gray, Byte>[70];
            imageList[0] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/netural1.jpg");
            imageList[1] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/sad1.jpg");
            imageList[2] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/angry1.jpg");
            imageList[3] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy1.jpg");
            imageList[4] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy2.jpg");
            imageList[5] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy3.jpg");
            imageList[6] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy4.jpg");
            imageList[7] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy5.jpg");
            imageList[8] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy6.jpg");
            imageList[9] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy7.jpg");
            imageList[10] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/375.jpg");
            imageList[11] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/376.jpg");
            imageList[12] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/377.jpg");
            imageList[13] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/378.jpg");
            imageList[14] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/379.jpg");
            imageList[15] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/380.jpg");
            imageList[16] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/381.jpg");
            imageList[17] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/382.jpg");
            imageList[18] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/383.jpg");
            imageList[19] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/384.jpg");
            imageList[20] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/385.jpg");
            imageList[21] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/386.jpg");
            imageList[22] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/387.jpg");
            imageList[23] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/388.jpg");
            imageList[24] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/389.jpg");
            imageList[25] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/390.jpg");
            imageList[26] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/391.jpg");
            imageList[27] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/392.jpg");
            imageList[28] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/393.jpg");
            imageList[29] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/394.jpg");
            imageList[30] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/395.jpg");
            imageList[31] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/396.jpg");
            imageList[32] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/545.jpg");
            imageList[33] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/546.jpg");
            imageList[34] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/547.jpg");
            imageList[35] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/548.jpg");
            imageList[36] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/579.jpg");
            imageList[37] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/580.jpg");
            imageList[38] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/581.jpg");
            imageList[39] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/582.jpg");
            imageList[40] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/583.jpg");
            imageList[41] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/584.jpg");
            imageList[42] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/585.jpg");
            imageList[43] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/586.jpg");
            imageList[44] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/587.jpg");
            imageList[45] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/588.jpg");
            imageList[46] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/589.jpg");
            imageList[47] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/590.jpg");
            imageList[48] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/557.jpg");
            imageList[49] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/558.jpg");
            imageList[50] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/559.jpg");
            imageList[51] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/560.jpg");
            imageList[52] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/561.jpg");
            imageList[53] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/562.jpg");
            imageList[54] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/563.jpg");
            imageList[55] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/564.jpg");
            imageList[56] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/565.jpg");
            imageList[57] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/566.jpg");
            imageList[58] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/567.jpg");
            imageList[59] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/568.jpg");
            imageList[60] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/569.jpg");
            imageList[61] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/570.jpg");
            imageList[62] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/571.jpg");
            imageList[63] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/572.jpg");
            imageList[64] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/573.jpg");
            imageList[65] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/574.jpg");
            imageList[66] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/575.jpg");
            imageList[67] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/576.jpg");
            imageList[68] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/577.jpg");
            imageList[69] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/578.jpg");

            String[] emoList = new String[70];
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
            emoList[10] = "Angry";
            emoList[11] = "Angry";
            emoList[12] = "Angry";
            emoList[13] = "Angry";
            emoList[14] = "Angry";
            emoList[15] = "Angry";
            emoList[16] = "Angry";
            emoList[17] = "Angry";
            emoList[18] = "Angry";
            emoList[19] = "Angry";
            emoList[20] = "Angry";
            emoList[21] = "Angry";
            emoList[22] = "Angry";
            emoList[23] = "Angry";
            emoList[24] = "Angry";
            emoList[25] = "Angry";
            emoList[26] = "Angry";
            emoList[27] = "Angry";
            emoList[28] = "Angry";
            emoList[29] = "Angry";
            emoList[30] = "Angry";
            emoList[31] = "Angry";
            emoList[32] = "Angry";
            emoList[33] = "Angry";
            emoList[34] = "Angry";
            emoList[35] = "Angry";
            emoList[36] = "Angry";
            emoList[37] = "Angry";
            emoList[38] = "Angry";
            emoList[39] = "Angry";
            emoList[40] = "Angry";
            emoList[41] = "Angry";
            emoList[42] = "Angry";
            emoList[43] = "Angry";
            emoList[44] = "Angry";
            emoList[45] = "Angry";
            emoList[46] = "Angry";
            emoList[47] = "Angry";
            emoList[48] = "Angry";
            emoList[49] = "Angry";
            emoList[50] = "Angry";
            emoList[51] = "Angry";
            emoList[52] = "Angry";
            emoList[53] = "Angry";
            emoList[54] = "Angry";
            emoList[55] = "Angry";
            emoList[56] = "Angry";
            emoList[57] = "Angry";
            emoList[58] = "Angry";
            emoList[59] = "Angry";
            emoList[60] = "Angry";
            emoList[61] = "Angry";
            emoList[62] = "Angry";
            emoList[63] = "Angry";
            emoList[64] = "Angry";
            emoList[65] = "Angry";
            emoList[66] = "Angry";
            emoList[67] = "Angry";
            emoList[68] = "Angry";
            emoList[69] = "Angry";

            int[] label = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69 };
            //Use the variable to detect the number of times these emotion appear in the video by frames;
            int smile = 0;
            int angry = 0;
            int netural = 0;
            int sad = 0;
            int noDetect = 0;
            totalFrame = grayFrameList.Count;
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

            for (int i = 0; i < totalFrame; i++)
            {
                result = fisher.Predict(grayFrameList.ElementAt(i));
                //lbmsg.Text = totalFrame.ToString();
                int num = result.Label;

                if (num == -1)
                {
                    faceEmotions.Add("No Detect");
                }
                else
                {
                    faceEmotions.Add(emoList[num].ToString());
                }

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

            if (angry > smile && angry > sad && angry > netural)
            {
                //lbEmotionConclusion2.Text = "There is a risk of child abuse";
            }

            //get total sum of the 5 emotion data.
            int total = smile + sad + angry + netural + noDetect;
            //put them into the percentage (emotion/frames * 100)
            double smilePer = (Convert.ToDouble(smile) / totalFrame) * 100;
            double sadPer = (Convert.ToDouble(sad) / totalFrame) * 100;
            double angryPer = (Convert.ToDouble(angry) / totalFrame) * 100;
            double neturalPer = (Convert.ToDouble(netural) / totalFrame) * 100;
            double noDetectPer = (Convert.ToDouble(noDetect) / totalFrame) * 100;

            //lbmsg.Text = "Smile Count:" + smile.ToString() + " ,Sad Count:" + sad.ToString() + " ,Angry Count:" + angry.ToString() + " ,Netural Count:" + netural.ToString() + " ,No Detect Count:" + noDetect.ToString();
            String[] emotionList = { "Smile", "Sad", "Angry", "Netural", "No Detect" };
            double[] emotionPer = { smilePer, sadPer, angryPer, neturalPer, noDetectPer };
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

            while (contours != null)
            {
                if (detectAndDrawHand(contours) == true)
                {
                    handList.Add(contours);
                }
                contours = contours.HNext;
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
                    if ((gesture.Equals("Left Palm")) || (gesture.Equals("Left Fist")) || (gesture.Equals("Right Palm")) || (gesture.Equals("Right Fist")))
                    {
                        PointF newCenter = new PointF(centerX, centerY);
                        handMotionList.Add(skin);
                        motionPoints.Add(newCenter);
                        gestures.Add(gesture);
                        runTimes.Add(time / 1000);
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
                //lbLeftHand.Text = "Left found";
                if (size < 2)
                {
                    //lbLeftHand.Text = "Fist";
                    gesture = "Left Fist";
                }
                else if (size > 3)
                {
                    //lbLeftHand.Text = "Palm";
                    gesture = "Left Palm";
                }
                else if ((size > 1) && (size < 4))
                {
                    //lbLeftHand.Text = size + " fingers";
                }
            }
            if (facePoint.X - 10 > centerX)
            {
                //lbRightHand.Text = "Right found";
                if (size < 2)
                {
                    //lbRightHand.Text = "Fist";
                    gesture = "Right Fist";
                }
                else if (size > 3)
                {
                    //lbRightHand.Text = "Palm";
                    gesture = "Right Palm";
                }
                else if ((size > 1) && (size < 4))
                {
                    //lbRightHand.Text = size + " fingers";
                }
            }
            if ((facePoint.X + 70 > centerX) && (facePoint.X - 10 < centerX))
            {
                //lbLeftHand.Text = "NULL";
                //lbRightHand.Text = "NULL";
            }
            return gesture;
        }

        private void handMotionAnalysis()
        {
            List<int> leftFists = new List<int>();
            List<int> rightFists = new List<int>();
            List<int> leftPalm = new List<int>();
            List<int> rightPalm = new List<int>();

            if (handMotionList != null)
            {
                for (int i = 0; i < handMotionList.Count(); i++)
                {
                    if (gestures.ElementAt(i).ToString().Equals("Left Fist"))
                    {
                        leftFists.Add(i);
                    }
                    else if (gestures.ElementAt(i).ToString().Equals("Right Fist"))
                    {
                        rightFists.Add(i);
                    }
                    else if (gestures.ElementAt(i).ToString().Equals("Left Palm"))
                    {
                        leftPalm.Add(i);
                    }
                    else if (gestures.ElementAt(i).ToString().Equals("Right Palm"))
                    {
                        rightPalm.Add(i);
                    }
                }
            }

            double leftFistWarningPer = (analysisLeftGestureMotion(leftFists) / totalFrame) * 100;
            double leftPalmWarningPer = (analysisLeftGestureMotion(leftPalm) / totalFrame) * 100;
            double rightFistWarningPer = (analysisRightGestureMotion(rightFists) / totalFrame) * 100;
            double rightPalmWarningPer = (analysisRightGestureMotion(rightPalm) / totalFrame) * 100;
            double safePer = ((totalFrame - leftFistWarningPer - leftPalmWarningPer - rightFistWarningPer - rightPalmWarningPer) / totalFrame) * 100;

            String[] motionList = { "Left Fist", "Left Slap", "Right Fist", "Right Slap", "No Detect" };
            double[] motionPer = { leftFistWarningPer, leftPalmWarningPer, rightFistWarningPer, rightPalmWarningPer, safePer };

            lb_msg.Text = "Total Frame: " + totalFrame.ToString() + "Left Fist: " + leftFistWarningPer.ToString() + "Right Fist: " + rightFistWarningPer.ToString();
        }

        private double analysisLeftGestureMotion(List<int> gestures)
        {
            long lastMotionTime = 0;
            PointF lastMotionPosition = new PointF();
            List<double> speeds = new List<double>();
            double speed = 0;
            double warningFrame = 0;
            double currentWarning = 0;

            //Check if gesture is empty
            if (gestures != null)
            {
                for (int i = 0; i < gestures.Count(); i++)
                {
                    //Get the id of all same gesture
                    int current = gestures.ElementAt(i);
                    if (i == 0)
                    {
                        //Set time and position if i = 0
                        lastMotionTime = runTimes.ElementAt(current);
                        lastMotionPosition = motionPoints.ElementAt(current);
                    }
                    else
                    {
                        long currentMotionTime = runTimes.ElementAt(current);
                        PointF currentMotionPosition = motionPoints.ElementAt(current);
                        float lastX = lastMotionPosition.X;
                        float lastY = lastMotionPosition.Y;
                        float currentX = currentMotionPosition.X;
                        float currentY = currentMotionPosition.Y;

                        if ((currentMotionTime < lastMotionTime + 5) && (currentX < lastX) && (currentY < lastY))
                        {
                            double distance = Math.Sqrt(Math.Pow((lastX - currentX), 2) + Math.Pow((lastY - currentY), 2));
                            double time = currentMotionTime - lastMotionTime;
                            double s = distance / time;
                            speed = speed + s;
                            currentWarning = currentWarning + 1;
                        }
                        else
                        {
                            if ((speed > 1) && (double.IsInfinity(speed) == false) && (double.IsNaN(speed) == false))
                            {
                                speeds.Add(speed);
                                warningFrame = warningFrame + currentWarning;
                            }
                            speed = 0;
                            currentWarning = 0;
                        }
                        lastMotionTime = currentMotionTime;
                        lastMotionPosition = currentMotionPosition;
                    }
                }
            }
            return warningFrame;
        }

        private double analysisRightGestureMotion(List<int> gestures)
        {
            long lastMotionTime = 0;
            PointF lastMotionPosition = new PointF();
            List<double> speeds = new List<double>();
            double speed = 0;
            double warningFrame = 0;
            double currentWarning = 0;

            //Check if gesture is empty
            if (gestures != null)
            {
                for (int i = 0; i < gestures.Count(); i++)
                {
                    //Get the id of all same gesture
                    int current = gestures.ElementAt(i);
                    if (i == 0)
                    {
                        //Set time and position if i = 0
                        lastMotionTime = runTimes.ElementAt(current);
                        lastMotionPosition = motionPoints.ElementAt(current);
                    }
                    else
                    {
                        long currentMotionTime = runTimes.ElementAt(current);
                        PointF currentMotionPosition = motionPoints.ElementAt(current);
                        float lastX = lastMotionPosition.X;
                        float lastY = lastMotionPosition.Y;
                        float currentX = currentMotionPosition.X;
                        float currentY = currentMotionPosition.Y;

                        if ((currentMotionTime < lastMotionTime + 5) && (currentX < lastX) && (currentY < lastY))
                        {
                            double distance = Math.Sqrt(Math.Pow((currentX - lastX), 2) + Math.Pow((currentY - lastY), 2));
                            double time = currentMotionTime - lastMotionTime;
                            double s = distance / time;
                            speed = speed + s;
                            currentWarning = currentWarning + 1;
                        }
                        else
                        {
                            if ((speed > 1) && (double.IsInfinity(speed) == false) && (double.IsNaN(speed) == false))
                            {
                                speeds.Add(speed);
                                warningFrame = warningFrame + currentWarning;
                            }
                            speed = 0;
                            currentWarning = 0;
                        }
                        lastMotionTime = currentMotionTime;
                        lastMotionPosition = currentMotionPosition;
                    }
                }
            }
            return warningFrame;
        }

        void FaceAnalysis()
        {
            //start to do the face analyze
            //Member Variable 
            Image<Bgr, Byte> imgNormal;
            Image<Gray, Byte> imgProcess;

            List<Image<Bgr, Byte>> imgNormalList;
            List<Image<Gray, Byte>> imgProcessList;
            Capture video;

            Rectangle[] faceDetect;
            CascadeClassifier facefile = new CascadeClassifier("C:/Emgu/opencv_attic/opencv/data/haarcascades/haarcascade_frontalface_default.xml");

            bool reading = true;
            imgNormalList = new List<Image<Bgr, byte>>();
            imgProcessList = new List<Image<Gray, byte>>();
            video = new Capture(savePath + strVideoPath);
            //Application.DoEvents();
            Stopwatch swatch = new Stopwatch();
            int m = 0;

            while (reading)
            {
                imgNormal = video.QueryFrame();
                if (imgNormal != null)
                {
                    imgProcess = imgNormal.Convert<Gray, Byte>();
                    faceDetect = facefile.DetectMultiScale(imgProcess, 1.1, 20, new Size(20, 20), Size.Empty);
                    foreach (Rectangle ac in faceDetect)
                    {
                        imgProcessList.Add(imgProcess.Copy(ac).Resize(64, 64, INTER.CV_INTER_CUBIC));
                        //imgNormal.Draw(ac, new Bgr(Color.Blue), 2);
                    }
                    imgNormalList.Add(imgNormal);
                    //imgNormal.Save("C:/Users/L33506/Desktop/emotionData/"+m+".jpg");
                    //imageBox.Image = imgNormal;
                    //complete.Text = imgProcessList.Count.ToString();

                    //Application.DoEvents();
                    System.Threading.Thread.Sleep(1000 / 10);
                    int time = 10000;
                    if (swatch.ElapsedMilliseconds >= time)
                    {
                        reading = false;
                    }
                }
                else
                {
                    reading = false;

                }
                m++;
            }

            //Analysis for face emotion

            //Data for face emotion when data base is not ready.
            Image<Gray, byte>[] imageList = new Image<Gray, Byte>[70];
            imageList[0] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/netural1.jpg");
            imageList[1] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/sad1.jpg");
            imageList[2] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/angry1.jpg");
            imageList[3] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy1.jpg");
            imageList[4] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy2.jpg");
            imageList[5] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy3.jpg");
            imageList[6] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy4.jpg");
            imageList[7] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy5.jpg");
            imageList[8] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy6.jpg");
            imageList[9] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/happy7.jpg");
            imageList[10] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/375.jpg");
            imageList[11] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/376.jpg");
            imageList[12] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/377.jpg");
            imageList[13] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/378.jpg");
            imageList[14] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/379.jpg");
            imageList[15] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/380.jpg");
            imageList[16] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/381.jpg");
            imageList[17] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/382.jpg");
            imageList[18] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/383.jpg");
            imageList[19] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/384.jpg");
            imageList[20] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/385.jpg");
            imageList[21] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/386.jpg");
            imageList[22] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/387.jpg");
            imageList[23] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/388.jpg");
            imageList[24] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/389.jpg");
            imageList[25] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/390.jpg");
            imageList[26] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/391.jpg");
            imageList[27] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/392.jpg");
            imageList[28] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/393.jpg");
            imageList[29] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/394.jpg");
            imageList[30] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/395.jpg");
            imageList[31] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/396.jpg");
            imageList[32] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/545.jpg");
            imageList[33] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/546.jpg");
            imageList[34] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/547.jpg");
            imageList[35] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/548.jpg");
            imageList[36] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/579.jpg");
            imageList[37] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/580.jpg");
            imageList[38] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/581.jpg");
            imageList[39] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/582.jpg");
            imageList[40] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/583.jpg");
            imageList[41] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/584.jpg");
            imageList[42] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/585.jpg");
            imageList[43] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/586.jpg");
            imageList[44] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/587.jpg");
            imageList[45] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/588.jpg");
            imageList[46] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/589.jpg");
            imageList[47] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/590.jpg");
            imageList[48] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/557.jpg");
            imageList[49] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/558.jpg");
            imageList[50] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/559.jpg");
            imageList[51] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/560.jpg");
            imageList[52] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/561.jpg");
            imageList[53] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/562.jpg");
            imageList[54] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/563.jpg");
            imageList[55] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/564.jpg");
            imageList[56] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/565.jpg");
            imageList[57] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/566.jpg");
            imageList[58] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/567.jpg");
            imageList[59] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/568.jpg");
            imageList[60] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/569.jpg");
            imageList[61] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/570.jpg");
            imageList[62] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/571.jpg");
            imageList[63] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/572.jpg");
            imageList[64] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/573.jpg");
            imageList[65] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/574.jpg");
            imageList[66] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/575.jpg");
            imageList[67] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/576.jpg");
            imageList[68] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/577.jpg");
            imageList[69] = new Image<Gray, byte>("C:/Users/L33506/Desktop/emotionData/578.jpg");


            String[] emoList = new String[70];
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
            emoList[10] = "Angry";
            emoList[11] = "Angry";
            emoList[12] = "Angry";
            emoList[13] = "Angry";
            emoList[14] = "Angry";
            emoList[15] = "Angry";
            emoList[16] = "Angry";
            emoList[17] = "Angry";
            emoList[18] = "Angry";
            emoList[19] = "Angry";
            emoList[20] = "Angry";
            emoList[21] = "Angry";
            emoList[22] = "Angry";
            emoList[23] = "Angry";
            emoList[24] = "Angry";
            emoList[25] = "Angry";
            emoList[26] = "Angry";
            emoList[27] = "Angry";
            emoList[28] = "Angry";
            emoList[29] = "Angry";
            emoList[30] = "Angry";
            emoList[31] = "Angry";
            emoList[32] = "Angry";
            emoList[33] = "Angry";
            emoList[34] = "Angry";
            emoList[35] = "Angry";
            emoList[36] = "Angry";
            emoList[37] = "Angry";
            emoList[38] = "Angry";
            emoList[39] = "Angry";
            emoList[40] = "Angry";
            emoList[41] = "Angry";
            emoList[42] = "Angry";
            emoList[43] = "Angry";
            emoList[44] = "Angry";
            emoList[45] = "Angry";
            emoList[46] = "Angry";
            emoList[47] = "Angry";
            emoList[48] = "Angry";
            emoList[49] = "Angry";
            emoList[50] = "Angry";
            emoList[51] = "Angry";
            emoList[52] = "Angry";
            emoList[53] = "Angry";
            emoList[54] = "Angry";
            emoList[55] = "Angry";
            emoList[56] = "Angry";
            emoList[57] = "Angry";
            emoList[58] = "Angry";
            emoList[59] = "Angry";
            emoList[60] = "Angry";
            emoList[61] = "Angry";
            emoList[62] = "Angry";
            emoList[63] = "Angry";
            emoList[64] = "Angry";
            emoList[65] = "Angry";
            emoList[66] = "Angry";
            emoList[67] = "Angry";
            emoList[68] = "Angry";
            emoList[69] = "Angry";

            int[] label = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69 };
            //Use the variable to detect the number of times these emotion appear in the video by frames;
            int smile = 0;
            int angry = 0;
            int netural = 0;
            int sad = 0;
            int noDetect = 0;
            int frames = imgProcessList.Count;

            List<Image<Gray, byte>> trainfaceList = new List<Image<Gray, byte>>();

            Rectangle[] trainFace;
            for (int i = 0; i < imageList.Length; i++)
            {
                trainFace = facefile.DetectMultiScale(imageList[i],
                1.1,
                20,
                new Size(20, 20),
                Size.Empty);

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

                result = fisher.Predict(imgProcessList.ElementAt(i));
                //lbmsg.Text = frames.ToString();
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

            if (angry > smile && angry > sad && angry > netural)
            {
                //label2.Text = "There is a risk of child abuse";
            }
            //get total sum of the 5 emotion data.
            int total = smile + sad + angry + netural + noDetect;
            //put them into the percentage (emotion/frames * 100)
            double smilePer = (Convert.ToDouble(smile) / frames) * 100;
            double sadPer = (Convert.ToDouble(sad) / frames) * 100;
            double angryPer = (Convert.ToDouble(angry) / frames) * 100;
            double neturalPer = (Convert.ToDouble(netural) / frames) * 100;
            double noDetectPer = (Convert.ToDouble(noDetect) / frames) * 100;


            //lbmsg.Text = "Smile Count:"+smile.ToString()+" ,Sad Count:"+sad.ToString()+" ,Angry Count:" + angry.ToString()+" ,Netural Count:"+ netural.ToString()+" ,No Detect Count:"+noDetect.ToString();
            String[] emotionList = { "Smile", "Sad", "Angry", "Netural", "No Detect" };
            double[] emotionPer = { smilePer, sadPer, angryPer, neturalPer, noDetectPer };
            
            /**
            //Designing the Pie Chart 
            chart1.Series.Add(new Series());
            chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Pie; // set the chart to pie
           // chart1.Series["Emotion"].ChartArea = "ChartArea1";
            chart1.ChartAreas.Add(new ChartArea());
            chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
            //Add data to pie chart

            /** 
           chart1.Series[0].Points.DataBindXY(emotionList, emotionPer);
       
                      chart1.Series[0].Points.AddXY("Smile",smilePer);
                      chart1.Series[0].Points.AddXY("Sad",sadPer);
                      chart1.Series[0].Points.AddXY("Angry",angryPer);
                      chart1.Series[0].Points.AddXY("Netural",neturalPer);
                      chart1.Series[0].Points.AddXY("No Detect",noDetectPer);
             
            
            

              //SET THE COLOR PALETTE FOR THE CHART TO BE A PRESET OF NONE 
              //DEFINE OUR OWN COLOR PALETTE FOR THE CHART 
              chart1.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.None;
              chart1.PaletteCustomColors = new Color[] { Color.LightGreen, Color.Blue, Color.Red, Color.Gray, Color.Black };

            chart1.Titles.Add("Anaylsis of Face Expression (Frame By Frame)");
              chart1.Series[0]["PieLabelStyle"] = "Disabled";
              //chart1.Series["Emotion"].Label = "#VALX";
                        
              chart1.Legends.Add(new Legend());
              chart1.Legends[0].Enabled = true;
              chart1.Legends[0].Docking = Docking.Bottom;
              chart1.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
              chart1.Series[0].LegendText = "#VALX (#PERCENT{P2}) ";
              chart1.DataManipulator.Sort(PointSortOrder.Ascending, chart1.Series[0]);
              
            //SET THE IMAGE OUTPUT TYPE TO BE JPEG
              chart1.ImageType = System.Web.UI.DataVisualization.Charting.ChartImageType.Jpeg;
            
                
            //end of Analysis for face emotion
            **/
            //POST BACK
        }
    }
}