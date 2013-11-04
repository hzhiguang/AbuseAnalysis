using System.Runtime.Serialization;

namespace FYP_ChildAbuseGIS2013.ServiceModel.Types
{
    [DataContract]
    public class Analysis
    {
        [DataMember]
        public int ID { get; set; }

        //[DataMember]
        //public int fileid { get; set; }

        [DataMember]
        public int totalframe { get; set; }

        [DataMember]
        public int smileframe { get; set; }

        [DataMember]
        public int angryframe { get; set; }

        [DataMember]
        public int sadframe { get; set; }

        [DataMember]
        public int neutralframe { get; set; }

        [DataMember]
        public int leftfistframe { get; set; }

        [DataMember]
        public int rightfistframe { get; set; }

        [DataMember]
        public int leftpalmframe { get; set; }

        [DataMember]
        public int rightpalmframe { get; set; }
    }
}