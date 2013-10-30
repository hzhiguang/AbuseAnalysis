using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FYP_ChildAbuseGIS2013.Controller;
using FYP_ChildAbuseGIS2013.Model;

namespace FYP_ChildAbuseGIS2013
{
    public partial class Design : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LocationController locCon = new LocationController();
            Location loc = new Location("LOL", 29745.257328092124, 40049.61971548824);
            /*if (locCon.insertLocation(loc) == true)
            {
                Label1.Text = "Works";
            }
            else
            {
                Label1.Text = "Bleah";
            }*/
            Label1.Text = locCon.insertLocation(loc);
        }
    }
}