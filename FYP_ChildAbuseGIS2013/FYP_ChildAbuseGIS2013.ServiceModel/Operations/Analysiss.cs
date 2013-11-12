using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace FYP_ChildAbuseGIS2013.ServiceModel.Operations
{
    [DataContract]
    [Route("/json/analysis")]
    public class Analysiss
    {
    }

    [DataContract]
    [Route("/json/createAnalysis")]
    public class CreateAnalysis
    {
        [DataMember]
        public int abuseper { get; set; }
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

        public CreateAnalysis(int abuseper, int total, int smile, int angry, int sad, int neut, int leftFist, int rightFist, int leftPalm, int rightPalm)
        {
            this.abuseper = abuseper;
            this.totalframe = total;
            this.smileframe = smile;
            this.angryframe = angry;
            this.sadframe = sad;
            this.neutralframe = neut;
            this.leftfistframe = leftFist;
            this.rightfistframe = rightFist;
            this.leftpalmframe = leftPalm;
            this.rightpalmframe = rightPalm;
        }
    }

    [DataContract]
    [Route("/json/analysis/{id}")]
    public class GetAnalysisDetails
    {
        [DataMember]
        public string ID { get; set; }
    }

    [DataContract]
    public class AnalysisResult : IHasResponseStatus
    {
        public AnalysisResult()
        {
            this.Analysis = new List<Analysis>();
        }

        [DataMember]
        public List<Analysis> Analysis { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class CreateAnalysisResult : IHasResponseStatus
    {
        public CreateAnalysisResult()
        {
            this.Analysis = Analysis;
        }

        [DataMember]
        public Analysis Analysis { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class AnalysisDetailsResult : IHasResponseStatus
    {
        public AnalysisDetailsResult()
        {
            this.Analysis = Analysis;
        }

        [DataMember]
        public Analysis Analysis { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }
}