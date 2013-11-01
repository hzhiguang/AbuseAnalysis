using System.Runtime.Serialization;

namespace FYP_ChildAbuseGIS2013.ServiceModel.Types
{
    [DataContract]
    public class AnalysisModel
    {
        [DataMember]
        public int ID { get; set; }

        //[DataMember]
        //public int videoID { get; set; }

        [DataMember]
        public int totalFrame { get; set; }

        [DataMember]
        public int smileFrame { get; set; }

        [DataMember]
        public int angryFrame { get; set; }

        [DataMember]
        public int sadFrame { get; set; }

        [DataMember]
        public int neutralFrame { get; set; }

        [DataMember]
        public int leftFistFrame { get; set; }

        [DataMember]
        public int rightFistFrame { get; set; }

        [DataMember]
        public int leftPalmFrame { get; set; }

        [DataMember]
        public int rightPalmFrame { get; set; }
    }
}