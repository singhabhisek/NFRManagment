<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Example</title>

    <link rel="stylesheet" href="Resources/css/Ultimate.css" />
    <%-- <link rel="stylesheet" href="Bootstrap.css" />--%>

    <script src="Resources/js/jquery.min.js"></script>
    <link href="Resources/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Resources/js/jquery-ui.min.js"></script>
    

   <%-- <script type="text/javascript">
        $(document).ready(function () {

            SearchText();

        });
        function SearchText() {

            $(".autosuggest").autocomplete({
                minLength: 3,

                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Dashboard.aspx/getTrxNames",
                        data: "{'prefixText':'" + document.getElementById('ContentPlaceHolder1_txtTransactionName').value + "'}",

                        dataType: "json",
                        success: function (data) {
                            //Limit 10 result
                            response(data.d.slice(0, 10));
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });

                }
            });

        }
    </script>--%>

    <style type="text/css">
        .auto-style7 {
            height: 36px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="margin-top: 80px; text-align: left; margin-left: 200px">
       <%-- <h4 style="align-content: center; text-align: center">Search Non-Functional Records for Application</h4>

        <br />--%>

        <fieldset class="box-border">
            <legend class="box-border">Search Records</legend>
            <table>
                <tr>
                    <td colspan="2" style="width: 33%">

                        <table style="width: 100%">
                            <tr>
                                <td style="width: 95%">
                                    <div class="form-floating" style="height: 90%">
                                        <asp:DropDownList CssClass="form-select form-select-sm" ID="ddlApplicationName" runat="server" OnSelectedIndexChanged="ddlApplicationName_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <label for="ddlApplicationName">Select Application</label>
                                    </div>
                                </td>
                                <td style="width: 5%">
                                    <asp:RequiredFieldValidator ForeColor="Red" ID="rfv1" runat="server" ControlToValidate="ddlApplicationName" InitialValue="0" SetFocusOnError="True">*</asp:RequiredFieldValidator>

                                </td>
                            </tr>
                        </table>
                    </td>

                    <td colspan="2" style="width: 33%">
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlReleaseID" runat="server" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <label for="ddlReleaseID">Select Release</label>

                        </div>
                    </td>

                    <td colspan="2" style="width: 34%">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 95%">
                                    <div class="form-floating">
                                        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                                        <asp:TextBox ID="txtTransactionName" Width="90%" Height="14px" runat="server" CssClass="autosuggest form-control"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="getTrxNames"
    MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
    TargetControlID="txtTransactionName" FirstRowSelected="true"  >
</cc1:AutoCompleteExtender>
                                        <label for="txtTransactionName">Search Transaction (Min 3 characters)</label>
                                    </div>

                                </td>
                                <td style="width: 5%">
                    <%--                <asp:RegularExpressionValidator Font-Size="10px" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTransactionName" ErrorMessage="Min three characters" ForeColor="Red" ValidationExpression="^[\s\S]{3,}$"></asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                        </table>

                    </td>

                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="Search Records" OnClick="btnSubmit_Click" /></td>
                    <td colspan="3" align="left">
                        <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary" Text="Clear Records" OnClick="btnClear_Click" /></td>
                    <%--<td colspan="2" align="left" style="align-content: start; text-align: left; margin-left: 0px; margin-top: 0px">
                    <asp:RegularExpressionValidator Font-Size="10px" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTransactionName" ErrorMessage="Min three characters to search" ForeColor="Red" ValidationExpression="^[\s\S]{3,}$"></asp:RegularExpressionValidator>
                </td>--%>
                </tr>

                <tr>
                    <td colspan="6" align="left" style="width: 500px">
                        <asp:Label ID="lblError" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td colspan="5">
                        <!--added on 07/25-->
                        <asp:Label ID="Label1" runat="server" Text="# of Records to display" ForeColor="#333333"></asp:Label>
                        <asp:DropDownList CssClass="select-dropdown" ID="ddlGridviewPaging" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="1" align="right">
                        <asp:ImageButton ID="btnExportExcel0" runat="server" ImageUrl="~/Resources/images/xl_down.png" AlternateText="Export to Excel" Height="40px" Width="40px" OnClick="btnExportExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="GridView1" Width="1000px" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            OnPageIndexChanging="OnPageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
                            CssClass="table table-striped table-bordered table-hover gvstyling tr th"
                            DataKeyNames="ID" enablepagingandcallback="false" PageSize="5" OnRowDataBound="GridView1_RowDataBound" OnPreRender="GridView1_PreRender" OnDataBound="GridView1_DataBound">
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Left" />

                            <PagerSettings Mode="Numeric" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />
                            <Columns>
                                <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="applicationName" HeaderText="Application Name">
                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="releaseID" HeaderText="Release Id">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="businessScenario" HeaderText="Business Scenario">
                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="transactionNames" HeaderText="Transaction Name">
                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="SLA" HeaderText="SLA (Sec)">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="TPS" HeaderText="TPS">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="backendCall" HeaderText="Backend Calls" Visible="false">
                                    <ItemStyle HorizontalAlign="Center" Width="150px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="callType" HeaderText="Call Type" Visible="false">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:CommandField ShowEditButton="True" />
                                <asp:CommandField ShowDeleteButton="True" />
                            </Columns>

                        </asp:GridView>

                    </td>
                </tr>
            </table>

        </fieldset>
        <br />
        <br />

        <br />
    </div>
</asp:Content>

