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
#HomeButton 
{
     
}
div.featureStyle1
{
    height:50px;
    width:450px;
    background-color:#062032;
    background:rgb(6, 32, 50); /* Fallback for older browsers without RGBA-support */
    background:rgba(6, 32, 50, 0.5);
    position:absolute;
    filter:alpha(opacity=60);
    margin-top:-750px;
    border:2px solid #000000;
    -webkit-border-top-right-radius: 5px;
	-webkit-border-bottom-right-radius: 5px;
	-moz-border-radius-topright: 5px;
	-moz-border-radius-bottomright: 5px;
	border-top-right-radius: 5px;
	border-bottom-right-radius: 5px;
   
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
    margin-left:50px;
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
<script type="text/JavaScript" src="../js/onemap.js"></script>  
<script type="text/JavaScript" src="../js/heatmap.js"></script> 
<script type="text/JavaScript" src="../js/heatmap-arcgis.js"></script> 
<script type="text/JavaScript" src="../js/HeatMapCustom.js"></script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id ="main" class="tundra">
<!--Start of map -->
<div id="map"  >
        
        <div id="heatLayer"></div>
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
                        <td><div id="HomeButton"></div></td>
                        <td><div class="btn_heatmap"><img src="image/heatmap.png" id="btnheatMap1" onmouseover=" this.src = '../image/heatmap_hover.png'" onmouseout="this.src = '../image/heatmap.png'"  class="map_clear_btn" alt="heatmap" /></div></td>
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
    <div style="margin-top:25px;">Legend</div>
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
                 <tr>
                    <td></td>
                    <td><img src="image/kindergarten.png"/ alt ="kindergarten_marker"></td>
                    <td>Kindergarten</td>
                </tr>
            </table>
            
        </div>
</div>

<!--End of feature1 -->


</div>
</asp:Content>
