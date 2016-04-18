<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ServiceConsumer.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="MainForm" runat="server">
        <div id="page_wrapper" runat="server">
            <div id="HeadDiv">
                Select a view: 
                <asp:DropDownList ID="ViewList" runat="server">
                    <asp:ListItem Text="QuickBooks" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Scheduler" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div id="MainDiv" runat="server"></div>
            <div class="loading" align="center">
                <img src="loader.gif" alt="" />
            </div>
            <div id="CsvDiv">
                <asp:Button Text="Download CSV file..." runat="server"/> 
                CSV Preview:<br />     
                <asp:TextBox ID="CSVPreviewBox" runat="server" TextMode="MultiLine" 
                             readonly="true" BorderStyle="Solid" Height="480px" Width="640px" />
            </div>
        </div>
    </form>
</body>
</html>
