<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="emotionDB.aspx.cs" Inherits="FYP_ChildAbuseGIS2013._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Populate DataBase</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <table>
        <tr>
            <td><asp:Label ID="lb_file" runat="server" Text="File :"></asp:Label></td>
            <td>
                <asp:FileUpload ID="imageDir" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lb_id" runat="server" Text="Name Image ID :"></asp:Label></td>
            <td><asp:TextBox ID="imageID" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="lb_expression" runat="server" Text="Name Image Expression :"></asp:Label></td>
            <td><asp:TextBox ID="imageExpression" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnInsert" runat="server" Text="Insert" 
                    onclick="btnInsert_Click" />
            </td>
            <td>
                <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    
    </table>
    <asp:GridView ID="dbView" runat="server"
            ForeColor="Black" 
            BackColor="AliceBlue"  
            Font-Bold="false"  
            Font-Italic="true"    
            BorderColor="CornflowerBlue"   
            Font-Names = "Calibri"          
             >  
            <HeaderStyle   
                ForeColor="Snow"  
                BackColor="Orange"  
                Font-Bold="true"  
                />  
    </asp:GridView>
    
    
    </form>
</body>
</html>
