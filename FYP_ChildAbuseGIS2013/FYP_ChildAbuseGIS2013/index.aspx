<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type='text/JavaScript' src='http://www.onemap.sg/API/JS?accessKEY=qo/s2TnSUmfLz+32CvLC4RMVkzEFYjxqyti1KhByvEacEdMWBpCuSSQ+IFRT84QjGPBCuz/cBom8PfSm3GjEsGc8PkdEEOEr&v=2.8&type=full'></script>
<script language="text/JavaScript" type="text/JavaScript">
    var map=new GetOneMap('map','sm',{level:1});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="map">
    
</div>
</asp:Content>
