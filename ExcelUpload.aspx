<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcelUpload.aspx.cs" Inherits="ExcelUpload" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Bootstrap.css" />
    <style type="text/css">
        /* file upload button */
        input[type="file"]::file-selector-button {
            border-radius: 4px;
            padding: 0 16px;
            height: 40px;
            cursor: pointer;
            background-color: lightgray;
            border: 1px solid rgba(0, 0, 0, 0.16);
            box-shadow: 0px 1px 0px rgba(0, 0, 0, 0.05);
            margin-right: 16px;
            transition: background-color 200ms;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <h3 style="align-content:center;text-align:center">File Upload Screen</h3>

        <br />
        <br />
        <table style="margin-left: 20px">
            <tr>
                <td colspan="2">
                    <div>Please upload the NFR updated template to store in the database. You can also download the sample NFR Template from <a href="Landing.aspx">here</a></div>
                    <br />
                </td>
            </tr>
            <tr>
                <td width="70%">
                    <p style="background-color: antiquewhite; border-radius: 15px; padding: 7px;">
                        <asp:FileUpload Width="100%" ID="FileUpload1" runat="server" accept=".xls,.xlsx" EnableTheming="False" />
                    </p>
                </td>
                <td width="30%">

                    <asp:Button CssClass="btn btn-labeled btn-success" Style="margin-left: 20px"  Height="40px" Text="Upload" OnClick="Upload" runat="server" />

                </td>
            </tr>

            <tr>
                <td colspan="2">

                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <panel>
                        <b>Logs :</b><br />
                    </panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="FileUpload_Msg" runat="server" Text="" Font-Names="Courier New"></asp:Label>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
