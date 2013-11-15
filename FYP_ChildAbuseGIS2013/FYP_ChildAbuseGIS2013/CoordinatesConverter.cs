using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FYP_ChildAbuseGIS2013.com.arcgisonline.tasks;

namespace FYP_ChildAbuseGIS2013
{
    class CoordinatesConverter
    {
        public static string CoordinatesConvertor(double InputX,double InputY, int InputWKID, int OutputWKID)
        {
            Geometry_GeometryServer geometryService = new Geometry_GeometryServer();

            SpatialReference inputSpatialReference = new GeographicCoordinateSystem();

            inputSpatialReference.WKID = InputWKID;

            inputSpatialReference.WKIDSpecified = true;

            SpatialReference outputSpatialReference = new ProjectedCoordinateSystem();

            outputSpatialReference.WKID = OutputWKID;

            outputSpatialReference.WKIDSpecified = true;

            PointN pt = new PointN();
            pt.X = InputX;
            pt.Y = InputY;

            Geometry[] inGeo = new Geometry[] { pt };

            Geometry[] geo = geometryService.Project(inputSpatialReference, outputSpatialReference, false, null, null, inGeo);
            PointN nPt = (PointN)geo[0];

            return (nPt.X.ToString() + ',' + nPt.Y.ToString());
        }
    }
}
