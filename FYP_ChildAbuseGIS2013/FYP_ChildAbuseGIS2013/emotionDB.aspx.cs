using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP_ChildAbuseGIS2013
{
    public partial class _Default : System.Web.UI.Page
    {
        EmotionExpressionDB.Service1SoapClient dbConn = new EmotionExpressionDB.Service1SoapClient();
        byte[] imageSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            dbView.DataSource = dbConn.getAllEmotionList();
            
            dbView.DataBind();
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {

            if (imageDir.PostedFile != null && imageDir.PostedFile.FileName != "")
            {
               imageSize = new byte[imageDir.PostedFile.ContentLength];
               HttpPostedFile uploadedImage = imageDir.PostedFile;
               uploadedImage.InputStream.Read(imageSize, 0, (int)imageDir.PostedFile.ContentLength);
                
            }

            String imageName = imageID.Text;
            String imageEmotion = imageExpression.Text;

            EmotionExpressionDB.ImageEmotion insert = new EmotionExpressionDB.ImageEmotion();
            insert.ID = Convert.ToInt32(imageName);
            insert.ImageSrc = imageSize;
            insert.Expression = imageEmotion;

            if (dbConn.insertEmotion(insert) == true)
            {
                lbMsg.Text = "Insert Successfully";
            }
            
            
        }
    }
}