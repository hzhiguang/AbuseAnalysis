using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace FYP_ChildAbuseGIS2013.ServiceInterface
{
    public class AnalysisService : Service
    {
        public AnalysisResult Get(AnalysisOperation request)
        {
            return new AnalysisResult { Analysis = Db.Select<Analysis>() };
        }
    }
}