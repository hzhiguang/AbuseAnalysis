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
    [Route("/analysis")]
    public class Analysis
    {
    }

    [DataContract]
    public class AnalysisResult : IHasResponseStatus
    {
        public AnalysisResult()
        {
            this.Analysis = new List<AnalysisModel>();
        }

        [DataMember]
        public List<AnalysisModel> Analysis { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
