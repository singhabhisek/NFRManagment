<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GridView Paging Example</title>
    <%--    <link rel="stylesheet" href="Bootstrap.css" />--%>
    <link rel="stylesheet" href="Ultimate.css" />
    <style type="text/css">
        
    </style>

    <script type="text/javascript">

        function OpenUploadView() {
            window.open("ExcelUpload.aspx", "_blank", "width=320,height=320", true);
        }

        function popupwindow(url, title, w, h) {
            var y = window.outerHeight / 2 + window.screenY - (h / 2)
            var x = window.outerWidth / 2 + window.screenX - (w / 2)
            return window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + y + ', left=' + x);
        }

    </script>
</head>
<body>

    <form id="form1" runat="server">

        <div style="margin-left: 20px">
            <b>Search Records (You can use %, _ as Wildcard in Search Transaction Text)</b>
            <table style="width: 1000px">
                <tr>
                    <td class="auto-style6" align="center">
                        <asp:Label ID="lblApplicationName" Text="Select Application" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:DropDownList class="select-dropdown" ID="ddlApplicationName" runat="server" OnSelectedIndexChanged="ddlApplicationName_SelectedIndexChanged" AutoPostBack="true" CssClass="auto-style4"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlApplicationName" ErrorMessage="*"  ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>

                    <td class="auto-style6" align="center">
                        <asp:Label ID="Label1" Text="Select Release" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:DropDownList class="select-dropdown" ID="ddlReleaseID" runat="server" CssClass="auto-style4"></asp:DropDownList>
                    </td>

                    <td class="auto-style6" align="center">
                        <asp:Label ID="Label2" Text="Search Transaction" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtTransactionName" Width="90%" runat="server" CssClass="auto-style4"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTransactionName" ErrorMessage="Minimum three character required" ValidationExpression="[\s\S]{3,}"></asp:RegularExpressionValidator>
                    </td>

                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn-primary" Text="Search Records" OnClick="btnSubmit_Click" /></td>
                    <td colspan="3" align="left">
                        <asp:Button ID="btnClear" runat="server" Text="Clear Records" OnClick="btnClear_Click" /></td>
                </tr>

                <br />
                <br />

                <tr>
                    <td colspan="1" align="left">
                        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>

                    </td>
                    <td colspan="4" align="center">
                        <asp:ImageButton ID="btnImportExcel" runat="server" ImageUrl="~/Images/test_logo.png" AlternateText="Import Excel to application" Height="28px" OnClientClick="javascript:popupwindow('ExcelUpload.aspx', '_blank', 600, 400)" />
                    </td>
                    <td colspan="1" align="right">
                        <asp:ImageButton ID="btnExportExcel" runat="server" ImageUrl="~/Images/Picture1.png" AlternateText="Export to Excel" Height="28px" OnClick="btnExportExcel_Click" />
                    </td>

                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="Label4" runat="server" Text="# of Records to display" ForeColor="#333333"></asp:Label>
                    <asp:DropDownList CssClass="select-dropdown" ID="ddlGridviewPaging" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="GridView1" Width="1000px" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            OnPageIndexChanging="OnPageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
                            CssClass="table table-striped table-bordered table-hover gvstyling th tr"
                            DataKeyNames="ID" enablepagingandcallback="false" PageSize="5" OnRowDataBound="GridView1_RowDataBound" OnPreRender="GridView1_PreRender" OnDataBound="GridView1_DataBound">
                            <PagerStyle HorizontalAlign="Left" />

                            <PagerSettings Mode="Numeric" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />
                            <Columns>
                                <asp:BoundField HeaderStyle-CssClass="gvstyling" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="applicationName" HeaderText="Application Name">
                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="releaseID" HeaderText="Release Id">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="transactionNames" HeaderText="Transaction Name">
                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="businessScenario" HeaderText="Business Scenario">
                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="SLA" HeaderText="SLA (Sec)">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="TPS" HeaderText="TPS">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="backendCall" HeaderText="Backend Calls">
                                    <ItemStyle HorizontalAlign="Center" Width="150px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="callType" HeaderText="Call Type">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                            
                                 <asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
                            </Columns>

                        </asp:GridView>
                    </td>
                </tr>
            </table>

            <br />
            <br />
            <br />

        </div>

        <%--class="table table-bordered table-condensed table-hover"--%>
        <%--CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows"--%>
    </form>
</body>
</html>

