using System.Runtime.Serialization;

namespace FYP_ChildAbuseGIS2013.ServiceModel.Types
{
    [DataContract]
    public class LocationModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int videoID { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }
}
