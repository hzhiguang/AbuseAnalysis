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
        //Get all file data
        public FileResponse Get(Files request)
        {
            return new FileResponse { File = Db.Select<File>() };
        }

        //Insert 1 file object
        public void Post(CreateFile request)
        {
            Db.ExecuteSql("INSERT INTO file (title, date, path, description, type, locationid, analysisid) VALUES ('" + request.title + "', '" + request.date + "', '" + request.path + "', '" + request.description + "', '" + request.type + "', '" + request.locationid + "', '" + request.analysisid + "')");
        }

        //Get all file data by type: video
        public VideoFileResponse Get(GetVideoFiles request)
        {
            return new VideoFileResponse { File = Db.Query<File>("SELECT * FROM file WHERE type = 'Video'") };
        }

        //Get all file data by type: image
        public ImageFileResponse Get(GetImageFiles request)
        {
            return new ImageFileResponse { File = Db.Query<File>("SELECT * FROM file WHERE type = 'Image'") };
        }

        //Get all file data if abuse percentage >= 75
        public AbuseFileResponse Get(GetAbuseFiles request)
        {
            return new AbuseFileResponse { File = Db.Query<File>("SELECT * FROM file INNER JOIN analysis ON file.analysisid = analysis.id WHERE analysis.abuseper >= 75") };
        }

        //Get all file data if abuse percentage < 75
        public NotAbuseFileResponse Get(GetNotAbuseFiles request)
        {
            return new NotAbuseFileResponse { File = Db.Query<File>("SELECT * FROM file INNER JOIN analysis ON file.analysisid = analysis.id WHERE analysis.abuseper < 75") };
        }

        //Get all file data if location is within a certain radius
        public BufferFileResponse Get(GetBufferFiles request)
        {
            return new BufferFileResponse { File = Db.Query<File>("SELECT * FROM file INNER JOIN location ON file.locationid = location.id WHERE ST_DWithin(geom, ST_GeomFromText('POINT(" + request.lat + " " + request.lon + ")', 3414), " + request.rad + ")") };
        }

        //Get all file data by location id
        public FileDetailsByLocationResponse Get(GetFileDetailsByLocation request)
        {
            return new FileDetailsByLocationResponse { File = Db.Query<File>("SELECT * FROM file WHERE locationid = '" + request.ID + "'") };
        }

        //Get 1 file object by id
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
