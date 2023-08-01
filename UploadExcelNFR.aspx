<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UploadExcelNFR.aspx.cs" Inherits="UploadExcelNFR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link rel="stylesheet" href="Resources/css/Ultimate.css" />
    <script type="text/javascript" src="Resources/js/jquery.min.js"></script>

    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <style type="text/css">
       
    </style>
    <style type="text/css">
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
</style>

    <script type="text/javascript">
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }
    $('form').live("submit", function () {
        ShowProgress();
    });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="margin-left: 200px">
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

