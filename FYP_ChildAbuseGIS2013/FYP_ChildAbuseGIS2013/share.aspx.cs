using System;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;
using Emgu.Util;
using Emgu.CV.UI;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.Drawing.Imaging;
using System.Text;
using AviFile;
using ZedGraph;
using NAudio.Wave;
using FYP_ChildAbuseGIS2013;
using FYP_ChildAbuseGIS2013.SoundDetector;
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

        //Temp Memory Storage
        private MemStorage contourStorage;

        //Face Detection
        private CascadeClassifier faceHaar;
        private Rectangle[] faceDetect;
        private Point facePoint;
        private List<string> faceEmotions;
        private int noDetect;

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

        //Sound Detection
        private int tickStart = 0;
        private string[] files, paths;
        private int window;
        private LineItem myCurve1;
        private LineItem myCurve2;
        private string vfilepath = "";
        private string ext = "";

        //Fever Detection
        private List<int> newList = new List<int>();

        //initialise and save file path variable
        string strVideoPath, savePath;

        //Get the geocorrindates of image or video variables.
        byte[] byte_property_id;
        string ascii_string_property_id, prop_type;
        int[] property_ids;
        int counter;
        double degrees, minutes, seconds, lat_dd, long_dd;

        //Analysis Table
        private CreateAnalysis cAna;
        private int abuseper;
        private int totalFrame;
        private int smile;
        private int angry;
        private int sad;
        private int neutral;
        private int leftfist;
        private int rightfist;
        private int leftpalm;
        private int rightpalm;
        private bool soundresult;
        private string soundpath;
        private int feverresult;

        //Location Table
        private CreateLocation cLoc;
        private string address;
        private double x;
        private double y;

        //File Table
        private CreateFile cFile;
        private string title;
        private DateTime date;
        private string path;
        private string desc;
        private string type;
        private int locationid;
        private int analysisid;

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
            string filepath = txt_fileUpLoad.Text;
            title = tbTitle.Text.ToString();
            desc = tbDescription.Text.ToString();

            if (fileUpLoad.HasFile)
            {
                string fileType = Path.GetExtension(filepath);
                if (fileType == ".avi")
                {
                    //Get the address
                    address = tbLocation.Text.ToString();
                    double lng = Double.Parse(tbX.Text.ToString());
                    double lat = Double.Parse(tbY.Text.ToString());
                    string convert = CoordinatesConverter.CoordinatesConvertor(lng, lat, 4326, 3414);
                    string[] converting = convert.Split(',');
                    x = Double.Parse(converting[0]);
                    y = Double.Parse(converting[1]);

                    //Get the video filename for analysis
                    type = "Video";
                    strVideoPath = fileUpLoad.PostedFile.FileName.ToString();
                    savePath = Server.MapPath("~\\video\\");
                    fileUpLoad.PostedFile.SaveAs(savePath + strVideoPath);
                    path = savePath + strVideoPath;
                    FileInfo oFileInfo = new FileInfo(path);
                    if (oFileInfo != null || oFileInfo.Length == 0)
                    {
                        date = oFileInfo.CreationTime;
                    }
                    AviManager aviManager = new AviManager(path, true);
                    AudioStream audioStream = aviManager.GetWaveStream();
                    audioStream.ExportStream(path + ".wav");
                    vfilepath = path + ".wav";
                    aviManager.Close();
                    soundAnalysis();
                    grabVideo(path);
                    videoAnalysis();
                    insertIntoDatabase();
                }
                else if (fileType == ".jpg")
                {
                    type = "Image";
                    txt_fileUpLoad.Text = "";
                    strVideoPath = fileUpLoad.PostedFile.FileName.ToString();
                    savePath = Server.MapPath("~\\image\\");
                    fileUpLoad.PostedFile.SaveAs(savePath + strVideoPath);
                    Bitmap pic = new Bitmap(savePath + strVideoPath);
                    property_ids = pic.PropertyIdList;

                    foreach (int scan_property in property_ids)
                    {
                        byte_property_id = pic.GetPropertyItem(scan_property).Value;
                        prop_type = pic.GetPropertyItem(scan_property).Type.ToString();

                        if (scan_property == 2)
                        {
                            //Latitude degrees minutes and seconds (rational)
                            degrees = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4);
                            minutes = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12);
                            seconds = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20);

                            lat_dd = degrees + (minutes / 60) + (seconds / 3600); //->Latitude
                        }
                        else if (scan_property == 4)
                        {
                            //Longitude degrees minutes and seconds (rational)
                            degrees = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4);
                            minutes = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12);
                            seconds = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20);
                            long_dd = degrees + (minutes / 60) + (seconds / 3600); //-->longtitude
                        }
                        else
                        {
                            //Do nothing...
                            /**
                            if (scan_property == 24)
                            {
                                //Magnetic bearing of subject to photographer (rational)
                                //do nothing
                            }
                            //scan_property ++;
                             * **/
                        }
                    }
                }
                if ((lat_dd > 0) && (long_dd > 0))
                {
                    //Reverse Geocoding
                    XmlDocument doc = new XmlDocument();
                    doc.Load("http://maps.googleapis.com/maps/api/geocode/xml?latlng=" + lat_dd + "," + long_dd + "&sensor=false");
                    XmlNode element = doc.SelectSingleNode("//GeocodeResponse/status");
                    if (element.InnerText == "ZERO_RESULTS")
                    {
                        lb_msg.Text = "No result found";
                    }
                    else
                    {
                        element = doc.SelectSingleNode("//GeocodeResponse/result/formatted_address");
                        address = element.InnerText;
                    }

                    string convert = CoordinatesConverter.CoordinatesConvertor(long_dd, lat_dd, 4326, 3414);
                    string[] converting = convert.Split(',');
                    x = Double.Parse(converting[0]);
                    y = Double.Parse(converting[1]);
                }
                else
                {
                    address = tbLocation.Text.ToString();
                    double lng = Double.Parse(tbX.Text.ToString());
                    double lat = Double.Parse(tbY.Text.ToString());
                    string convert = CoordinatesConverter.CoordinatesConvertor(lng, lat, 4326, 3414);
                    string[] converting = convert.Split(',');
                    x = Double.Parse(converting[0]);
                    y = Double.Parse(converting[1]);
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

                    //Fever
                    Bitmap feverMap = currentFrame.ToBitmap();
                    newList.Add(retrieveGreenPixel(feverMap));

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

            feverresult = newList.ElementAt(0);
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
            noDetect = 0;
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
                    neutral++;
                }
                else
                {
                    // it should not come here . 
                }
            }

            //get total sum of the 5 emotion data.
            int total = smile + sad + angry + neutral + noDetect;
            //put them into the percentage (emotion/frames * 100)
            double smilePer = (Convert.ToDouble(smile) / totalFrame) * 100;
            double sadPer = (Convert.ToDouble(sad) / totalFrame) * 100;
            double angryPer = (Convert.ToDouble(angry) / totalFrame) * 100;
            double neturalPer = (Convert.ToDouble(neutral) / totalFrame) * 100;
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

            leftfist = leftFists.Count();
            rightfist = rightFists.Count();
            leftpalm = leftPalm.Count();
            rightpalm = rightPalm.Count();
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

        private void soundAnalysis()
        {
            NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(vfilepath));
            WaveFileReader wavFile = new WaveFileReader(vfilepath);
            byte[] mainBuffer = new byte[wave.Length];

            float fileSize = (float)wavFile.Length / 1048576;
            if (fileSize < 2)
                window = 8;
            else if (fileSize > 2 && fileSize < 4)
                window = 16;
            else if (fileSize > 4 && fileSize < 8)
                window = 32;
            else if (fileSize > 8 && fileSize < 12)
                window = 128;
            else if (fileSize > 12 && fileSize < 20)
                window = 256;
            else if (fileSize > 20 && fileSize < 30)
                window = 512;
            else
                window = 2048;

            float[] fbuffer = new float[mainBuffer.Length / window];
            wave.Read(mainBuffer, 0, mainBuffer.Length);

            for (int i = 0; i < fbuffer.Length; i++)
            {
                fbuffer[i] = (BitConverter.ToSingle(mainBuffer, i * window));
            }

            double time = wave.TotalTime.TotalSeconds;
            ZedGraphControl zedGraphControl2 = new ZedGraphControl();

            GraphPane myPane1 = zedGraphControl2.GraphPane;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            for (int i = 0; i < fbuffer.Length; i++)
            {
                list1.Add(i, fbuffer[i]);
            }
            list2.Add(0, 0);
            list2.Add(time, 0);
            if (myCurve1 != null && myCurve2 != null)
            {
                myCurve1.Clear();
                myCurve2.Clear();
            }
            GraphPane myPane2 = zedGraphControl2.GraphPane;
            myPane2.Title.Text = "Audio Sound Wave";
            myPane2.XAxis.Title.Text = "Time, Seconds";
            myPane2.YAxis.Title.Text = "Sound Wave Graph";
            myCurve1 = myPane1.AddCurve(null, list1, System.Drawing.Color.Blue, SymbolType.None);
            myCurve1.IsX2Axis = true;
            myCurve2 = myPane1.AddCurve(null, list2, System.Drawing.Color.Black, SymbolType.None);
            myPane1.XAxis.Scale.MaxAuto = true;
            myPane1.XAxis.Scale.MinAuto = true;

            //Threshold Line
            double threshHoldY = -1.2;
            double threshHoldX = 1.2;
            LineObj threshHoldLine = new LineObj(System.Drawing.Color.Red, myPane2.XAxis.Scale.Min, threshHoldY, myPane2.XAxis.Scale.Max, threshHoldY);
            LineObj threshHoldLine2 = new LineObj(System.Drawing.Color.Red, myPane2.XAxis.Scale.Min, threshHoldX, myPane2.XAxis.Scale.Max, threshHoldX);
            myPane2.GraphObjList.Add(threshHoldLine);
            myPane2.GraphObjList.Add(threshHoldLine2);

            // Add a text box with instructions
            TextObj text = new TextObj(
                "Ratio Conversion: 1:100\nRed Lines: Threshold\nZoom: left mouse & drag\nContext Menu: right mouse",
                0.05f, 0.95f, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom);
            text.FontSpec.StringAlignment = StringAlignment.Near;
            myPane2.GraphObjList.Add(text);

            // Show the x axis grid
            myPane2.XAxis.MajorGrid.IsVisible = true;
            myPane2.YAxis.MajorGrid.IsVisible = true;
            // Just manually control the X axis range so it scrolls continuously
            // instead of discrete step-sized jumps
            //myPane2.XAxis.Scale.Format = @"00:00:00";
            myPane2.XAxis.Scale.IsSkipCrossLabel = true;
            myPane2.XAxis.Scale.IsPreventLabelOverlap = true;
            myPane2.XAxis.Type = ZedGraph.AxisType.Linear;
            myPane2.XAxis.Scale.Min = 0;
            myPane2.XAxis.Scale.Max = 1.2;
            myPane2.AxisChange();

            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane2.YAxis.MajorTic.IsOpposite = false;
            myPane2.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane2.YAxis.MajorGrid.IsZeroLine = false;
            // Align the Y axis labels so they are flush to the axis
            myPane2.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane2.YAxis.Scale.Min = -1.5;
            myPane2.YAxis.Scale.Max = 1.5;

            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();

            Bitmap graph = myPane1.GetImage();
            graph = resizeImage(graph, new Size(500, 500));
            savePath = Server.MapPath("~\\image\\");
            soundpath = savePath + title + ".png";
            graph.Save(soundpath);
            soundresult = false;
        }

        private int retrieveGreenPixel(Bitmap image1)
        {
            Color clr = image1.GetPixel(50, 50); // Get the color of pixel at position 5,5
            int totalColors = clr.R + clr.B + clr.G;
            int red = clr.R;
            int blue = clr.B;
            int green = clr.G;
            return green;
        }

        private void insertIntoDatabase()
        {
            abuseper = 0;
            //Face
            if (angry > smile && angry > sad && angry > neutral && angry > noDetect)
            {
                abuseper = abuseper + 25;
            }
            //Motion
            if ((((leftfist + rightfist + leftpalm + rightpalm) / totalFrame) * 100) > 20)
            {
                abuseper = abuseper + 25;
            }
            //Sound
            if (soundresult == true)
            {
                abuseper = abuseper + 25;
            }
            //Heat
            if (feverresult >= 50)
            {
                abuseper = abuseper + 25;
            }
            CreateAnalysis cAna = new CreateAnalysis(abuseper, totalFrame, smile, angry, sad, neutral, leftfist, rightfist, leftpalm, rightpalm, soundresult, soundpath, feverresult);
            CreateLocation cLoc = new CreateLocation(address, x, y);
            CreateFile cFile = new CreateFile(title, date, path, desc, type);
            insertRecord(cAna, cLoc, cFile);
        }

        private Bitmap resizeImage(Bitmap imgToResize, Size size)
        {
            Bitmap b = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage((System.Drawing.Image)b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }
            return b;
        }
    }
}