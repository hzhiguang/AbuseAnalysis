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
    public class Files
    {
    }

    [DataContract]
    [Route("/file", "POST")]
    public class CreateFile
    {
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public DateTime date { get; set; }
        [DataMember]
        public string path { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public int locationid { get; set; }
        [DataMember]
        public int analysisid { get; set; }
    }

    [DataContract]
    [Route("/json/file/{id}")]
    public class GetFileDetails
    {
        [DataMember]
        public string ID { get; set; }
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

    [DataContract]
    public class CreateFileResponse : IHasResponseStatus
    {
        public CreateFileResponse()
        {
            this.File = File;
        }

        [DataMember]
        public File File { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
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