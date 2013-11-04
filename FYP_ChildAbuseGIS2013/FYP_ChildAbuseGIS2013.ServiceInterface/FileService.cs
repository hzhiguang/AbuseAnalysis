using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace FYP_ChildAbuseGIS2013.ServiceInterface
{
    public class FileService : Service
    {
        public FileResponse Get(FilesOperation request)
        {
            return new FileResponse { File = Db.Select<File>() };
        }
    }
}
