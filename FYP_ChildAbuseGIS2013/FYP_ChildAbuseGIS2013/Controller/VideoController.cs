using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_ChildAbuseGIS2013.Model;

namespace FYP_ChildAbuseGIS2013.Controller
{
    public class VideoController
    {
        public VideoController() { }

        public bool insertVideo()
        {
            bool success = false;
            MainController conn = new MainController();
            bool started = conn.startConn();
            if (started == true)
            {
                string query = "INSERT INTO ";
                success = true;
            }
            conn.closeConn();
            return success;
        }
    }
}