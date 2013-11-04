<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<head>
    <title>Index Page</title>
</head>
<style>
#map
{
   height:750px;

}

div.featureStyle1
{
    height:50px;
    width:1300px;
    background-color:#062032;
    background:rgb(6, 32, 50); /* Fallback for older browsers without RGBA-support */
    background:rgba(6, 32, 50, 0.5);
    position:absolute;
    filter:alpha(opacity=60);
    margin-top:-750px;
    border:2px solid #000000;
}
#main
{
 background-color: #BFD2FA;  
}
table.menuTable
{
    margin-top : 8px;
    margin-left:10px;
}
</style>
<link rel="stylesheet" href="http://js.arcgis.com/3.7/js/esri/css/esri.css">
<link rel="stylesheet" type="text/css" href="css/onemapDesign.css">
<script type="text/JavaScript" src="http://www.onemap.sg/API/JS?accessKEY=qo/s2TnSUmfLz+32CvLC4RMVkzEFYjxqyti1KhByvEacEdMWBpCuSSQ+IFRT84QjGPBCuz/cBom8PfSm3GjEsGc8PkdEEOEr&amp;v=2.8&amp;type=full"></script>
<script type="text/JavaScript" src="http://js.arcgis.com/3.7/"></script>
<script type="text/javascript">

    dojo.require("esri.map");
    dojo.require("esri.dijit.Geocoder");
    dojo.require("esri.tasks.locator");
    dojo.require("esri.graphic");
     dojo.require("esri.InfoTemplate");
     dojo.require("esri.symbols.SimpleMarkerSymbol");
     dojo.require("esri.symbols.Font");
     dojo.require("esri.symbols.TextSymbol");
     dojo.require("dojo._base.array");
     dojo.require("dojo._base.Color");
     dojo.require("dojo.parser");
     dojo.require("dojo.dom");
     dojo.require("dijit.registry");
     dojo.require("esri.dijit.LocateButton")
  
    var covert;
    var centerPoint = "26968.103,39560.969";
    var oneMap = new GetOneMap('map', 'sm', { level: 1, center: centerPoint });
    var geocoder, geoLocate;
    function init() {        
        geocoder = new esri.dijit.Geocoder({
            map: oneMap.map,
            autoComplete: true,
            maxLocations: 10,
            arcgisGeocoder: {
                name:"Singapore" 
            }
        }, "tb_search");
        geocoder.startup();

        geocoder.on("select", showLocation);

        function showLocation(evt) {
          oneMap.map.graphics.clear();
          var point = evt.result.feature.geometry;
          var symbol = new SimpleMarkerSymbol()
            .setStyle("square")
            .setColor(new Color([255,0,0,0.5]));
          var graphic = new Graphic(point, symbol);
          map.graphics.add(graphic);

          map.infoWindow.setTitle("Search Result");
          map.infoWindow.setContent(evt.result.name);
          map.infoWindow.show(evt.result.feature.geometry);
        });
    }
    dojo.addOnLoad(init);
    
   
      function GetCords(xyCorr) {
          var inXYList = xyCorr;
          var inputSR = 4326;
          var outputSR = 3414;
          var CoordConvertorObj = new CoordConvertor();
          CoordConvertorObj.ConvertCoordinate(inXYList, inputSR, outputSR, showVals);
      }
      

      function showVals(outXY) {
          var covert = outXY;
      }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id ="main">
<!--Start of map -->
<div id="map"></div>
<!--End of map -->

<!--Start of feature1 -->
<div id="featureColTop" class="featureStyle1">
    <table class= "menuTable">
        <tr>
            <td class="tdStyle">Search :</td>
            <td class="tdStyle"> <input type="text" id="tb_search" /></td>
        </tr>
    </table>
</div>
<!--End of feature1 -->
</div>
</asp:Content>
