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
    public class LocationDetailsService : Service
    {
        public LocationDetailsResult Get(LocationDetails request)
        {
            var location = Db.GetByIdOrDefault<LocationModel>(request.ID);
            if (location == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Location does not exist: " + request.ID));
            }

            return new LocationDetailsResult { Location = location };
        }
    }
}
