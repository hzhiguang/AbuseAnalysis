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

                    //Face Analysis Pie Chart
                    var faceAnalysis = [['Smile', value[3]],
                                        ['Angry', value[4]],
                                        ['Sad', value[5]],
                                        ['Neutral', value[6]],
                                        ['No Detect', value[2] - value[3] - value[4] - value[5] - value[6]]];
                    $.jqplot('faceAnalysis', [faceAnalysis],
                    {
                        title: 'Face Analysis',
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

                    //Hand Motion Analysis Pie Chart
                    var handMotion = [['Left Fist', value[7]],
                                    ['Right Fist', value[8]],
                                    ['Left Palm', value[9]],
                                    ['Right Palm', value[10]],
                                    ['No Detect', value[2]-value[7]-value[8]-value[9]-value[10]]];
                    $.jqplot('handMotion', [handMotion],
                    {
                        title: 'Hand Motion Analysis',
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
    <h1 style="text-align:center">Analysis</h1>
    <table>
        <tbody>
            <tr>
                <td><div id="faceAnalysis" class="plot" style="height:600px; width:650px;"></div></td>
                <td><div id="handMotion" class="plot" style="height:600px; width:650px;"></div></td>
            </tr>
        </tbody>
    </table>
</asp:Content>
