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
            File file = new File();
            file.title = request.title;
            file.date = request.date;
            file.path = request.path;
            file.description = request.description;
            file.type = request.type;
            file.locationid = request.locationid;
            file.analysisid = request.analysisid;
            Db.Insert(file);
            //Db.Insert(new File { title = request.title, date = request.date, path = request.path, description = request.description, type = request.type, locationid = request.locationid, analysisid = request.analysisid });
            //return new CreateFileResponse { File = file };
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
