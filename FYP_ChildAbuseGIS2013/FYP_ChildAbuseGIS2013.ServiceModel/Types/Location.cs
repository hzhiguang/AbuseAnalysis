using System.Runtime.Serialization;

namespace FYP_ChildAbuseGIS2013.ServiceModel.Types
{
    [DataContract]
    public class Location
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }

        [DataMember]
        public byte[] geom { get; set; }
    }
}
