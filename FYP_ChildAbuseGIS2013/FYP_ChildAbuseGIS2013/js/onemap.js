require([

        "esri/map", "esri/dijit/Geocoder", "esri/tasks/locator", "esri/toolbars/draw", "esri/tasks/geometry", "esri/graphic", "myModules/InfoWindow", "dojo/dom",
        "dojo/dom-construct", "esri/InfoTemplate", "esri/symbols/SimpleMarkerSymbol", "esri/geometry/Point", "esri/symbols/Font", "esri/symbols/TextSymbol", "dojo/_base/lang", "dojo/json",
          "esri/config", "dojo/_base/array", "dojo/_base/Color",
        "dojo/number", "dojo/parser", "dojo/dom", "dijit/registry", "esri/layers/FeatureLayer", "esri/request", "esri/geometry/Extent", "esri/symbols/SimpleFillSymbol", "esri/symbols/PictureMarkerSymbol",
        "esri/renderers/ClassBreaksRenderer", "esri/layers/GraphicsLayer", "esri/SpatialReference", "esri/tasks/GeometryService", "esri/dijit/PopupTemplate", "esri/geometry/Point", "esri/geometry/webMercatorUtils", "esri/tasks/AreasAndLengthsParameters",
        "dijit/form/Button", "dijit/form/Textarea", "dijit/layout/BorderContainer", "dijit/layout/ContentPane", "dojo/domReady!", "esri/dijit/HomeButton", "esri/dijit/Scalebar"
      ], function (
        Map, Geocoder, Locator, Draw, geometry, Graphic, InfoWindow, dom,
        domConstruct,
        InfoTemplate, SimpleMarkerSymbol, Point,
        Font, TextSymbol, lang, json, esriConfig,
        arrayUtils, Color,
        number, parser, dom, registry, FeatureLayer, esriRequest, Extent, SimpleFillSymbol, PictureMarkerSymbol, ClassBreaksRenderer,
        GraphicsLayer, SpatialReference, GeometryService, PopupTemplate, Point, webMercatorUtils, AreasAndLengthsParameters, HomeButton, Scalebar
      //"extras/ClusterLayer",ClusterLayer
      ) {
          parser.parse();

          //Memebers variable
          var map, geocoder, locator, marker, gsvc, toolbar, home, location, locationAddress, raduisOfArea, areaDraw, lengthDraw, home, scalebar;
          var ManGeometry;
          var ManCorrX;
          var ManCorrY;
          var ManSymbol;
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
              esriConfig.defaults.io.proxyUrl = "/proxy.ashx";
              esriConfig.defaults.io.alwaysUseProxy = false;
              map = oneMap.map;
              map.infoWindow.resize(250, 150);
              addFileData();
              showme();
              addChildCare();
              policeStationMarker();
              autocomplete();
              GetKindergartens();
              //GetFamily();
              //GetVoluntaryWelfareOrgs();
              //GetStudentCare();
              createHomeButton();
              createScaleBar();
              dojo.connect(btnheatMap1, "onclick", heatInit);
              dojo.connect(draw, "onclick", activateTool);
              dojo.connect(map_clear, "onclick", clear);
              dojo.connect(search, "onclick", locate);
              createToolbar();
              
              //dojo.connect(map, "onClick", addPoint);
              //dojo.connect(draw, "onclick", activateTool());
          }

          dojo.addOnLoad(init);

          //this is to check the corrdinates on the map by clicking
          /**
          function addPoint(evt) {
          map.infoWindow.setTitle("Coordinates");
          map.infoWindow.setContent("lat/lon : " + evt.mapPoint.x + ", " + evt.mapPoint.y +
          "<br />screen x/y : " + evt.screenPoint.x + ", " + evt.screenPoint.y);
          map.infoWindow.show(evt.screenPoint, map.getInfoWindowAnchor(evt.screenPoint));
          }
          **/


          function createHomeButton() {
              home = new HomeButton({
                  map: map
              }, document.getElementById("HomeButton"));
              home.startup();
          }

          function createScaleBar() {
              scalebar = new Scalebar({
                  map: oneMap.map,
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
              toolbar.activate(esri.toolbars.Draw.FREEHAND_POLYGON); //CIRCLE
              map.hideZoomSlider();
          }

          function createToolbar(themap) {
              //console.log("initalise toolbar");
              toolbar = new Draw(map);
              //toolbar.on("draw-end", addToMap);
              toolbar.on("draw-end", lang.hitch(addToMap));
              gsvc = new esri.tasks.GeometryService("http://tasks.arcgisonline.com/ArcGIS/rest/services/Geometry/GeometryServer");
              gsvc.on("areas-and-lengths-complete", outputAreaAndLength);
          }

          function addToMap(evt) {
              //alert("hello");
              var symbol;
              toolbar.deactivate();
              map.showZoomSlider();
              switch (evt.geometry.type) {
                  case "point":
                  case "multipoint":
                      ManSymbol = new SimpleMarkerSymbol();
                      break;
                  case "polyline":
                      ManSymbol = new SimpleLineSymbol();
                      break;
                  default:
                      ManSymbol = new SimpleFillSymbol();
                      break;
              }
              ;
              //var customExtent = new Extent(26968.103, 39560.969, 26968.103, 39560.969, new SpatialReference({ wkid: 3414 }));
              ManGeometry = evt.geometry;


              //console.log(geometry.getCentroid().x);
              ManCorrX = ManGeometry.getCentroid().x;
              ManCorrY = ManGeometry.getCentroid().y;
              ManBuffer(ManGeometry);

          }

          function outputAreaAndLength(evtObj) {
              var result = evtObj.result;
              console.log(json.stringify(result));
              areaDraw = result.areas[0].toFixed(3);
              lengthDraw = result.lengths[0].toFixed(3);
              raduisOfArea = areaDraw / 3.14;
              raduisOfArea = Math.sqrt(raduisOfArea);
              console.log(raduisOfArea);
              displaybufferPara(ManCorrX, ManCorrY, ManGeometry);
              //dom.byId("area").innerHTML = areaDraw;
              //dom.byId("length").innerHTML = lengthDraw;
          }

          function ManBuffer(geometry) {
              var areasAndLengthParams = new AreasAndLengthsParameters();
              areasAndLengthParams.areaUnit = esri.tasks.GeometryService.UNIT_KILOMETER;
              areasAndLengthParams.lengthUnit = esri.tasks.GeometryService.UNIT_FOOT;
              gsvc.simplify([geometry], function (simplifiedGeometries) {
                  areasAndLengthParams.polygons = simplifiedGeometries;
                  gsvc.areasAndLengths(areasAndLengthParams);

              });

          }
          function displaybufferPara(x, y, symbol) {
              $.ajax({
                  type: "GET",
                  async: false,
                  url: "/api/json/bufferfile/" + raduisOfArea + "/" + x + "/" + y,
                  data: "{}",
                  contentType: "application/json; charset=utf-8",
                  //crossDomain: true,
                  dataType: "json",
                  //cache: false,
                  success: function (file) {
                      var statement = "";
                      bufferResult = [];
                      statement = "There are a total of <font colr='red'><b>" + bufferResult.length + "</b></font> reported child abuse case within <underline>" + raduisOfArea + "</underline> of this area <br/><br/>"
                      for (var i = 0; i < file.File.length; i++) {
                          bufferResult[i] = [file.File[i].ID, file.File[i].title, file.File[i].date, file.File[i].path, file.File[i].description, file.File[i].type, file.File[i].locationid, file.File[i].analysisid];
                          statement += "<b>Case</b> " + (i + 1) + " :" + bufferResult[i][1] + "<br/><br/><b>Description</b> :" + bufferResult[i][4] + "<br/><br/><b>Date</b>:" + bufferResult[i][2] + "<br/><br/>";
                      }
                      var infoTemplate = new InfoTemplate("FreeHand Polygon Buffer Result", "<b>Area</b>:" + areaDraw + " KM <br/><br/><b>Length</b>: " + lengthDraw + " Feet <br/><br/>" + statement);
                      var graphic = new Graphic(ManGeometry, ManSymbol, null, infoTemplate);
                      map.graphics.add(graphic);
                  },
                  error: function (file) {
                      alert("error");
                  }
              });

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
             // alert(geocoder.value);
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
                  new dojo.Color([0, 0, 255, 1]), 2
                ),
                new dojo.Color([255, 0, 0, 0.35])
              );


              dojo.forEach(geometries, function (geometry) {
                  var statement = "";
                  statement = "There are a total of <font color='red'><b>" + bufferResult.length + "</b></font> reported child abuse case within <underline>1</underline>KM of this area <br/><br/>"
                  for (var i = 0; i < bufferResult.length; i++) {
                      
                      statement += "<b>Case</b> " + (i + 1) + " :" + bufferResult[i][1] + "<br/><br/><b>Description</b> :" + bufferResult[i][4] + "<br/><br/><b>Date</b>:" + bufferResult[i][2] + "<br/><br/>";
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

          /*Adding Markers of Child Abuse Case from our own Database.
          The method going in this flow :
          1. retrieve all child abuse case(cac) from database
          2. retrieve individual location data by its location id from the cac
          3.retrieve individual location data by its analysis id from the cac
          */
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

          /*
          
          */
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


          /*
          Retrieve Child Care Corrdindate Using OneMap API
          */
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

          //Getting Child Care Address From GeoCoding
          function getChildCareAddress(corridnatesXY) {
              // debugger;
              var oneMapAddressInfoObj = new GetAddressInfo;
              oneMapAddressInfoObj.XY = corridnatesXY;

              oneMapAddressInfoObj.GetAddress(function (addressData) {
                  getCCAddress(addressData);
              });
          }
          //Display Child Care Marker
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
          //End of Child Care Center Marker

          /*
          Start of Police Station Marker
          Using webservice that is provided by the school to retrieve corrdinates of the police station
          and plotting on the map with markers
          */
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

          //clear all graphic layer on the Map and put back the marker
          function clear() {
              map.graphics.clear();
              markerList = [];
              childcareMarkerList = [];
              addChildCare();
              policeStationMarker();
              GetKindergartens();
              //GetFamily();
              //GetVoluntaryWelfareOrgs();
              //GetStudentCare();
              showme();
          }


          //Mashup 

          function GetKindergartens() {
              $.getJSON("http://gis.sit.nyp.edu.sg/safety_at_sg/proxy.ashx?http://www.onemap.sg/API/services.svc/mashupData?token=qo/s2TnSUmfLz+32CvLC4RMVkzEFYjxqyti1KhByvEacEdMWBpCuSSQ+IFRT84QjGPBCuz/cBom8PfSm3GjEsGc8PkdEEOEr&themeName=KINDERGARTENS&extents=-4423.6,15672.6,69773.4,52887.4", function (result) {
                  DisplayKindergartens(result.SrchResults);
              }
		);

          }

          function DisplayKindergartens(KindergartensJson) {
              //Loop All the Information From OneMap Mashup

              for (i = 1; i < KindergartensJson.length; i++) {
                  var XY = (KindergartensJson[i]['XY']).split(',');
                  var ADDRESSSTREETNAME = KindergartensJson[i]['ADDRESSSTREETNAME'];
                  var ADDRESSPOSTALCODE = KindergartensJson[i]['ADDRESSPOSTALCODE'];
                  var ADDRESSUNITNUMBER = KindergartensJson[i]['ADDRESSUNITNUMBER'];
                  var NAME = KindergartensJson[i]['NAME'];
                  var DESCRIPTION = KindergartensJson[i]['DESCRIPTION'];
                  var ICON_NAME = KindergartensJson[i]['ICON_NAME'];
                  var HYPERLINK = KindergartensJson[i]['HYPERLINK'];

                  var point = new esri.geometry.Point(XY[0], XY[1], map.spatialReference);

                  var symbol = new esri.symbol.PictureMarkerSymbol();
                  symbol.setUrl("http://www.onemap.sg/icons/Kindergartens/" + ICON_NAME);
                  symbol.setSize("20");


                  var infoTemplate = new esri.InfoTemplate();

                  infoTemplate.setTitle("<img src='image/loading.gif' style='width:25px; height:25px;'/>&nbsp;&nbsp; " + DESCRIPTION);
                  infoTemplate.setContent("<div style='word-wrap: break-word;'><b>Name: </b> <br/>" + NAME +
                  "<br/>" +
                  "<b>Address:  </b><br />" + ADDRESSPOSTALCODE + " " + ADDRESSSTREETNAME + " " + ADDRESSUNITNUMBER +
                  "<br />" +
                  "<b>More Info: </b> <a href='" + HYPERLINK + "' target='_blank'>" + HYPERLINK + "</a>" +
                  "</div>"
                  );
                  var PointGraphic = new esri.Graphic(point, symbol, null, infoTemplate);
                  //PointGraphic.setSymbol(symbol);
                  //PointGraphic.setInfoTemplate(infoTemplate);
                  map.graphics.add(PointGraphic);
                  //Kindergartens.push();
              }
          }

          function GetStudentCare() {

              $.getJSON("http://gis.sit.nyp.edu.sg/safety_at_sg/proxy.ashx?http://www.onemap.sg/API/services.svc/mashupData?token=qo/s2TnSUmfLz+32CvLC4RMVkzEFYjxqyti1KhByvEacEdMWBpCuSSQ+IFRT84QjGPBCuz/cBom8PfSm3GjEsGc8PkdEEOEr&themeName=STUDENTCARE&extents=-4423.6,15672.6,69773.4,52887.4", function (result) {
                  DisplayStudentCare(result.SrchResults);
              }
		);

          }

          function DisplayStudentCare(StudentCareJson) {
              //Loop All the Information From OneMap Mashup

              for (i = 1; i < StudentCareJson.length; i++) {
                  var XY = (StudentCareJson[i]['XY']).split(',');
                  var ADDRESSSTREETNAME = StudentCareJson[i]['ADDRESSSTREETNAME'];
                  var ADDRESSPOSTALCODE = StudentCareJson[i]['ADDRESSPOSTALCODE'];
                  var ADDRESSUNITNUMBER = StudentCareJson[i]['ADDRESSUNITNUMBER'];
                  var NAME = StudentCareJson[i]['NAME'];
                  var DESCRIPTION = StudentCareJson[i]['DESCRIPTION'];
                  var ICON_NAME = StudentCareJson[i]['ICON_NAME'];
                  var HYPERLINK = StudentCareJson[i]['HYPERLINK'];

                  var point = new esri.geometry.Point(XY[0], XY[1], map.spatialReference);
                  /*http://www.onemap.sg/icons/StudentCare/ + ICON_NAME*/
                  var symbol = new esri.symbol.PictureMarkerSymbol();
                  symbol.setUrl("http://www.onemap.sg/icons/StudentCare/" + ICON_NAME);
                  symbol.setSize("20");


                  var infoTemplate = new esri.InfoTemplate();

                  infoTemplate.setTitle("<img src='image/loading.gif' style='width:25px; height:25px;'/>&nbsp;&nbsp; " + DESCRIPTION);
                  infoTemplate.setContent("<div style='word-wrap: break-word;'><b>Name: </b> <br/>" + NAME +
                  "<br/>" +
                  "<b>Address:  </b><br />" + ADDRESSPOSTALCODE + " " + ADDRESSSTREETNAME + " " + ADDRESSUNITNUMBER +
                  "<br />" +
                  "<b>More Info: </b> <a href='" + HYPERLINK + "' target='_blank'>" + HYPERLINK + "</a>" +
                  "</div>"
                  );
                  var PointGraphic = new esri.Graphic(point, symbol, null, infoTemplate);
                  //PointGraphic.setSymbol(symbol);
                  //PointGraphic.setInfoTemplate(infoTemplate);
                  map.graphics.add(PointGraphic);
                  //Kindergartens.push();
              }
          }

          function GetFamily() {

              $.getJSON("http://gis.sit.nyp.edu.sg/safety_at_sg/proxy.ashx?http://www.onemap.sg/API/services.svc/mashupData?token=qo/s2TnSUmfLz+32CvLC4RMVkzEFYjxqyti1KhByvEacEdMWBpCuSSQ+IFRT84QjGPBCuz/cBom8PfSm3GjEsGc8PkdEEOEr&themeName=FAMILY&extents=-4423.6,15672.6,69773.4,52887.4", function (result) {
                  DisplayFamily(result.SrchResults);
              }
		);

          }

          function DisplayFamily(FamilyJson) {
              //Loop All the Information From OneMap Mashup

              for (i = 1; i < FamilyJson.length; i++) {
                  var XY = (FamilyJson[i]['XY']).split(',');
                  var ADDRESSSTREETNAME = FamilyJson[i]['ADDRESSSTREETNAME'];
                  var ADDRESSPOSTALCODE = FamilyJson[i]['ADDRESSPOSTALCODE'];
                  var ADDRESSUNITNUMBER = FamilyJson[i]['ADDRESSUNITNUMBER'];
                  var NAME = FamilyJson[i]['NAME'];
                  var DESCRIPTION = FamilyJson[i]['DESCRIPTION'];
                  var ICON_NAME = FamilyJson[i]['ICON_NAME'];
                  var HYPERLINK = FamilyJson[i]['HYPERLINK'];

                  var point = new esri.geometry.Point(XY[0], XY[1], map.spatialReference);
                  /*http://www.onemap.sg/icons/Family/ + ICON_NAME*/
                  var symbol = new esri.symbol.PictureMarkerSymbol();
                  symbol.setUrl("http://www.onemap.sg/icons/Family/" + ICON_NAME);
                  symbol.setSize("20");


                  var infoTemplate = new esri.InfoTemplate();

                  infoTemplate.setTitle("<img src='image/loading.gif' style='width:25px; height:25px;'/>&nbsp;&nbsp; " + DESCRIPTION);
                  infoTemplate.setContent("<div style='word-wrap: break-word;'><b>Name: </b> <br/>" + NAME +
                  "<br/>" +
                  "<b>Address:  </b><br />" + ADDRESSPOSTALCODE + " " + ADDRESSSTREETNAME + " " + ADDRESSUNITNUMBER +
                  "<br />" +
                  "<b>More Info: </b> <a href='" + HYPERLINK + "' target='_blank'>" + HYPERLINK + "</a>" +
                  "</div>"
                  );
                  var PointGraphic = new esri.Graphic(point, symbol, null, infoTemplate);
                  //PointGraphic.setSymbol(symbol);
                  //PointGraphic.setInfoTemplate(infoTemplate);
                  map.graphics.add(PointGraphic);
                  //Kindergartens.push();
              }
          }

          function GetVoluntaryWelfareOrgs() {

              $.getJSON("http://gis.sit.nyp.edu.sg/safety_at_sg/proxy.ashx?http://www.onemap.sg/API/services.svc/mashupData?token=qo/s2TnSUmfLz+32CvLC4RMVkzEFYjxqyti1KhByvEacEdMWBpCuSSQ+IFRT84QjGPBCuz/cBom8PfSm3GjEsGc8PkdEEOEr&themeName=VOLUNTARYWELFAREORGS&extents=-4423.6,15672.6,69773.4,52887.4", function (result) {
                  DisplayVoluntaryWelfareOrgs(result.SrchResults);
              }
		);

          }

          function DisplayVoluntaryWelfareOrgs(VoluntaryWelfareOrgsJson) {
              //Loop All the Information From OneMap Mashup

              for (i = 1; i < VoluntaryWelfareOrgsJson.length; i++) {
                  var XY = (VoluntaryWelfareOrgsJson[i]['XY']).split(',');
                  var ADDRESSSTREETNAME = VoluntaryWelfareOrgsJson[i]['ADDRESSSTREETNAME'];
                  var ADDRESSPOSTALCODE = VoluntaryWelfareOrgsJson[i]['ADDRESSPOSTALCODE'];
                  var ADDRESSUNITNUMBER = VoluntaryWelfareOrgsJson[i]['ADDRESSUNITNUMBER'];
                  var NAME = VoluntaryWelfareOrgsJson[i]['NAME'];
                  var DESCRIPTION = VoluntaryWelfareOrgsJson[i]['DESCRIPTION'];
                  var ICON_NAME = VoluntaryWelfareOrgsJson[i]['ICON_NAME'];
                  var HYPERLINK = VoluntaryWelfareOrgsJson[i]['HYPERLINK'];

                  var point = new esri.geometry.Point(XY[0], XY[1], map.spatialReference);
                  /*http://www.onemap.sg/icons/VoluntaryWelfareOrgs/ + ICON_NAME*/
                  var symbol = new esri.symbol.PictureMarkerSymbol();
                  symbol.setUrl("http://www.onemap.sg/icons/VoluntaryWelfareOrgs/" + ICON_NAME);
                  symbol.setSize("20");


                  var infoTemplate = new esri.InfoTemplate();

                  infoTemplate.setTitle("<img src='image/loading.gif' style='width:25px; height:25px;'/>&nbsp;&nbsp; " + DESCRIPTION);
                  infoTemplate.setContent("<div style='word-wrap: break-word;'><b>Name: </b> <br/>" + NAME +
                  "<br/>" +
                  "<b>Address:  </b><br />" + ADDRESSPOSTALCODE + " " + ADDRESSSTREETNAME + " " + ADDRESSUNITNUMBER +
                  "<br />" +
                  "<b>More Info: </b> <a href='" + HYPERLINK + "' target='_blank'>" + HYPERLINK + "</a>" +
                  "</div>"
                  );
                  var PointGraphic = new esri.Graphic(point, symbol, null, infoTemplate);
                  //PointGraphic.setSymbol(symbol);
                  //PointGraphic.setInfoTemplate(infoTemplate);
                  map.graphics.add(PointGraphic);
                  //Kindergartens.push();
              }
          }

          // heat map 2  stuff

          function heatInit() {

              heatLayer = new HeatmapLayer({
                  config: {
                      "useLocalMaximum": true,
                      "radius": 20,
                      "gradient": {
                          0.45: "rgb(000,000,255)",
                          0.55: "rgb(000,255,255)",
                          0.65: "rgb(000,255,000)",
                          0.95: "rgb(255,255,000)",
                          1.00: "rgb(255,000,000)"
                      }
                  },
                  "map": map,
                  "domNodeId": "heatLayer",
                  "opacity": 0.85
              });
              //POIFeatureLayer = new esri.layers.FeatureLayer("http://172.20.129.239/RoadWatchDummyData(Json)V9/WcfService.svc/Json");
              //POIFeatureLayer = new esri.layers.FeatureLayer("http://localhost/RoadWatchDummyData/SgDataService.asmx/GetZebraCrossing");
              //POIFeatureLayer = new esri.layers.FeatureLayer("http://www.onemap.sg/DataService/Services.svc/Disability");
              POIFeatureLayer = new esri.layers.FeatureLayer("http://sitarcgis2.sit.nyp.edu.sg/ArcGIS/rest/services/POIs/MapServer/1");
              map.graphics.add(heatLayer);
              alert("HJI");
              getHeatmapKindergartens();
              //return true;
              /**
              if (heatLayer == null) {
              } else {
              map.graphics.remove(heatLayer);
              //map.removeLayer(map.getLayer(map.graphicsLayerIds["heatLayer"]));
              POIFeatureLayer = null;
              heatLayer = null;
              return false;
              }
              **/
          }

          function getHeatmapKindergartens() {

              var overwrite = false;
              var where = "TYPE=''";


              if (overwrite == false) {
                  where = "TYPE='KINDERGARTENS'";
                  overwrite = true;
              } else {
                  where += " OR TYPE='KINDERGARTENS'";
              }

              console.log(where);
              getFeatures(where);


          }

          function getHeatmapHealth() {
              if (heatInit()) {
                  var overwrite = false;
                  var where = "TYPE=''";

                  if (document.getElementById('cbClinic').checked) {
                      if (overwrite == false) {
                          where = "TYPE='CLINIC'";
                          overwrite = true;
                      } else {
                          where += " OR TYPE='CLINIC'";
                      }
                  }

                  if (document.getElementById('cbHospital').checked) {
                      if (overwrite == false) {
                          where = "TYPE='HOSPITAL'";
                          overwrite = true;
                      } else {
                          where += "";
                      }
                  }
                  console.log(where);
                  getFeatures(where);
              }
              else { }
          }


          function getFeatures(where) {
              //alert("we are stuck at here");
              // set up query
              var query = new esri.tasks.Query();
              // only within extent
              query.geometry = map.extent;
              // give me all of them!
              query.where = where;
              // make sure I get them back in my spatial reference
              query.geometryType = "esriGeometryEnvelope";
              query.outSpatialReference = map.spatialReference;
              query.returnGeometry = true;
              // get em!
              console.log(query);
              var data = [];
              POIFeatureLayer.queryFeatures(query, function (featureSet) {
                  // if we get results back
                  if (featureSet && featureSet.features && featureSet.features.length > 0) {
                      // set data to features
                      //alert("we are getting the data");
                      data = featureSet.features;

                  }
                  // set heatmap data
                  //heatLayer.setData(data);
              });

              var data = ['{attributes: {},geometry: {spatialReference: {wkid: 3414} type: "point"x: 30761.221000016 y: 36885.9490036653 }}'];
              //        
              heatLayer.setData(data);
              //alert("end of everything");
          }


          //end of require
        
      });