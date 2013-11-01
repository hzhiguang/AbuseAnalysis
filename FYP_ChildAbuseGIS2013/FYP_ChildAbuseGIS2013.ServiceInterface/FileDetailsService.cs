using System;
using System.Net;
using System.Collections.Generic;
using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.Common.Web;

namespace FYP_ChildAbuseGIS2013.ServiceInterface
{
    public class FileDetailsService : Service
    {
        public FileDetailsResponse Get(FileDetails request)
        {
            var file = Db.GetByIdOrDefault<FileModel>(request.ID);
            if (file == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("File does not exist: " + request.ID));
            }

            return new FileDetailsResponse { File = file};
        }
    }
}
