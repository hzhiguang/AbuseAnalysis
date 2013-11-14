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
        //Get all location data
        public LocationResult Get(Locations request)
        {
            return new LocationResult { Location = Db.Select<Location>() };
        }

        //Insert 1 location object
        public CreateLocationResult Post(CreateLocation request)
        {
            Db.ExecuteSql("INSERT INTO location (address, x, y, geom) Values ('" + request.address + "','" + request.x + "','" + request.y + "', ST_GeomFromText('POINT(" + request.x + " " + request.y + ")', 3414))");
            long id = Db.GetLastInsertId();
            var location = Db.GetByIdOrDefault<Location>(id);
            if (location == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("Location does not exist: " + id.ToString()));
            }

            return new CreateLocationResult { Location = location };
        }

        //Get 1 location object by id
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
