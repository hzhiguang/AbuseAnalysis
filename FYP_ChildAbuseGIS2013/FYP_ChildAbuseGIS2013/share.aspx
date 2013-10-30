<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="share.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.share" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
    #uploadContain
    {
        border:2px solid black;
        width:220px;
        height:30px;
        padding:20px;
        margin-left:460px;
        margin-top:100px;
        background-color:Blue;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
<div id = "uploadContain">
    <asp:FileUpload ID="FileUpload1" runat="server" />
</div>
    </form>
</asp:Content>
