<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="Contacts - Copy.aspx.cs" Inherits="sample" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

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

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>NFR Chart</title>
    <style>
        .chart-container {
            width: 80%;
            margin: auto;
            margin-top:200px;
        }
    </style>
        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

</head>
<body>
    
        <div class="chart-container">
            <asp:Chart ID="Chart1" runat="server" Width="500" Height="300">
                <Titles>
                    <asp:Title Text="NFR Trends" />
                </Titles>
                <Legends>
                    <asp:Legend Name="Legend1" Title="Transaction Names" />
                </Legends>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
  


     <div>
            <canvas id="myChart" width="400" height="200"></canvas>
        </div>


</body>
</html>



    </asp:Content>