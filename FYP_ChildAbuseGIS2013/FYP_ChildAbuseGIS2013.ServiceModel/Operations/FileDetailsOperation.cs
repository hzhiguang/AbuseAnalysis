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
    [Route("/json/file/{id}")]
    public class FileDetailsOperation
    {
        [DataMember]
        public string ID { get; set; }
    }

    [DataContract]
    public class FileDetailsResponse : IHasResponseStatus
    {
        public FileDetailsResponse()
        {
            this.File = File;
        }

        [DataMember]
        public File File { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }
}