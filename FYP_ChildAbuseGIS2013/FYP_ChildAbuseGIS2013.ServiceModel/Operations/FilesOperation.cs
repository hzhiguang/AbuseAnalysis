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
    [Route("/json/file")]
    public class FilesOperation
    {
    }

    [DataContract]
    public class FileResponse : IHasResponseStatus
    {
        public FileResponse()
        {
            this.File = new List<File>();
        }

        [DataMember]
        public List<File> File { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }
}