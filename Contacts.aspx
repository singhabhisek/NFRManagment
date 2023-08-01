<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="Contacts.aspx.cs" Inherits="sample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="bootstrap_min.css" />
</head>
<body>

    <form id="form1" runat="server">

        <br />
        <br />

        <div class="form-floating">
            <asp:DropDownList CssClass="form-select" ID="floatingSelect" runat="server">
                <asp:ListItem Text="Test1" Value="Test1"></asp:ListItem>
                <asp:ListItem Text="Test2" Value="Test2"></asp:ListItem>
            </asp:DropDownList>

            <label for="floatingSelect">Select Environment</label>
        </div>

        <br />
        <br />

        <div class="form-floating">
            <asp:TextBox CssClass="form-control" ID="txt1" runat="server">
               
            </asp:TextBox>

            <label for="txt1">Select Log</label>
        </div>
        <asp:Button CssClass="btn btn-primary" ID="aa" runat="server" Text="Submit Value" OnClick="aa_Click" />
        <asp:Button CssClass="btn btn-warning" ID="Button1" runat="server" Text="Submit Value" OnClick="aa_Click" />
        <asp:Button CssClass="btn btn-danger" ID="Button2" runat="server" Text="Submit Value" OnClick="aa_Click" />
        <br />
        <br />
        <div>
            <div style="width: 200px; background-color: #00FFFF;">
                <table>
                    <tr>
                        <td>
                            <span title="Application">Application Name</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px; background-color: #00FFFF;">
                            <asp:DropDownList ID="ddl" runat="server" Width="100%">
                                <asp:ListItem >Test1</asp:ListItem>
                                <asp:ListItem>Test2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

                <asp:Label ID="lbl" runat="server"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
