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
    public class LocationsService : Service
    {
        public LocationResult Get(Locations request)
        {
            return new LocationResult { Location = Db.Select<Location>() };
        }

        public LocationDetailsResult Get(GetLocationDetails request)
        {
            var location = Db.GetByIdOrDefault<Location>(request.ID);
            if (location == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Location does not exist: " + request.ID));
            }

            return new LocationDetailsResult { Location = location };
        }
    }
}
