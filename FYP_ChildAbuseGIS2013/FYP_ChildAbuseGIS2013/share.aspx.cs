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
using FYP_ChildAbuseGIS2013.ServiceInterface;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;

namespace FYP_ChildAbuseGIS2013
{
    public partial class share : System.Web.UI.Page
    {
        //Emgu CV
        private Capture grabber;

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
        }

        private void grabVideo(string path)
        {
            grabber = new Capture(path);
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
                    //FaceAnalysis();
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