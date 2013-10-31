using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Request
{
    public class Location
    {
        public int ID { get; set; }
        public int videoID { get; set; }
        public string address { get; set; }
        public double x { get; set; }
        public double y { get; set; }

        public Location() { }

        public Location(int id, string address, double x, double y)
        {
            this.ID = id;
            this.address = address;
            this.x = x;
            this.y = y;
        }

        public Location(string address, double x, double y)
        {
            this.address = address;
            this.x = x;
            this.y = y;
        }
    }
}