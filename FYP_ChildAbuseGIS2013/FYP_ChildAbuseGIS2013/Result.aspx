<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="result.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.Result" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<head>
    <title>Analysis Result</title>
    <link rel="stylesheet" type="text/css" href="../../css/jquery.jqplot.min.css" />
    <script type="text/javascript" src="../../script/chart/jquery.jqplot.min.js"></script>
    <script type="text/javascript" src="../../script/chart/jqplot.pieRenderer.min.js"></script>
    <script type="text/javascript" src="../../script/chart/jqplot.donutRenderer.min.js"></script>
    <script type="text/javascript" charset="utf-8">
        $.urlParam = function (name) {
            var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
            if (results == null) {
                return null;
            }
            else {
                return results[1] || 0;
            }
        }

        $(document).ready(function () {
            var id = $.urlParam('ID');

            var data_file = "http://localhost:27020/api/json/analysis/" + id;
            var http_request = new XMLHttpRequest();
            try {
                // Opera 8.0+, Firefox, Chrome, Safari
                http_request = new XMLHttpRequest();
            } catch (e) {
                // Internet Explorer Browsers
                try {
                    http_request = new ActiveXObject("Msxml2.XMLHTTP");
                } catch (e) {
                    try {
                        http_request = new ActiveXObject("Microsoft.XMLHTTP");
                    } catch (e) {
                        // Something went wrong
                        alert("Your browser broke!");
                        return false;
                    }
                }
            }
            http_request.onreadystatechange = function () {
                if (http_request.readyState == 4) {
                    // Javascript function JSON.parse to parse JSON data
                    var jsonObj = JSON.parse(http_request.responseText);
                    var name = [];
                    var value = [];
                    var a = 0;
                    for (var att in jsonObj.Analysis) {
                        name[a] = att;
                        value[a] = jsonObj.Analysis[att];
                        a++;
                    }
                    var newData = [['Left Fist', value[6]],
                                    ['Right Fist', value[7]],
                                    ['Left Palm', value[8]],
                                    ['Right Palm', value[8]],
                                    ['No Detect', value[1]-value[6]-value[7]-value[8]-value[9]]];
                    $.jqplot('chart1', [newData],
                    {
                        seriesDefaults: {
                            // Make this a pie chart.
                            renderer: jQuery.jqplot.PieRenderer,
                            rendererOptions: {
                                // Put data labels on the pie slices.
                                // By default, labels show the percentage of the slice.
                                showDataLabels: true
                            }
                        },
                        legend: { show: true, location: 'e' }
                    });
                }
            }
            http_request.open("GET", data_file, true);
            http_request.send();
        });
    </script>
</head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="chart1" class="plot" style="height:400px; width:300px;"></div>
</asp:Content>
