using System.Runtime.Serialization;
using System;

namespace FYP_ChildAbuseGIS2013.ServiceModel.Types
{
    [DataContract]
    public class FileModel
    {
        [DataMember]
        public int ID { get; set; }

        //[DataMember]
        //public int locationID { get; set; }

        //[DataMember]
        //public int analysisID { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public DateTime date { get; set; }

        [DataMember]
        public string path { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string type { get; set; }
    }
}
