﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="share.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.share" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<head>
    <title>Share With Us</title>
    <link rel="stylesheet" type="text/css" href="../../css/share.css">
    <link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css">
    <script type ="text/javascript"src="../../script/TableTools.min.js"></script>
    <script type="text/javascript" charset="utf-8">
        var resultTable;
        $(document).ready(function () {
            resultTable = $("#example").dataTable({
                "bProcessing": true,
                "sAjaxSource": "http://localhost:27020/api/json/file",
                "sAjaxDataProp": "File",
                "aoColumns": [
                                { "sTitle": "ID", "mData": "ID" },
                                { "sTitle": "Title", "mData": "title" },
                                { "sTitle": "Date", "mData": "date" },
                                { "sTitle": "Description", "mData": "description" },
                                { "sTitle": "Type", "mData": "type" }
                            ],
                "bPaginate": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": true
            });

            $("#example tbody").delegate("tr", "click", function () {
                var iPos = resultTable.fnGetPosition(this);
                if (iPos != null) {
                    window.location.href = "result.aspx?ID=" + iPos + "";
                }
            });
        });

        function onLoadFile(myForm) {
            var fileUpload = document.getElementById('<%=fileUpLoad.ClientID %>');
            document.getElementById('<%=txt_fileUpLoad.ClientID %>').value = fileUpload.value;
            return true;
        }

        function skm_LockScreen(str) {
            var lock = document.getElementById('skm_LockPane');
            if (lock)
                lock.className = 'LockOn';

            lock.innerHTML = str + "<br/><img src=@'../../image/loading.gif' alt='loading'/>";
        }
    </script>
</head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="Form1" runat="server" method="post" >
    <table class="tableStyle">
       <tr>
            <td class="style3"> 
                <div class="image">
                    <asp:Image ID="iconLogo1" runat="server" ImageUrl="~/image/Share.png" />
                </div>
            </td>
            <td class="style2">
                <div class="submitCorner">
                    <table>
                        <tr>
                            <td>Video/Image :</td>
                            <td>
                                <div class="upLoadContain">
                                    <asp:TextBox ID="txt_fileUpLoad" class="file" runat="server" />
                                    <div class ="fileUpLoadContain">
                                        <asp:FileUpload ID="fileUpLoad" name="fileUpLoad" runat="server" onchange="return onLoadFile(this);" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="headerAlignment">Title:</td>
                            <td><asp:TextBox ID="tbTitle" class="tbStyle" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Location:</td>
                            <td><input type="text" id="tbLocation" runat="server" class="tbStyle"/></td>
                        </tr>
                        <tr>
                            <td class="headerAlignment">Description:</td>
                            <td><asp:TextBox ID="tbDescription" class="tbStyle" runat="server" Height="105px" TextMode="MultiLine" Width="300px" /></td>
                        </tr>
                        <tr>
                            <td colspan="2"><asp:Button ID="btnSubmit" runat="server" Text="Analyze"  name="Upload" class="submitBtn" onclick="upload_Click"  OnClientClick="skm_LockScreen('Analyze Processing , Please Wait');" />&nbsp;&nbsp;&nbsp;<asp:Label ID="lb_msg" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div> 
            </td>
       </tr>
       <tr>
            <td colspan="2">
                <table cellpadding="0" cellspacing="0" border="0" class="dataTable" id="example">
                </table>
            </td>
       </tr>
    </table>
    <div id="skm_LockPane" class="LockOff">
    </div>
    </form>
</asp:Content>