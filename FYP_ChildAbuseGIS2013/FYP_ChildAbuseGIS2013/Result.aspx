<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="result.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.Result" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<head>
    <title>Analysis Result</title>
    <link rel="stylesheet" type="text/css" href="../../css/jquery.jqplot.min.css" />
    <link rel="Stylesheet" type="text/css" href="../../css/resultChart.css"/>
    <script type="text/javascript" src="../../script/chart/jquery.jqplot.min.js"></script>
    <script type="text/javascript" src="../../script/chart/jqplot.pieRenderer.min.js"></script>
    <script type="text/javascript" src="../../script/chart/jqplot.donutRenderer.min.js"></script>
    <script type="text/javascript" src="../../script/chart/chartSlideshow.js"></script>
</head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 style="text-align:center">Analysis</h1>
    <div id="chartSlideShow">
        <img id="next" src="../../image/slideshow/next.png" alt="" />
        <img id="prev" src="../../image/slideshow/prev.png" alt="" />
        <div id="overallAnalysis" class="plot"></div>
        <div id="faceAnalysis" class="plot"></div>
        <div id="handMotion" class="plot"></div>
        <div id="soundAnalysis class="plot"></div>
        <div id="feverAnalysis" class="plot"></div>
        <div id="twitterAna" class="plot"></div>
    </div>
</asp:Content>