<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true"
    CodeBehind="share.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.share" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <title>Share With Us</title>
        <link rel="stylesheet" type="text/css" href="../../css/share.css">
        <link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css">
        <script type="text/javascript" src="../../script/TableTools.min.js"></script>
        <script type="text/javascript" src="../../js/share.js"></script>
        <script type="text/javascript">
            function onLoadFile(myForm) {
                var fileUpload = document.getElementById('<%=fileUpLoad.ClientID %>');
                document.getElementById('<%=txt_fileUpLoad.ClientID %>').value = fileUpload.value;
                return true;
            }

            var location = document.getElementById("tbLocation");
            $(function () {
                $("#tbLocation").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=Yishun&types=geocode&components=country:sg&sensor=true&key=AIzaSyB8VvHDdnvy4ZSiACllPSOw8FmduyfhvdI",
                            dataType: "jsonp",
                            data: {
                                featureClass: "predictions",
                                style: "full",
                                maxRows: 12,
                                name_startsWith: request.term
                            },
                            success: function( data ) {
                                response( $.map( data.geonames, function( item ) {
                                    return {
                                        label: item.name + (item.adminName1 ? ", " + item.adminName1 : "") + ", " + item.countryName,
                                        value: item.name
                                    }
                                }));
                            }
                        });
                    },
                    minLength: 2,
                    select: function( event, ui ) {
                        alert("Hi");
                    }
                });
            });
        </script>
    </head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="Form1" runat="server" method="post">
    <table class="tableStyle">
        <tr>
            <td colspan="2">
                <div class="image">
                    <center><asp:Image ID="Image1" runat="server" ImageUrl="~/image/Share.png" /></center>
                    <hr />
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="submitCorner">
                    <table>
                        <tr>
                            <td>
                                Video/Image :
                            </td>
                            <td>
                                <div class="upLoadContain">
                                    <asp:TextBox ID="txt_fileUpLoad" class="file" runat="server" />
                                    <div class="fileUpLoadContain">
                                        <asp:FileUpload ID="fileUpLoad" name="fileUpLoad" runat="server" onchange="return onLoadFile(this);" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="headerAlignment">
                                Title:
                            </td>
                            <td>
                                <asp:TextBox ID="tbTitle" class="tbStyle" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Address:
                            </td>
                            <td>
                                <asp:TextBox ID="tbLocation" class="tbStyle" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="headerAlignment">
                                Description:
                            </td>
                            <td>
                                <asp:TextBox ID="tbDescription" class="tbStyle" runat="server" Height="105px" TextMode="MultiLine"
                                    Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center><asp:Button ID="btnSubmit" runat="server" Text="Analyze"  name="Upload" class="submitBtn" onclick="upload_Click"  OnClientClick="skm_LockScreen('Analyze Processing , Please Wait');" />&nbsp;&nbsp;&nbsp;<asp:Label ID="lb_msg" runat="server"></center>
                                </asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" class="dataTable" id="example">
                </table>
            </td>
        </tr>
    </table>
    <div id="skm_LockPane" class="LockOff">
    </div>
    </form>
</asp:Content>
