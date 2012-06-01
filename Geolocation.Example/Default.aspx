<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Geolocation.Example.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        For real world usage, you would obviously accept a zip code input here and use an API such as <a href="https://developers.google.com/maps/documentation/javascript/geocoding">Google's</a> or <a href="http://msdn.microsoft.com/en-us/library/cc966793.aspx">Bing's</a> to get the lat/long for the origin location. Here, I will use 90210 and hard code the lat/long.<br/><br/>
        See <a href="https://github.com/scottschluer/Geocoder">https://github.com/scottschluer/Geocoder</a> for an easy to use Geocoder API for Google.<br/><br/>
        Zip Code: 90210<br/>
        Radius: 
            <asp:DropDownList runat="server" ID="Radius">
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
            </asp:DropDownList><br />
            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
            
            <br/><br/>
            
            <asp:GridView runat="server" ID="gvLocations" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField HeaderText="Name" DataField="Name" />
                    <asp:BoundField HeaderText="Distance" DataField="Distance" />
                    <asp:BoundField HeaderText="Direction" DataField="Direction" />
                </Columns>
            </asp:GridView>
    </div>
    </form>
</body>
</html>
