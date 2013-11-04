﻿using System;
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
    [Route("/location/{id}")]
    public class LocationDetailsOperation
    {
        [DataMember]
        public string ID { get; set; }
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
