using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_ChildAbuseGIS2013.Model
{
    public class Location
    {
        public int ID { get; set; }
        public int videoID { get; set; }
        public string address { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }
}