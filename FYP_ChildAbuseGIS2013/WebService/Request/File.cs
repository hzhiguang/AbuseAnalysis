using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;

namespace WebService.Request
{
    [Route("/file")]
    [Route("/file/{id}")]
    public class File
    {
        public int ID { get; set; }
        public int locationID { get; set; }
        public int analysisID { get; set; }
        public string title { get; set; }
        public DateTime dateTimeTaken { get; set; }
        public string path { get; set; }
        public string desc { get; set; }
        public string fileType { get; set; }

        public File() { }

        public File(int ID, string title, DateTime date, string path, string desc, string type)
        {
            this.ID = ID;
            this.title = title;
            this.dateTimeTaken = date;
            this.path = path;
            this.desc = desc;
            this.fileType = type;
        }

        public File(string title, DateTime date, string path, string desc, string type)
        {
            this.title = title;
            this.dateTimeTaken = date;
            this.path = path;
            this.desc = desc;
            this.fileType = type;
        }
    }
}