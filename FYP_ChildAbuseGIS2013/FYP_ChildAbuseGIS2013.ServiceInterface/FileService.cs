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

        public VideoFileResponse Get(GetVideoFiles request)
        {
            return new VideoFileResponse { File = Db.Query<File>("SELECT * FROM file WHERE type = 'Video'") };
        }

        public ImageFileResponse Get(GetImageFiles request)
        {
            return new ImageFileResponse { File = Db.Query<File>("SELECT * FROM file WHERE type = 'Image'") };
        }

        public AbuseFileResponse Get(GetAbuseFiles request)
        {
            return new AbuseFileResponse { File = Db.Query<File>("SELECT * FROM file INNER JOIN analysis ON file.analysisid = analysis.id WHERE analysis.abuseper >= 75") };
        }

        public NotAbuseFileResponse Get(GetNotAbuseFiles request)
        {
            return new NotAbuseFileResponse { File = Db.Query<File>("SELECT * FROM file INNER JOIN analysis ON file.analysisid = analysis.id WHERE analysis.abuseper < 75") };
        }

        public BufferFileResponse Get(GetBufferFiles request)
        {
            return new BufferFileResponse { File = Db.Query<File>("SELECT * FROM file INNER JOIN location ON file.locationid = location.id WHERE ST_DWithin(geom, ST_GeomFromText('POINT(" + request.lat + " " + request.lon + ")', 3414), " + request.rad + ")") };
        }

        public FileDetailsByLocationResponse Get(GetFileDetailsByLocation request)
        {
            return new FileDetailsByLocationResponse { File = Db.Query<File>("SELECT * FROM file WHERE locationid = '" + request.ID + "'") };
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
