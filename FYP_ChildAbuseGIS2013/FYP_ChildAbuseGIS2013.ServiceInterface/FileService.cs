using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace FYP_ChildAbuseGIS2013.ServiceInterface
{
    public class FileService : Service
    {
        public FileResponse Get(Files request)
        {
            return new FileResponse { File = Db.Select<FileModel>() };
        }
    }
}
