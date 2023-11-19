<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DashboardWithDiscrepancy - Copy.aspx.cs" Inherits="DashboardWithDiscrepancy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Example</title>


    <%-- <link rel="stylesheet" href="Bootstrap.css" />--%>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.1/css/bootstrap.css" />

    <link rel="stylesheet" href="Resources/css/Ultimate.css" />
    <script src="Resources/js/CommonJS.js" type="text/javascript"></script>
    <script src="Resources/js/jquery.min.js"></script>
    <link href="Resources/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Resources/js/jquery-ui.min.js"></script>

     <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
     <style>
        .chart-container {
            width: 80%;
            margin: auto;
        }
    </style>


    <style type="text/css">
        .verticalSpace {
            margin-top: 15px;
        }
    </style>


    <style>
        .modalPopup {
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            margin-left: -100px;
            padding: 10px;
            width: auto;
            height: auto;
            text-align: left;
        }

        /*#tooltip {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #FEE18D;
            padding: 5px;
            opacity: 0.85;
        }

            #tooltip h3, #tooltip div {
                margin: 0;
            }*/
    </style>


    <style>
        .tooltip12 {
            position: relative;
            display: inline-block;
            cursor: pointer;
        }

            .tooltip12 .tooltiptext12 {
                visibility: visible;
                width: 200px;
                background-color: #555;
                color: #fff;
                text-align: center;
                border-radius: 6px;
                padding: 5px;
                position: absolute;
                z-index: 1;
                bottom: 125%;
                left: 50%;
                margin-left: -200px;
                opacity: 0;
                transition: opacity 0.2s;
            }

            .tooltip12:hover .tooltiptext12 {
                visibility: visible;
                opacity: 1;
            }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="div1" style="height: calc(100vh-20px); overflow-y: hidden; display: flex;">

        <div style="margin-top: 80px; text-align: left; margin-left: 150px; overflow-y: auto">
            <%-- <h4 style="align-content: center; text-align: center">Search Non-Functional Records for Application</h4>

        <br />--%>

            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <%--overflow-y: auto--%>
                    <fieldset class="box-border" style="width: 100%; margin-left: 10px;">
                        <legend class="box-border">Search Records</legend>

                        <div class="form-floating form-control-position" style="margin-left: 35px;">
                            <asp:DropDownList CssClass="form-select form-select-sm" ID="ddlApplicationName" runat="server" OnSelectedIndexChanged="ddlApplicationName_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <label for="ddlApplicationName">Select Application</label>
                        </div>

                        <div class="form-floating form-control-position">
                            <asp:DropDownList ID="ddlReleaseID" runat="server" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <label for="ddlReleaseID">Select Release</label>
                        </div>

                        <div class="form-floating form-control-position">
                            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                            <asp:TextBox ID="txtTransactionName" Width="90%" Height="14px" runat="server" CssClass="autosuggest form-control"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="getTrxNames"
                                MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                TargetControlID="txtTransactionName" FirstRowSelected="true">
                            </cc1:AutoCompleteExtender>
                            <label for="txtTransactionName">Search Transaction (Min 3 characters)</label>
                        </div>
                        <br />

                        <table style="margin-top: 10px; width: 100%;">
                            <tr>
                                <td colspan="3" align="right" style="width: 50%">
                                    <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="Search Records" OnClick="btnSubmit_Click" /></td>
                                <td colspan="3" align="left" style="width: 50%">
                                    <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary" Text="Clear Records" OnClick="btnClear_Click" /></td>
                                <%--<td colspan="2" align="left" style="align-content: start; text-align: left; margin-left: 0px; margin-top: 0px">
                    <asp:RegularExpressionValidator Font-Size="10px" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTransactionName" ErrorMessage="Min three characters to search" ForeColor="Red" ValidationExpression="^[\s\S]{3,}$"></asp:RegularExpressionValidator>
                </td>--%>
                            </tr>

                            <tr>
                                <td colspan="3" align="left" style="width: 800px;">
                                    <asp:Label ID="lblError" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                                </td>

                                <td colspan="1" align="right">
                                    <asp:LinkButton ID="Button1" runat="server" Text="Recent Searches" />
                                    <!-- ModalPopupExtender -->
                                    <cc1:ModalPopupExtender ID="mp1" runat="server" BehaviorID="mpe" PopupControlID="Panl1" TargetControlID="Button1"
                                        CancelControlID="Button2" BackgroundCssClass="Background">
                                    </cc1:ModalPopupExtender>
                                </td>

                                <td colspan="1" align="right">
                                    <!--added on 07/25-->
                                    <asp:Label ID="Label1" runat="server" Text="# of Records to display" ForeColor="#333333"></asp:Label>
                                    <asp:DropDownList CssClass="select-dropdown" ID="ddlGridviewPaging" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="1" align="right">
                                    <%--                                    <asp:ImageButton ID="btnExportExcel0" runat="server" ImageUrl="~/Resources/images/xl_down.png" AlternateText="Export to Excel" Height="30px" Width="40px" OnClick="btnExportExcel_Click" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView1" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                        OnPageIndexChanging="OnPageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
                                        CssClass="table table-striped table-bordered table-hover gvstyling tr th"
                                        enablepagingandcallback="false" PageSize="5" OnRowDataBound="GridView1_RowDataBound" OnPreRender="GridView1_PreRender" OnDataBound="GridView1_DataBound">
                                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Left" />

                                        <PagerSettings Mode="Numeric" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="ApplicationName" HeaderText="Application Name">
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="releaseID" HeaderText="Release Id">
                                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            </asp:BoundField>

                                            <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="transactionName" HeaderText="Transaction Name">
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>

                                            <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="SLA" HeaderText="SLA (Sec)">
                                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            </asp:BoundField>

                                            <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="TPS" HeaderText="TPH/TPS">
    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
</asp:BoundField>

                                            <%--<asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="TotalSyncSLA" HeaderText="TotalSyncSLA">
                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="MaxAsyncSLA" HeaderText="MaxAsyncSLA" >
                        <ItemStyle HorizontalAlign="Center" Width="150px"></ItemStyle>
                    </asp:BoundField>--%>

                                            <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="backendCall" HeaderText="Backend Call" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="Compare" HeaderText="Compare" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Comparison">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgStatus" ToolTip="Shows high/low for comaprison" Height="18px" Width="18px" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="100px"  HeaderText="Details">
                                                <ItemTemplate>
                                                    <div class="tooltip12">
                                                        <img src="Resources/images/info.png" alt="Info" width="20" height="20" />
                                                        <div class="tooltiptext12">
                                                            <%# Eval("backendCall") %>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Panel ID="Panel1" runat="server">

                                                        <asp:Image ID="Label4" runat="server" ImageUrl="~/Resources/images/user-solid.svg" Width="15px" Height="15px"></asp:Image>
                                                    </asp:Panel>
                                                    <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="PopupMenu"
                                                        TargetControlID="Panel1" PopupPosition="Bottom">
                                                    </cc1:HoverMenuExtender>
                                                    <asp:Panel ID="PopupMenu" runat="server" CssClass="modalPopup">
                                                        <asp:Label ID="Label1" runat="server" Style="align-text: left" Text='<%# Eval("backendCall") %>'></asp:Label>
                                                        <br />
                                                        <hr />

                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>


                                        </Columns>


                                    </asp:GridView>

                                </td>
                            </tr>
                        </table>

                    </fieldset>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none">

                <fieldset class="box-border" style="margin-bottom: 0px; vertical-align: bottom; width: 90%; height: 150px; overflow-y: auto">
                    <legend class="box-border">Last 5 Searches By You</legend>
                    <div>
                        <asp:PlaceHolder ID="phLinks" runat="server"></asp:PlaceHolder>
                    </div>
                </fieldset>
                <asp:Button ID="Button2" Style="margin-top: 15px;" CssClass="btn btn-secondary" runat="server" Text="Close" />
            </asp:Panel>
        </div>
    </div>

     <div class="chart-container">
        <canvas id="slaTpsChart"></canvas>
    </div>

     <script>
        // Dummy data for testing
        var chartData = {
            labels: ["Release1", "Release2", "Release3", "Release4", "Release5"],
            datasets: [
                {
                    label: 'SLA',
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 2,
                    fill: false,
                    data: [2, 3, 1.5, 2.8, 2.2]
                },
                {
                    label: 'TPS',
                    borderColor: 'rgba(255, 99, 132, 1)',
                    borderWidth: 2,
                    fill: false,
                    data: [100, 120, 90, 110, 105]
                }
            ]
        };

        // Create chart
        var ctx = document.getElementById('slaTpsChart').getContext('2d');
        var myChart = new Chart(ctx, {
            type: 'line',
            data: chartData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'Release ID'
                        }
                    }],
                    y: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'Values'
                        }
                    }]
                }
            }
        });
     </script>

</asp:Content>

