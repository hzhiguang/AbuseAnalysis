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
    [Route("/json/location")]
    public class Locations
    {
    }

    [DataContract]
    [Route("/json/createLocation", "POST")]
    public class CreateLocation
    {
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public double x { get; set; }
        [DataMember]
        public double y { get; set; }
        [DataMember]
        public byte[] geom { get; set; }

        public CreateLocation(string address, double x, double y)
        {
            this.address = address;
            this.x = x;
            this.y = y;
        }
    }

    [DataContract]
    [Route("/json/location/{id}")]
    public class GetLocationDetails
    {
        [DataMember]
        public string ID { get; set; }
    }

    [DataContract]
    public class LocationResult : IHasResponseStatus
    {
        public LocationResult()
        {
            this.Location = new List<Location>();
        }

        [DataMember]
        public List<Location> Location { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class CreateLocationResult : IHasResponseStatus
    {
        public CreateLocationResult()
        {
            this.Location = Location;
        }

        [DataMember]
        public Location Location { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class LocationDetailsResult : IHasResponseStatus
    {
        public LocationDetailsResult()
        {
            this.Location = Location;
        }

        [DataMember]
        public Location Location { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
