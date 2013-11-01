using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using FYP_ChildAbuseGIS2013.ServiceModel.Operations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace FYP_ChildAbuseGIS2013.ServiceInterface
{
    public class LocationsService : Service
    {
        public LocationResult Get(Locations request)
        {
            return new LocationResult { Location = Db.Select<LocationModel>() };
        }
    }
}
