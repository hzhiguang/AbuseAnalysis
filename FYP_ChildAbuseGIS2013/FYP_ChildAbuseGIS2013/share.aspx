<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="share.aspx.cs" Inherits="FYP_ChildAbuseGIS2013.share" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
    <title>Share With Us</title>
</head>
<script>
    $(document).ready(function () {
        $("#resultTable").dataTable({
            "bProcessing": true,
            "sAjaxSource": "http://localhost:27020/api/json/file",
            "sAjaxDataProp": "file",
            "bPaginate": true,
            "bLengthChange": false,
            "bFilter": true,
            "bSort": false,
            "bInfo": true,
            "bAutoWidth": true
        });

        $.ajax({
            type: "GET",
            url: "http://localhost:27020/api/json/file",
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                if (data.success) {
                    alert("LOL");
                    //$('#nric').val(data.patientInfo.nric.toString());
                    //$('#handphone').val(data.patientInfo.mobileNo.toString());
                    //$('#email').val(data.patientInfo.emailAddress);
                    //$('#address').val(data.patientInfo.address);
                }
            }
        });
    });
</script>
<style type="text/css">
div.upLoadContain {
	width:400px;
	height: 20px;

}
input.file {
	border-left: 1px solid #BBB;
        border-top: 1px solid #BBB;
        border-bottom: 1px solid #BBB;
        width: 300px;
	    height: 20px;
	    color: #888;
	   
	
	    -webkit-border-top-left-radius: 5px;
	    -webkit-border-bottom-left-radius: 5px;
	    -moz-border-radius-topleft: 5px;
	    -moz-border-radius-bottomleft: 5px;
	    border-top-left-radius: 5px;
	    border-bottom-left-radius: 5px;
	
	    outline: none;
        border-right-style: none;
        border-right-color: inherit;
        border-right-width: 0;
        margin-top: 1px;
    }

div.fileUpLoadContain {
	width: 80px;
	height: 24px;
	background: #7abcff;
	background: -moz-linear-gradient(top,  #7abcff 0%, #60abf8 44%, #4096ee 100%);
	background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#7abcff), color-stop(44%,#60abf8), color-stop(100%,#4096ee));
	background: -webkit-linear-gradient(top,  #7abcff 0%,#60abf8 44%,#4096ee 100%);
	background: -o-linear-gradient(top,  #7abcff 0%,#60abf8 44%,#4096ee 100%);
	background: -ms-linear-gradient(top,  #7abcff 0%,#60abf8 44%,#4096ee 100%);
	background: linear-gradient(top,  #7abcff 0%,#60abf8 44%,#4096ee 100%);
	filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#7abcff', endColorstr='#4096ee',GradientType=0 );

	display: inline;
	position: absolute;
	overflow: hidden;
	cursor: pointer;
	
	-webkit-border-top-right-radius: 5px;
	-webkit-border-bottom-right-radius: 5px;
	-moz-border-radius-topright: 5px;
	-moz-border-radius-bottomright: 5px;
	border-top-right-radius: 5px;
	border-bottom-right-radius: 5px;
	
	font-weight: bold;
	color: #FFF;
	text-align: center;
}
div.fileUpLoadContain:before {
	content: 'Choose File';
	position: absolute;
	left: 0; right: 0;
	text-align: center;
	
	cursor: pointer;
}

div.fileUpLoadContain input {
	position: relative;
	height: 30px;
	width: 250px;
	display: inline;
	cursor: pointer;
	opacity: 0;
        top: -13px;
        left: 5px;
    }


input.submitBtn{
-moz-box-shadow:inset 0px 1px 0px 0px #97c4fe;
	-webkit-box-shadow:inset 0px 1px 0px 0px #97c4fe;
	box-shadow:inset 0px 1px 0px 0px #97c4fe;
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #3d94f6), color-stop(1, #1e62d0) );
	background:-moz-linear-gradient( center top, #3d94f6 5%, #1e62d0 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#3d94f6', endColorstr='#1e62d0');
	background-color:#3d94f6;
	-webkit-border-top-left-radius:18px;
	-moz-border-radius-topleft:18px;
	border-top-left-radius:18px;
	-webkit-border-top-right-radius:18px;
	-moz-border-radius-topright:18px;
	border-top-right-radius:18px;
	-webkit-border-bottom-right-radius:18px;
	-moz-border-radius-bottomright:18px;
	border-bottom-right-radius:18px;
	-webkit-border-bottom-left-radius:18px;
	-moz-border-radius-bottomleft:18px;
	border-bottom-left-radius:18px;
	text-indent:0px;
	display:inline-block;
	color:#ffffff;
	font-family:Verdana;
	font-size:15px;
	font-weight:bold;
	font-style:normal;
	height:40px;
	line-height:40px;
	width:100px;
	text-decoration:none;
	text-align:center;
}
input.submitBtn:hover {
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #1e62d0), color-stop(1, #3d94f6) );
	background:-moz-linear-gradient( center top, #1e62d0 5%, #3d94f6 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#1e62d0', endColorstr='#3d94f6');
	background-color:#1e62d0;
}
input.submitBtn:active {
	position:relative;
	top:0px;
}

div.image
{
    width:200;
    height:75px;
    margin-left:80px;
}
table.tableStyle
{
    width:1200px;
}
table.tableStyle td
{
    border: 0px solid #1e62d0;
    padding:12px;
    border-collapse:collapse;
}
div.resultCorner
{
    width:700px;
    margin-bottom:700px;
    
}

div.LockOff {
			display: none;
			visibility: hidden;
		}
		
div.LockOn {
			display: block;
			visibility: visible;
			position: absolute;
			z-index: 999;
			top: 0px;
			left: 0px;
			width: 105%;
			height: 105%;
			background-color: #ccc;
			text-align: center;
			padding-top: 20%;
			filter: alpha(opacity=75);
			opacity: 0.75;
		}
.tbStyle
{
    -webkit-border-top-left-radius: 5px;
	-webkit-border-bottom-left-radius: 5px;
	-moz-border-radius-topleft: 5px;
	-moz-border-radius-bottomleft: 5px;
	border-top-left-radius: 5px;
	border-bottom-left-radius: 5px;
	border-top-right-radius: 5px;
	border-bottom-right-radius: 5px;
	border: 1px solid #BBB;
	color: #888;
	outline: none;
	width:300px;
	
}
.headerAlignment
{
   text-align:left;
	vertical-align:top; 
}
    
</style>
<script type="text/javascript">

function onLoadFile(myForm) {
    var fileUpload = document.getElementById('<%=fileUpLoad.ClientID %>');
    document.getElementById('<%=txt_fileUpLoad.ClientID %>').value = fileUpload.value;
    return true;
}
</script>
<script type="text/javascript">
    function skm_LockScreen(str) {
        var lock = document.getElementById('skm_LockPane');
        if (lock)
            lock.className = 'LockOn';

        lock.innerHTML = str + "<br/><img src=@'../../image/loading.gif' alt='loading'/>";
    }
	</script>


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
                                <td><div class = "upLoadContain"><asp:TextBox ID="txt_fileUpLoad" class="file" runat="server"  /><div class ="fileUpLoadContain"><asp:FileUpload ID="fileUpLoad" name="fileUpLoad"  runat="server"  onchange="return onLoadFile(this);" /></div></div></td>
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
                                <td><asp:TextBox ID="tbDescription" class="tbStyle" runat="server" Height="105px" 
                        TextMode="MultiLine" Width="300px"></asp:TextBox></td>
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
                <table id = "resultTable">
                <thead>
                    <th>Header1</th>
                    <th>Header1</th>
                    <th>Header1</th>
                    <th>Header1</th>
                    <th>Header1</th>
                </thead>
                <tbody>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                     <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                     <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                      <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                      <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                      <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                      <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                      <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                      <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                      <tr>
                        <td></td>
                        <td></td>
                        <td></td>      
                        <td></td>
                        <td></td>
                    </tr>
                </tbody>
                </table>
            </td>
       </tr>
</table>
<div id="skm_LockPane" class="LockOff">
</div>
</form>
</asp:Content>
