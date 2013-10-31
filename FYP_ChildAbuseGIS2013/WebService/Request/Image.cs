using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Request
{
    public class Image
    {
        public int ID { get; set; }
        public string path { get; set; }
        public string type { get; set; }

        public Image() { }

        public Image(int ID, string path, string type)
        {
            this.ID = ID;
            this.path = path;
            this.type = type;
        }

        public Image(string path, string type)
        {
            this.path = path;
            this.type = type;
        }
    }
}