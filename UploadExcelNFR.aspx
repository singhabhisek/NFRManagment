<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UploadExcelNFR.aspx.cs" Inherits="UploadExcelNFR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link rel="stylesheet" href="Resources/css/Ultimate.css" />
    <script type="text/javascript" src="Resources/js/jquery.min.js"></script>
    <script src="Resources/js/CommonJS.js" type="text/javascript"></script>

    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    
    <script type="text/javascript">

       
        $('form').live("submit", function () {
            ShowProgress();
        });
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="margin-top: 80px; text-align: left; margin-left: 200px">
        <h3 style="align-content: center; text-align: center">File Upload Screen</h3>

        <br />
        <br />
        <table style="margin-left: 20px">
            <tr>
                <td colspan="2">
                    <div>Please upload the NFR updated template to store in the database. You can also download the sample NFR Template from <a href="Resources/sample/SampleUpload.xlsx">here</a>. Post upload, please goto <a href="Dashboard.aspx">Dashboard</a> screen to view uploaded data</div>
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

                    <asp:Button CssClass="btn btn-primary" Style="margin-left: 20px" Height="40px" Text="Upload" ID="btnSubmit" OnClick="Upload" runat="server" />

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
    </div>

    <div class="loading" align="center">
    Loading. Please wait.<br />
    <br />
    <img src="Resources/images/loader.gif" height="30" width="30" alt="" />
</div>

</asp:Content>

