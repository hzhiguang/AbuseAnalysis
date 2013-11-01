using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FYP_ChildAbuseGIS2013.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace FYP_ChildAbuseGIS2013.ServiceModel.Operations
{
    [DataContract]
    [Route("/location")]
    public class Locations
    {
    }

    [DataContract]
    public class LocationResult : IHasResponseStatus
    {
        public LocationResult()
        {
            this.Location = new List<LocationModel>();
        }

        [DataMember]
        public List<LocationModel> Location { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
