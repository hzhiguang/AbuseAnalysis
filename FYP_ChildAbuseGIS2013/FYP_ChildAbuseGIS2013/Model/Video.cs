using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_ChildAbuseGIS2013.Model
{
    public class Video
    {
        public int ID { get; set; }
        public int locationID { get; set; }
        public int evidenceID { get; set; }
        public int analysisID { get; set; }
        public string name { get; set; }
        public DateTime dateOfUpload { get; set; }
        public string path { get; set; }
    }
}