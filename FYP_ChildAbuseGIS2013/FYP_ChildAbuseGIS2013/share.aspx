<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true"
    CodeBehind="share.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.share" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <title>Share With Us</title>
        <link rel="stylesheet" type="text/css" href="../../css/share.css">
        <link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css">
        <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
        <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
        <script type ="text/javascript"src="script/jquery.dataTables.js"></script>
        <script type ="text/javascript"src="script/jquery.dataTables.min.js"></script>
        <script type="text/javascript" src="../../js/share.js"></script>
        <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&libraries=places"></script>
        <script type="text/javascript">
            function onLoadFile(myForm) {
                var fileUpload = document.getElementById('<%=fileUpLoad.ClientID %>');
                document.getElementById('<%=txt_fileUpLoad.ClientID %>').value = fileUpload.value;
                return true;
            };

            function initialize() {
                var countryRestrict = { 'country': 'sg' };
                autocomplete = new google.maps.places.Autocomplete(
                    /** @type {HTMLInputElement} */(document.getElementById('autocomplete')),
                    {
                        types: ['geocode'],
                        componentRestrictions: countryRestrict
                    }
                );
                google.maps.event.addListener(autocomplete, 'place_changed', function () {
                    alert("testing");
                });
            }
        </script>
    </head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<body onload="initialize()">
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
                                <div id="locationField">
                                    <input id="autocomplete" placeholder="Enter address" type="text"></input>
                                </div>
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
</body>
</asp:Content>
