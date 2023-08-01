<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/MasterPage.master" CodeFile="CompareSLA.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Example</title>
    <link rel="stylesheet" href="Resources/css/Ultimate.css" />
    <%-- <link rel="stylesheet" href="Bootstrap.css" />--%>

     <script src="Resources/js/jquery.min.js"></script>
    <link href="Resources/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Resources/js/jquery-ui.min.js"></script>

    <script type="text/javascript">
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
                        url: "CompareSLA.aspx/getTrxNames",
                        data: "{'prefixText':'" + document.getElementById('ContentPlaceHolder1_txtTransactionName').value + "'}",

                        dataType: "json",
                        success: function (data) {
                            response(data.d.slice(0, 10));
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });

                }
            });

        }
    </script>

    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="margin-top: 80px; text-align: left; margin-left: 200px">
        <b>Search Records</b>
        <table style="width: 1000px">
            <tr>

                <td colspan="2" style="width: 33%;padding: 0px;">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 98%">
                                <div class="form-floating" style="height: 90%">
                                    <asp:DropDownList CssClass="form-select form-select-sm" ID="ddlApplicationName" runat="server" OnSelectedIndexChanged="ddlApplicationName_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <label for="ddlApplicationName">Select Application</label>
                                </div>
                            </td>
                            <td style="width: 2%">
                                <asp:RequiredFieldValidator ForeColor="Red" ID="rfv1" runat="server" ControlToValidate="ddlApplicationName" InitialValue="0" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>

                <td colspan="2" style="width: 33%">
                    <div class="form-floating">
                        <asp:DropDownList ID="ddlReleaseID" runat="server" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="false"></asp:DropDownList>
                        <label for="ddlReleaseID">Select Release - 1</label>

                    </div>
                </td>

                <td colspan="2" style="width: 34%">
                    <div class="form-floating">
                        <asp:TextBox ID="txtTransactionName" Width="90%" Height="14px" runat="server" CssClass="autosuggest form-control"></asp:TextBox>
                        <label for="txtTransactionName">Search Transaction</label>
                    </div>
                    <%-- <table style="width: 100%">
                        <tr>
                            <td style="width: 95%">
                                <div class="form-floating">
                                    <asp:TextBox ID="txtTransactionName" Height="1px" runat="server" CssClass="form-control"></asp:TextBox>
                                    <label for="txtTransactionName">Select Transaction</label>

                                </div>
                            </td>
                            <td style="width: 5%">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTransactionName" ErrorMessage="Min three charcters required" ValidationExpression="^[\s\S]{3,}$"></asp:RegularExpressionValidator>

                            </td>
                        </tr>
                    </table> --%>
                </td>

            </tr>
            <tr>
                <%--<td class="auto-style6" align="center">
                    <asp:Label ID="Label3" Text="Select Release - 2" runat="server"></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:DropDownList class="select-dropdown" ID="ddlReleaseID1" runat="server" CssClass="auto-style4" OnSelectedIndexChanged="ddlReleaseID1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </td>--%>

                <td colspan="2" style="width: 33%">
                    <div class="form-floating" style="width: 98%">
                        <asp:DropDownList ID="ddlReleaseID1" Width="98%" runat="server" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="false"></asp:DropDownList>
                        <label for="ddlReleaseID1">Select Release - 2</label>

                    </div>
                </td>

               <%-- <td class="auto-style6" align="center">
                    <asp:Label ID="Label4" Text="Select Release -3" runat="server"></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:DropDownList class="select-dropdown" ID="ddlReleaseID2" runat="server" CssClass="auto-style4"></asp:DropDownList>
                </td>--%>

                <td colspan="2" style="width: 33%">
                    <div class="form-floating">
                        <asp:DropDownList ID="ddlReleaseID2" runat="server" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="false"></asp:DropDownList>
                        <label for="ddlReleaseID2">Select Release - 3</label>

                    </div>
                </td>

                <td class="auto-style6" align="center">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>

            </tr>
            <tr>
                <td colspan="3" align="right">
                    <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="Search Records" OnClick="btnSubmit_Click" /></td>
                <td colspan="3" align="left">
                    <asp:Button ID="btnClear" CssClass="btn btn-secondary" runat="server" Text="Clear Records" OnClick="btnClear_Click" /></td>
            </tr>

            <tr>
                <td colspan="6" style="color: #FF0000">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td colspan="5" align="left">
                    <asp:Label ID="Label5" runat="server" Text="# of Records to display" ForeColor="#333333"></asp:Label>
                    <asp:DropDownList CssClass="select-dropdown" ID="ddlGridviewPaging" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="1" align="right">
                    <asp:ImageButton ID="btnExportExcel" runat="server" ImageUrl="~/Resources/images/xl_down.png" AlternateText="Export to Excel" Width="40px" Height="40px" OnClick="btnExportExcel_Click" />
                </td>

            </tr>

            <tr>
                <td colspan="6" >
                    <asp:GridView ID="GridView1" Width="1000px" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="True" AllowPaging="True"
                        OnPageIndexChanging="OnPageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
                        CssClass="table table-striped table-bordered table-hover gvstyling tr th" OnDataBound="GridView1_DataBound"
                        enablepagingandcallback="false" PageSize="10" OnRowDataBound="GridView1_RowDataBound" OnPreRender="GridView1_PreRender">
                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Left" />

                        <PagerSettings Mode="Numeric" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />

                    </asp:GridView>
                </td>
            </tr>
        </table>

        <br />
        <br />
        <br />

    </div>

    <%--<asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />--%><%--<asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />--%>
</asp:Content>

