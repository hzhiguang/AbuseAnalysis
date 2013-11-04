using System.Runtime.Serialization;

namespace FYP_ChildAbuseGIS2013.ServiceModel.Types
{
    [DataContract]
    public class Image
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string path { get; set; }

        [DataMember]
        public string type { get; set; }
    }
}
