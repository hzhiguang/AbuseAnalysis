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
    public class FileService : Service
    {
        public FileResponse Get(Files request)
        {
            return new FileResponse { File = Db.Select<File>() };
        }

        public void Post(CreateFile request)
        {
            Db.ExecuteSql("INSERT INTO file (title, date, path, description, type, locationid, analysisid) VALUES ('" + request.title + "', '" + request.date + "', '" + request.path + "', '" + request.description + "', '" + request.type + "', '" + request.locationid + "', '" + request.analysisid + "')");
        }

        public FileDetailsResponse Get(GetFileDetails request)
        {
            var file = Db.GetByIdOrDefault<File>(request.ID);
            if (file == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("File does not exist: " + request.ID));
            }

            return new FileDetailsResponse { File = file };
        }
    }
}
