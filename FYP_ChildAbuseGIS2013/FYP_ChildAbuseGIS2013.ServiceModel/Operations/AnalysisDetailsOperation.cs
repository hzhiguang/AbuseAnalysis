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
    [Route("/analysis/{id}")]
    public class AnalysisDetailsOperation
    {
        [DataMember]
        public string ID { get; set; }
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
