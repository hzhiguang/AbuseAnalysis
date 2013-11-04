using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace FYP_ChildAbuseGIS2013
{
    public partial class Result : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SET UP THE DATA TO PLOT  
            double[] yVal = { 80, 20 };
            string[] xName = { "Pass", "Fail" };
            chart1.Series.Add(new Series());
            chart1.Series[0].Points.DataBindXY(xName, yVal);

            //SET THE CHART TYPE TO BE PIE
            chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Pie;
            chart1.Series[0]["PieLabelStyle"] = "Outside";
            chart1.Series[0]["PieStartAngle"] = "-90";

            //SET THE COLOR PALETTE FOR THE CHART TO BE A PRESET OF NONE 
            //DEFINE OUR OWN COLOR PALETTE FOR THE CHART 
            chart1.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.None;
            chart1.PaletteCustomColors = new Color[] { Color.Blue, Color.Red };

            //SET THE IMAGE OUTPUT TYPE TO BE JPEG
            chart1.ImageType = System.Web.UI.DataVisualization.Charting.ChartImageType.Jpeg;

            //ADD A PLACE HOLDER CHART AREA TO THE CHART
            //SET THE CHART AREA TO BE 3D
            chart1.ChartAreas.Add(new ChartArea());
            chart1.ChartAreas[0].Area3DStyle.Enable3D = true;

            //ADD A PLACE HOLDER LEGEND TO THE CHART
            //DISABLE THE LEGEND
            chart1.Legends.Add(new Legend());
            chart1.Legends[0].Enabled = false;

            Session["result"] = true;
            //Response.Redirect("share.aspx");
        }
    }
}