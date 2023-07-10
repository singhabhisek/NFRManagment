<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="CompareSLA.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GridView Paging Example</title>
    <link rel="stylesheet" href="Bootstrap.css" />
    <link rel="stylesheet" href="StyleSheet.css" />
    <style type="text/css">
        
    </style>

    <script type="text/javascript">



</script>
</head>
<body>

    <form id="form1" runat="server">

        <div style="margin-left: 20px">
            <b>Search Records</b>
            <table style="width: 1000px">
                <tr>
                    <td class="auto-style6" align="center">
                        <asp:Label ID="lblApplicationName" Text="Select Application" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:DropDownList class="select-dropdown" ID="ddlApplicationName" runat="server" OnSelectedIndexChanged="ddlApplicationName_SelectedIndexChanged" AutoPostBack="true" CssClass="auto-style4"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" InitialValue="0" runat="server" ForeColor="Red"  ControlToValidate="ddlApplicationName" ErrorMessage="*" ></asp:RequiredFieldValidator>
                    </td>

                    <td class="auto-style6" align="center">
                        <asp:Label ID="Label1" Text="Select Release" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:DropDownList class="select-dropdown" ID="ddlReleaseID" runat="server" CssClass="auto-style4" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>

                    <td class="auto-style6" align="center">
                        <asp:Label ID="Label2" Text="Search Transaction" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtTransactionName" Width="90%" runat="server" CssClass="auto-style4"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td class="auto-style6" align="center">
                        <asp:Label ID="Label3" Text="Select Release - 2" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:DropDownList class="select-dropdown" ID="ddlReleaseID1" runat="server" CssClass="auto-style4" OnSelectedIndexChanged="ddlReleaseID1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>

                    <td class="auto-style6" align="center">
                        <asp:Label ID="Label4" Text="Select Release -3" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:DropDownList class="select-dropdown" ID="ddlReleaseID2" runat="server" CssClass="auto-style4"></asp:DropDownList>
                    </td>

                    <td class="auto-style6" align="center">&nbsp;</td>
                    <td class="auto-style1">&nbsp;</td>

                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:Button ID="btnSubmit" runat="server" Text="Search Records" OnClick="btnSubmit_Click" /></td>
                    <td colspan="3" align="left">
                        <asp:Button ID="btnClear" runat="server" Text="Clear Records" OnClick="btnClear_Click" /></td>
                </tr>

                <br />
                <br />

                <tr>
                    <td colspan="5" align="right"></td>
                    <td colspan="1" align="right">
                        <asp:ImageButton ID="btnExportExcel" runat="server" ImageUrl="~/Images/test_logo.png" AlternateText="Export to Excel" Height="28px" OnClick="btnExportExcel_Click" />
                    </td>

                </tr>
                <tr>
                    <td colspan="6" style="color: #FF0000">
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="GridView1" Width="1000px" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="True" AllowPaging="True"
                            OnPageIndexChanging="OnPageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
                            CssClass="table table-striped table-bordered table-hover"
                            enablepagingandcallback="false" PageSize="2" OnRowDataBound="GridView1_RowDataBound" OnPreRender="GridView1_PreRender">
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />

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
    </form>
</body>
</html>

