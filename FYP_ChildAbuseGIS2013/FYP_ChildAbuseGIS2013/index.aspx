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
#HomeButton {
      /**position: absolute;
      margin-top: 150px;
      left: 20px;
      z-index: 99;
      **/
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
        "dijit/form/Button", "dijit/form/Textarea", "dijit/layout/BorderContainer", "dijit/layout/ContentPane", "dojo/domReady!", "esri/dijit/HomeButton", "esri/dijit/Scalebar"
      ], function (
        Map, Geocoder, Locator, Draw, geometry, Graphic, InfoWindow, dom,
        domConstruct,
        InfoTemplate, SimpleMarkerSymbol, Point,
        Font, TextSymbol,
        arrayUtils, Color,
        number, parser, dom, registry, FeatureLayer, esriRequest, Extent, SimpleFillSymbol, PictureMarkerSymbol, ClassBreaksRenderer,
        GraphicsLayer, SpatialReference, PopupTemplate, Point, webMercatorUtils, HomeButton, Scalebar
      //"extras/ClusterLayer",ClusterLayer
      ) {
          parser.parse();


          var map, geocoder, locator, marker, gsvc, toolbar, home, location, locationAddress;
          var markerList = [];
          var childcareMarkerList = [];
          var x = "";
          var y = "";
          var raduis = "";

          var fileData = new Array();

          var locationData = new Array();
          var analysisData = new Array();
          var bufferResult = new Array();
          var clusterLayer;
          var popupOptions;

          var centerPoint = "26968.103,39560.969";
          var oneMap = new GetOneMap('map', 'sm', {
              level: 1,
              center: centerPoint
          });


          function init() {
              map = oneMap.map;
              map.infoWindow.resize(250, 150);
              gsvc = new esri.tasks.GeometryService("http://tasks.arcgisonline.com/ArcGIS/rest/services/Geometry/GeometryServer");

              addFileData();
              showme();
              addChildCare();
              policeStationMarker();
              autocomplete();
              createHomeButton();
              createScaleBar();
              dojo.connect(show, "onclick", showme);
              dojo.connect(draw, "onclick", activateTool);
              dojo.connect(map_clear, "onclick", clear);
              dojo.connect(search, "onclick", locate);
              createToolbar();
              //dojo.connect(draw, "onclick", activateTool());
          }

          dojo.addOnLoad(init);



          function createHomeButton() {
              home = new HomeButton({
                  map: map
              }, "HomeButton");
              home.startup();
          }

          function createScaleBar() {
              var scalebar = new Scalebar({
                  map: map,
                  // "dual" displays both miles and kilmometers
                  // "english" is the default, which displays miles
                  // use "metric" for kilometers
                  scalebarUnit: "dual",
                  attachTo: "GetOneMap bottom-right"
              });
          }

          function activateTool() {
              //alert("activate");
              //var tool = this.label.toUpperCase().replace(/ /g, "_");
              toolbar.activate(esri.toolbars.Draw.CIRCLE);
              map.hideZoomSlider();
          }

          function createToolbar(themap) {
              //console.log("initalise toolbar");
              toolbar = new Draw(map);
              toolbar.on("draw-end", addToMap);
          }

          function addToMap(evt) {
              //alert("hello");
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
              console.log(evt.geometry.toJson());
              console.log(evt.geometry.getPoint(0, 4));
              var graphic = new Graphic(evt.geometry, symbol);
              map.graphics.add(graphic);
          }



          //autocomplete function to search and locate address
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
          //showing result of location 
          function showResults(evt) {
              var candidate;
              var geom;
              arrayUtils.every(evt.addresses, function (candidate) {
                  console.log(candidate.score);
                  if (candidate.score > 80) {
                      console.log(candidate.location);
                      x = candidate.location.x;
                      y = candidate.location.y;
                      geom = candidate.location;
                      doBuffer(geom);
                  }
              });
              //if (geom !== undefined) {
              map.centerAndZoom(geom, 4);
              //}
          }
          //function design buffer base on location
          function doBuffer(location) {
              //map.graphics.clear();
              var params = new esri.tasks.BufferParameters();
              params.geometries = [location];

              //buffer in linear units such as meters, km, miles etc.
              params.distances = [0.01, 1];
              raduis = 1000;

              //1000cm = 1km
              params.unit = esri.tasks.GeometryService.UNIT_KILOMETER;
              params.outSpatialReference = map.SpatialReference;
              gsvc.buffer(params, showBuffer);
          }

          //function displaying buffer on the map
          function showBuffer(geometries) {
              bufferPara(raduis, x, y);
              var symbol = new esri.symbol.SimpleFillSymbol(
                esri.symbol.SimpleFillSymbol.STYLE_SOLID,
                new esri.symbol.SimpleLineSymbol(
                  esri.symbol.SimpleLineSymbol.STYLE_SOLID,
                  new dojo.Color([255, 0, 0, 0.65]), 2
                ),
                new dojo.Color([255, 0, 0, 0.35])
              );


              dojo.forEach(geometries, function (geometry) {
                  console.log("print");
                  var statement = "";


                  for (var i = 0; i < bufferResult.length; i++) {
                      statement = "There are a total of " + bufferResult.length + " reported child abuse case within 1km of this area <br/><br/>"
                      statement += "Case " + (i+1) + " :" + bufferResult[i][1] + "<br/><br/>Description :" + bufferResult[i][4] + "<br/><br/>Date:" + bufferResult[i][2] + "<br/><br/>";
                  }

                  var infoTemplate = new InfoTemplate("Buffer Result", statement);

                  var graphic = new esri.Graphic(geometry, symbol, null, infoTemplate);
                  map.graphics.add(graphic);
              });
          }

          function bufferPara(raduis, lat, long) {
              $.ajax({
                  type: "GET",
                  async: false,
                  url: "/api/json/bufferfile/" + raduis + "/" + lat + "/" + long,
                  data: "{}",
                  contentType: "application/json; charset=utf-8",
                  //crossDomain: true,
                  dataType: "json",
                  //cache: false,
                  success: function (file) {
                      console.log("got it");
                      alert("success");
                      bufferResult = [];
                      for (var i = 0; i < file.File.length; i++) {
                          bufferResult[i] = [file.File[i].ID, file.File[i].title, file.File[i].date, file.File[i].path, file.File[i].description, file.File[i].type, file.File[i].locationid, file.File[i].analysisid];
                      }
                  },
                  error: function (file) {
                      alert("error");
                  }
              });
          }

          //add marker of child abuse case
          function addFileData() {
              $(document).ready(function () {

                  $.ajax({
                      type: "GET",
                      async: false,
                      url: "/api/json/file",
                      data: "{}",
                      contentType: "application/json; charset=utf-8",
                      //crossDomain: true,
                      dataType: "json",
                      //cache: false,
                      success: function (file) {
                          console.log(file.File.length);
                          for (var i = 0; i < file.File.length; i++) {
                              fileData[i] = [file.File[i].ID, file.File[i].title, file.File[i].date, file.File[i].path, file.File[i].description, file.File[i].type, file.File[i].locationid, file.File[i].analysisid];
                          }
                      },
                      error: function (file) {
                          alert("error");
                      }
                  });
              });
          }
          function addLocationData(i) {
              $(document).ready(function () {
                  $.ajax({
                      type: "GET",
                      async: false,
                      url: "/api/json/location/" + fileData[i][6],
                      data: "{}",
                      contentType: "application/json; charset=utf-8",
                      //crossDomain: true,
                      dataType: "json",
                      //cache: false,
                      success: function (loc) {
                          //alert("sucesss");
                          locationData[i] = [loc.Location.ID, loc.Location.address, loc.Location.x, loc.Location.y];
                          console.log(locationData[i][0]);
                      },
                      error: function (loc) {
                          alert("error");
                      }
                  });
              });
          }

          function addAnalysisData(i) {
              $(document).ready(function () {
                  $.ajax({
                      type: "GET",
                      async: false,
                      url: "/api/json/analysis/" + fileData[i][7],
                      data: "{}",
                      contentType: "application/json; charset=utf-8",
                      //crossDomain: true,
                      dataType: "json",
                      //cache: false,
                      success: function (aly) {
                          //alert("sucesss");
                          analysisData[i] = [aly.Analysis.ID, aly.Analysis.abuseper];
                          console.log(analysisData[i][0]);

                      },
                      error: function (aly) {
                          alert("error");
                      }
                  });
              });
          }
          function showme() {

              for (var i = 0; i < fileData.length; i++) {
                  addLocationData(i);
                  addAnalysisData(i);
                  addCaseMarker(fileData[i][0], locationData[i][0], analysisData[i][0], fileData[i][1], fileData[i][4], fileData[i][2], locationData[i][1], analysisData[i][1], fileData[i][3], locationData[i][2], locationData[i][3]);
                  // alert(analysisData[i][0]);
                  // alert(locationData[i][0]);

              }
          }
          /** addCaseMarker(fileData[i][0], locationData[i][0], analysisData[i][0], fileData[i][1], fileData[i][4], fileData[i][2], locationData[i][1], analysisData[i][1], fileData[i][3], locationData[i][2], locationData[i][3]); **/

          function addCaseMarker(cID, locID, alyID, cTitle, cDesc, cDate, locAdd, alyResult, cPath, cX, cY) {
              var spatialReference = new SpatialReference({ wkid: 3414 });
              if ((cID == locID) && (cID == alyID)) {
                  if (alyResult > 75) {
                      var graphic;
                      var pictureSymbol = new PictureMarkerSymbol();
                      pictureSymbol.setUrl("../image/locationMarker.png");
                      var infoTemplate = new InfoTemplate("Child Abuse Case", "Title:" + cTitle + "<br/><br/>Description:" + cDesc + "<br/><br/>Date:" + cDate + "<br/><br/> location: " + locAdd + "<br/><br/>Anaylsis:" + alyResult + "<br/><br/> Source:" + cPath);
                      graphic = new Graphic(new Point(cX, cY, spatialReference), pictureSymbol, null, infoTemplate);
                      map.graphics.add(graphic);
                  }
                  else {

                      var graphic;
                      var pictureSymbol = new PictureMarkerSymbol();
                      pictureSymbol.setUrl("../image/locationMarker2.png");
                      var infoTemplate = new InfoTemplate("Child Abuse Case", "Title:" + cTitle + "<br/><br/>Description:" + cDesc + "<br/><br/>Date:" + cDate + "<br/><br/> location: " + locAdd + "<br/><br/>Anaylsis:" + alyResult + "<br/><br/> Source:" + cPath);
                      graphic = new Graphic(new Point(cX, cY, spatialReference), pictureSymbol, null, infoTemplate);
                      map.graphics.add(graphic);
                  }
              }
          }


          //adding child care center marker
          function addChildCare() {
              var searchText = "childcare"
              var returngeom = 1;
              var otptflds = "SEARCHVAL, CATEGORY, THEME, AGENCY";
              var ddlPageno = 1;
              var rset = 1;

              var themeSearch = new ThemeSearch;
              themeSearch.searchVal = searchText;
              themeSearch.returnGeom = returngeom;
              themeSearch.otptFlds = otptflds;
              themeSearch.rset = rset;
              themeSearch.GetThemeSearchResults(GetResult)
          }


          function GetResult(resultData) {

              var returngeom = 1;
              var childcareinfomarker = [];

              var pages = resultData.nop;
              var results = resultData.results;
              if (results == 'No results') {
                  alert("No Results of agency marker");
                  return false
              }
              else {
                  for (var i = 0; i < results.length; i++) {
                      var row = results[i];
                      childcareinfomarker = [row.SEARCHVAL, row.AGENCY, row.CATEGORY, row.THEME, row.X, row.Y];
                      childcareMarkerList.push(childcareinfomarker);
                      //Reverse GeoCode and display marker
                      var corridnates = row.X + "," + row.Y
                      getChildCareAddress(corridnates);
                      //End of Reverse
                  }
              }
          }

          //Getting Address From geocoding and display the marker
          function getChildCareAddress(corridnatesXY) {
              // debugger;
              var oneMapAddressInfoObj = new GetAddressInfo;
              oneMapAddressInfoObj.XY = corridnatesXY;

              oneMapAddressInfoObj.GetAddress(function (addressData) {
                  getCCAddress(addressData);
              });
          }

          function getCCAddress(addressData) {
              //debugger;
              if (addressData.results == "No results") {
                  return "No results";
                  // return false
              }
              else {
                  locationAddress = addressData.results[0].ROAD + ", " + addressData.results[0].POSTALCODE;
                  //addressData.results[0].XCOORD addressData.results[0].YCOORD 
                  //addressData.results[0].BUILDINGNAME + ", " + 

                  var graphic;
                  var spatialReference = new SpatialReference({ wkid: 3414 });
                  var pictureSymbol = new PictureMarkerSymbol();
                  pictureSymbol.setUrl("../image/childcare.png");

                  for (var i = 0; i < childcareMarkerList.length; i++) {
                      var infoTemplate = new InfoTemplate(
                       childcareMarkerList[i][1],
                        "Name:" + childcareMarkerList[i][0] + "<br/><br/>Category:" + childcareMarkerList[i][2] + "<br/><br/>Address:" + locationAddress
                      );
                      graphic = new Graphic(new Point(childcareMarkerList[i][4], childcareMarkerList[i][5], spatialReference), pictureSymbol, null, infoTemplate);
                      map.graphics.add(graphic);
                  }
              }
          }
          //Start of police station
          function policeStationMarker() {
              var xmlhttp = new XMLHttpRequest();
              xmlhttp.open("GET", "http://gis.sit.nyp.edu.sg/SAFETY_AT_SG_WEBSERVICE/SgDataService.asmx/GetPoliceStation", false);
              xmlhttp.send();
              var xmlDoc = xmlhttp.responseXML;
              var xmlalbums = xmlDoc.documentElement.getElementsByTagName("PoliceStation");
              map.infoWindow.resize(350, 120);

              $.each(xmlalbums, function () {
                  var name = $(this).find("ID").text();

                  policestation = $(this).find("Name").text();
                  address = $(this).find("Address").text();
                  postal = $(this).find("PostalCode").text();
                  coordX = $(this).find("X").text();
                  coordY = $(this).find("Y").text();

                  var graphic;
                  var spatialReference = new SpatialReference({ wkid: 3414 });
                  var pictureSymbol = new PictureMarkerSymbol();
                  pictureSymbol.setUrl("../image/spf.png");


                  var infoTemplate = new InfoTemplate("Police Station NO." + name, "Name:" + policestation + "<br/><br/>Address:" + address + "<br/><br/>Postal Code:" + postal);
                  graphic = new Graphic(new Point(coordX, coordY, spatialReference), pictureSymbol, null, infoTemplate);
                  map.graphics.add(graphic);

              });

          }
          //End of Police List Marker

          // fucntion clear Map
          function clear() {
              alert("clear");
              map.graphics.clear();
              markerList = [];
              childcareMarkerList = [];
              addChildCare();
              policeStationMarker();
              showme();
          }
          //end of require
      });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id ="main" class="tundra">
<!--Start of map -->
<div id="map" class="map">
<!--<div id="HomeButton"></div>-->
</div>
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
                    <!--<button data-dojo-type="dijit/form/Button">Polygon</button>-->
                        <td></td>
                        <td><div class="btn_design"><img src="image/draw.png" id="draw" onmouseover=" this.src = '../image/draw_hover.png'" onmouseout="this.src = '../image/draw.png'"  class="map_clear_btn" alt="draw" /></div></td>
                        <td><div class="btn_design"><img src="image/map_clear.png" id="map_clear" onmouseover=" this.src = '../image/map_clear_hover.png'" onmouseout="this.src = '../image/map_clear.png'"  class="map_clear_btn" alt="clear"/></div></td>
                    
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
                    <td><img src="image/locationMarker.png"/ alt ="child_abuse_case_marker" id="show"></td>
                    <td>Child Abuse Case</td>
                </tr>
                <tr>
                    <td></td>
                    <td><img src="image/locationMarker2.png"/ alt ="report_abuse_case_marker"></td>
                    <td>Report Case</td>
                </tr>
                 <tr>
                    <td></td>
                    <td><img src="image/childcare.png"/ alt ="child_care_marker"></td>
                    <td>Child Care Center</td>
                </tr>
                <tr>
                    <td></td>
                    <td><img src="image/spf.png"/ alt ="police_station_marker"></td>
                    <td>Police Station</td>
                </tr>
            </table>
            
        </div>
</div>

<!--End of feature1 -->


</div>
</asp:Content>
