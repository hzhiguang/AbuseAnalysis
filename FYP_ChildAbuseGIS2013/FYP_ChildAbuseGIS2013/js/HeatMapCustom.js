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

        if (document.getElementById('cbKindergarten').checked) {
            if (overwrite == false) {
                where = "TYPE='KINDERGARTENS'";
                overwrite = true;
            } else {
                where += " OR TYPE='KINDERGARTENS'";
            }
        }

        if (document.getElementById('cbStudentCare').checked) {
            if (overwrite == false) {
                where = "TYPE='STUDENTCARE'";
                overwrite = true;
            } else {
                where += "";
            }
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
alert("we are stuck at here");
    // set up query
    var query = new esri.tasks.Query();
    // only within extent
    query.geometry = map.extent;
    // give me all of them!
    query.where = where;
    // make sure I get them back in my spatial reference
    query.geometryType = "esriGeometryEnvelope";
    query.outSpatialReference = map.spatialReference;
    // get em!
    console.log(query);
    POIFeatureLayer.queryFeatures(query, function (featureSet) {
        var data = [];
        // if we get results back
        if (featureSet && featureSet.features && featureSet.features.length > 0) {
            // set data to features
             data = featureSet.features;
            
        }
        // set heatmap data
        heatLayer.setData(data);
        });

//    var data = ['{attributes: {},geometry: {spatialReference: {wkid: 3414} type: "point"x: 30761.221000016 y: 36885.9490036653 }}'];
//        
//        heatLayer.setDataSet(data);
}

