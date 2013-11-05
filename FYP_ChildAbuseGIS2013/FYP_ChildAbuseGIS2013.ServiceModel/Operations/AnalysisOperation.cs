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
    public class AnalysisOperation
    {
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
}
