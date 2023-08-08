<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default4.aspx.cs" Inherits="Default4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" href="Ultimate.css" />
</head>

<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="scr1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>

                <asp:DropDownList ID="ddlApplicationName" runat="server">
                    <asp:ListItem Text="Select Text" Value="Select Text"></asp:ListItem>
                    <asp:ListItem Text="Text11" Value="Text11"></asp:ListItem>
                    <asp:ListItem Text="Text12" Value="Text12"></asp:ListItem>
                    <asp:ListItem Text="Text13" Value="Text13"></asp:ListItem>
                </asp:DropDownList>

                <asp:DropDownList ID="ddlReleaseID" runat="server">
                     <asp:ListItem Text="Select Text" Value="Select Text"></asp:ListItem>
                    <asp:ListItem Text="Text21" Value="Text21"></asp:ListItem>
                    <asp:ListItem Text="Text22" Value="Text22"></asp:ListItem>
                    <asp:ListItem Text="Text23" Value="Text23"></asp:ListItem>
                </asp:DropDownList>


                <asp:TextBox ID="txtTransactionName" runat="server"></asp:TextBox>

                <asp:Button ID="save" runat="server" Text="Save Cookie" OnClick="save_Click" />

                <asp:Button ID="retrieve" runat="server" Text="Retrieve Cookie" OnClick="retrieve_Click" />

                <asp:Button ID="delete" runat="server" Text="Clear Cookie" OnClick="delete_Click" />
                <asp:Label ID="ss" runat="server" Text="hallo"></asp:Label>
                <div style="margin-bottom: 500px; margin-top: 500px">
                    <asp:PlaceHolder ID="phLinks" runat="server"></asp:PlaceHolder>
                    <asp:ListBox ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"></asp:ListBox>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
