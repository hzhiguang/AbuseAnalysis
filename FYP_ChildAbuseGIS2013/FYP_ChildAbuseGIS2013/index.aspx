<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.index"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
    <title>Index Page</title>
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8"  >
      <!--<meta http-equiv="X-UA-Compatible" content="IE=edge">
      <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no">-->
      <link rel="stylesheet" href="http://js.arcgis.com/3.7/js/esri/css/esri.css">
</head>
<style>
#map
{
   height:750px;
   width:1050px;
}

div.featureStyle1
{
    height:50px;
    width:1050px;
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
    margin-top : 3px;
    margin-left:10px;
}
.locate
{
    background-image:url(image/search.png);
    width:32px;
    height:32px;
    border-width: 0;
    background-color: transparent;
     -webkit-border-top-right-radius: 5px;
	-webkit-border-bottom-right-radius: 5px;
	-webkit-border-top-left-radius: 5px;
	-webkit-border-bottom-left-radius: 5px;
	
	-moz-border-radius-topright: 5px;
	-moz-border-radius-bottomright: 5px;
	-moz-border-radius-topleft: 5px;
	-moz-border-radius-bottomleft: 5px;
	
	border-top-right-radius: 5px;
	border-bottom-right-radius: 5px;
	border-top-left-radius: 5px;
	border-bottom-left-radius: 5px;
    
    
	
}
div.subMenu
{
    margin-left:350px;
}
.map_clear_btn{
    
    background-color: white;
    border :2px solid #929292;
    -webkit-border-top-right-radius: 15px;
	-webkit-border-bottom-right-radius: 15px;
	-webkit-border-top-left-radius: 15px;
	-webkit-border-bottom-left-radius: 15px;
	
	-moz-border-radius-topright: 15px;
	-moz-border-radius-bottomright: 15px;
	-moz-border-radius-topleft: 15px;
	-moz-border-radius-bottomleft: 15px;
	
	border-top-right-radius: 15px;
	border-bottom-right-radius: 15px;
	border-top-left-radius: 15px;
	border-bottom-left-radius: 15px;
    
  
}
#legend
{
    width:250px;
    height:747px;
    border:2px solid Black;
    background-color:#E1EFFC;
     -webkit-border-top-right-radius: 3px;
	-webkit-border-bottom-right-radius: 3px;
	-webkit-border-top-left-radius: 3px;
	-webkit-border-bottom-left-radius: 3px;
	
	-moz-border-radius-topright: 3px;
	-moz-border-radius-bottomright: 3px;
	-moz-border-radius-topleft: 3px;
	-moz-border-radius-bottomleft: 3px;
	
	border-top-right-radius: 3px;
	border-bottom-right-radius: 3px;
	border-top-left-radius: 3px;
	border-bottom-left-radius: 3px;
	
	
	
   /** background:rgb(255, 255, 255); --> Fallback for older browsers without RGBA-support 
    background:rgba(255, 255, 255, 0.5);**/
    margin-left:1048px;
    position:absolute;
    filter:alpha(opacity=60);
    margin-top:-750px;

	
}
div.legendInfo
{
    width:220px;
    height:690px;
    background-color:White;
    margin-left:10px;
    border : 1px solid #97C8F4;
}
#legendTable
{
    width:220px;
}
  /* set title font properties */
      .infowindow .window .top .right .user .titlebar .title { font-family:Arial, Helvetica, sans-serif; font-weight:bold; font-size:14pt; }
      /* set content font properties */
      .infowindow .window .top .right .user .content { font-style:italic; font-size:10pt; }
</style>
<!-- the basic of using arcgis map -->
<link rel="stylesheet" href="   http://js.arcgis.com/3.7/js/dojo/dijit/themes/tundra/tundra.css">




<!-- 
    There are 4 theme in arcgis API 
    <link rel="stylesheet" href=" http://js.arcgis.com/3.7/js/dojo/dijit/themes/claro/claro.css">
<link rel="stylesheet" href=" http://js.arcgis.com/3.7/js/dojo/dijit/themes/nihilo/nihilo.css">
.<link rel="stylesheet" href="http://js.arcgis.com/3.7/js/dojo/dijit/themes/soria/soria.css">
-->

<!----><link rel="stylesheet" type="text/css" href="css/onemapDesign.css">
<script type="text/JavaScript" src="http://www.onemap.sg/API/JS?accessKEY=xkg8VRu6Ol+gMH+SUamkRIEB7fKzhwMvfMo/2U8UJcFhdvR4yN1GutmUIA3A6r3LDhot215OVVkZvNRzjl28TNUZgYFSswOi&amp;v=2.8&amp;type=full"></script>
<script>
var dojoConfig = {
        parseOnLoad:true,
        packages: [{
          "name": "myModules",
          "location": location.pathname.replace(/\/[^/]+$/, "") + "/js"
        }]
      };
    </script>
<script type="text/JavaScript" src="http://js.arcgis.com/3.7/"></script>  
<script type="text/javascript">
    //dojo.require("esri/map");
    //dojo.require("esri/dijit/Geocoder");
    //dojo.require("esri/tasks/locator");
    //dojo.require("esri/symbol/SimpleMarkerSymbol");
    //dojo.require("esri/InfoTemplate");
    //dojo.require("esri/Graphic");
    //dojo.require("esri/symbol/TextSymbol");
    //dojo.require("dojo/_base/array")


    require([
        "esri/map", "esri/dijit/Geocoder", "esri/tasks/locator", "esri/toolbars/draw", "esri/tasks/geometry", "esri/graphic", "myModules/InfoWindow", "dojo/dom",
        "dojo/dom-construct", "esri/InfoTemplate", "esri/symbols/SimpleMarkerSymbol", "esri/geometry/Point", "esri/symbols/Font", "esri/symbols/TextSymbol", "dojo/_base/array", "dojo/_base/Color",
        "dojo/number", "dojo/parser", "dojo/dom", "dijit/registry", "esri/layers/FeatureLayer", "esri/request", "esri/geometry/Extent", "esri/symbols/SimpleFillSymbol", "esri/symbols/PictureMarkerSymbol",
        "esri/renderers/ClassBreaksRenderer", "esri/layers/GraphicsLayer", "esri/SpatialReference", "esri/dijit/PopupTemplate", "esri/geometry/Point", "esri/geometry/webMercatorUtils",
        "dijit/form/Button", "dijit/form/Textarea", "dijit/layout/BorderContainer", "dijit/layout/ContentPane", "dojo/domReady!"
      ], function (
        Map, Geocoder, Locator, Draw, geometry, Graphic, InfoWindow, dom,
        domConstruct,
        InfoTemplate, SimpleMarkerSymbol, Point,
        Font, TextSymbol,
        arrayUtils, Color,
        number, parser, dom, registry, FeatureLayer, esriRequest, Extent, SimpleFillSymbol, PictureMarkerSymbol, ClassBreaksRenderer,
        GraphicsLayer, SpatialReference, PopupTemplate, Point, webMercatorUtils
      //"extras/ClusterLayer",ClusterLayer
      ) {
          parser.parse();


          var map, geocoder, locator, marker, gsvc,toolbar;
          var clusterLayer;
          var popupOptions;

          var centerPoint = "26968.103,39560.969";
          var oneMap = new GetOneMap('map', 'sm', {
              level: 1,
              center: centerPoint
          });


          function init() {
              map = oneMap.map;
              map.infoWindow.resize(250, 100);
              gsvc = new esri.tasks.GeometryService("http://tasks.arcgisonline.com/ArcGIS/rest/services/Geometry/GeometryServer");

              //registry.byId("locate").on("click", locate);
              dojo.connect(search, "onclick", locate);
              dojo.connect(map_clear, "onclick", clear);
             // dojo.connect(tb, "onDrawEnd", addPolygon);
              //registry.byId("map_clear").on("click", clear);
              autocomplete();
              addMarker();
              addChildCare();
          }
          dojo.addOnLoad(init);

          function autocomplete() {
              geocoder = new Geocoder({
                  map: map,
                  autoNavigate: false,
                  autoComplete: true,
                  maxLocations: 5,
                  showResults: true,
                  arcgisGeocoder: {
                      name: " Singapore",
                      sourceCountry: "SGP",
                      suffix: " Singapore",
                      placeholder: "Buffering the Area"

                  }
              }, document.getElementById("tb_search"));
              geocoder.startup();
          }

          // listen for button click then geocode

          //dojo.connect("tb_value", "onLoad", addMarkers);

          locator = new Locator("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer");
          locator.on("address-to-locations-complete", showResults);
          function locate() {
              alert(geocoder.value);
              //map.graphics.clear();
              var address = {
                  "SingleLine": geocoder.value
              };
              locator.outSpatialReference = map.spatialReference;
              var options = {
                  address: address,
                  outFields: ["Loc_name"]
              }
              locator.addressToLocations(options);

          }

          function showResults(evt) {
              var candidate;
              //var symbol = new SimpleMarkerSymbol();
              var infoTemplate = new InfoTemplate(
            "Location",
            "Address: ${address}<br />Score: ${score}<br />Source locator: ${locatorName}"
          );
              //symbol.setStyle(SimpleMarkerSymbol.STYLE_SQUARE);
              //symbol.setColor(new Color([153, 0, 51, 0.75]));

              var geom;
              arrayUtils.every(evt.addresses, function (candidate) {
                  console.log(candidate.score);
                  if (candidate.score > 80) {
                      console.log(candidate.location);
                      //var attributes = {
                      // address: candidate.address,
                      // score: candidate.score,
                      // locatorName: candidate.attributes.Loc_name
                      //};
                      geom = candidate.location;
                      console.log(geom);
                      doBuffer(geom);
                  }
              });
              //if (geom !== undefined) {
              map.centerAndZoom(geom, 4);
              //}
          }

          function addMarker() {
              var spatialReference = new SpatialReference({ wkid: 3414 });
              var geom = new Point(29830.4695669479, 40135.9793048648, spatialReference);
              var iconPath = "M16,3.5c-4.142,0-7.5,3.358-7.5,7.5c0,4.143,7.5,18.121,7.5,18.121S23.5,15.143,23.5,11C23.5,6.858,20.143,3.5,16,3.5z M16,14.584c-1.979,0-3.584-1.604-3.584-3.584S14.021,7.416,16,7.416S19.584,9.021,19.584,11S17.979,14.584,16,14.584z";
              var pictureSymbol = new PictureMarkerSymbol();
              pictureSymbol.setUrl("../image/locationMarker.png");
              var infoTemplate = new InfoTemplate(
            "Location",
            "Title:<br/>Description:<br/>Date:<br/> location:"
          );
              //symbol.setPath(iconPath);
              //symbol.setColor(new Color([35, 63, 156]));
              //var symbol = new SimpleMarkerSymbol();
              //var attributes = "<tr><td>Test:</td><td>Hello</td></tr>"
              var graphic = new Graphic(geom, pictureSymbol, null, infoTemplate);
              //add a graphic to the map at the geocoded location
              map.graphics.add(graphic);
          }
          function addChildCare() {
              // alert("ICE CREAM");

              var attraction = new Array();
              $(document).ready(function () {
                  $.ajax({
                      type: "OPTIONS",
                      url: "http://www.onemap.sg/API/services.svc/themeSearch?token=xkg8VRu6Ol+gMH+SUamkRIEB7fKzhwMvfMo/2U8UJcFhdvR4yN1GutmUIA3A6r3LDhot215OVVkZvNRzjl28TNUZgYFSswOi&searchVal=child care&otptFlds=SEARCHVAL,CATEGORY,THEME,AGENCY&returnGeom=1&rset=1",
                      data: "{}",
                      //contentType: "application/json; charset=utf-8",
                      crossDomain: true,
                      dataType: "json",
                      //cache: false,
                      success: function (msg) {
                          for (var i = 0; i < msg.d.length; i++) {
                              attraction[i] = [msg.d[i].SEARCHVAL, msg.d[i].CATEGORY, msg.d[i].THEME, msg.d[i].X, msg.d[i].Y];
                              alert(msg.d[i].SEARCHVAL);
                          }

                      },
                      error: function (msg) {
                          //alert(JSON.stringify(msg)+ " error")
                      }
                  });
              });
          }
          function doBuffer(location) {

              //map.graphics.clear();
              var params = new esri.tasks.BufferParameters();
              console.log(location);
              params.geometries = [location];

              //buffer in linear units such as meters, km, miles etc.
              params.distances = [0.1, 1];
              params.unit = esri.tasks.GeometryService.UNIT_KILOMETER;
              params.outSpatialReference = map.SpatialReference;

              gsvc.buffer(params, showBuffer);
          }

          function showBuffer(geometries) {
              var symbol = new esri.symbol.SimpleFillSymbol(
        esri.symbol.SimpleFillSymbol.STYLE_SOLID,
        new esri.symbol.SimpleLineSymbol(
          esri.symbol.SimpleLineSymbol.STYLE_SOLID,
          new dojo.Color([255, 0, 0, 0.65]), 2
        ),
        new dojo.Color([255, 0, 0, 0.35])
      );

              dojo.forEach(geometries, function (geometry) {
                  var infoTemplate = new InfoTemplate(
                "Area",
                "Surronding"
            );
                  var graphic = new esri.Graphic(geometry, symbol, null, infoTemplate);
                  map.graphics.add(graphic);
              });
          }

          function createToolbar(themap) {
              toolbar = new Draw(map);
              toolbar.on("draw-end", addToMap);
          }

          function addPolygon(evt) {
              var symbol;
              toolbar.deactivate();
              map.showZoomSlider();
              switch (evt.geometry.type) {
                  case "point":
                  case "multipoint":
                      symbol = new SimpleMarkerSymbol();
                      break;
                  case "polyline":
                      symbol = new SimpleLineSymbol();
                      break;
                  default:
                      symbol = new SimpleFillSymbol();
                      break;
              }
              var graphic = new Graphic(evt.geometry, symbol);
              map.graphics.add(graphic);
          }
          function clear() {
              alert("clear");
              map.graphics.clear();
              addMarker();
          }
          //end of require
      });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id ="main" class="tundra">
<!--Start of map -->
<div id="map" data-dojo-type="dijit/layout/ContentPane" 
           data-dojo-props="region:'center'"></div>
<!--End of map -->

<!--Start of feature1 -->
<div id="featureColTop" class="featureStyle1">
    <table class= "menuTable">
        <tr>
            <td><textarea rows=1 cols=1 id="tb_search"   ></textarea></td>
            <td><img src="image/search.png" id="search" class="locate_btn" onmouseover=" this.src = '../image/search_hover.png'" onmouseout="this.src = '../image/search.png'"  alt="Search" /></td>
            <td><div class="subMenu">
                <table id ="subMenuTable">
                    <tr>
                        <td><div class="btn_design"><img src="image/draw.png" id="Img1" onmouseover=" this.src = '../image/draw_hover.png'" onmouseout="this.src = '../image/draw.png'"  class="map_clear_btn" alt="draw"/></div></td>
                        <td><div class="btn_design" onclick="tb.activate(esri.toolbars.Draw.POLYGON);" id="tb"><img src="image/map_clear.png" id="map_clear" onmouseover=" this.src = '../image/map_clear_hover.png'" onmouseout="this.src = '../image/map_clear.png'"  class="map_clear_btn" alt="clear"/></div></td>
                    
                    </tr>
                </table></div></td>
           
            <td>
                <div>
                    
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="legend" class="legendClass">
    <div style="margin-top:25px; margin-left:5px;">Legend</div>
        <div class="legendInfo">
            <table id="legendTable">
                <tr>
                    <td></td>
                    <td><b>Sybmol</b></td>
                    <td><b>Represent</b></td>
                </tr>
                <tr>
                    <td></td>
                    <td><img src="image/locationMarker.png"/ alt ="child_abuse_case"></td>
                    <td>Child Abuse Case</td>
                </tr>
                <tr>
                    <td></td>
                    <td><img src="image/locationMarker2.png"/ alt ="child_abuse_case"></td>
                    <td>Report Case</td>
                </tr>
            </table>
            
        </div>
</div>

<!--End of feature1 -->


</div>
</asp:Content>
