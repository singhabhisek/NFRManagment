<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Landing.aspx.cs" Inherits="Landing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>GridView Paging Example</title>
    <link rel="stylesheet" href="Bootstrap.css" />
    <link rel="stylesheet" href="StyleSheet.css" />
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-left: 20px">
        <b>Search Records</b>
        <table style="width: 1000px">
            <tr>
                <td class="auto-style6" align="center">
                    <asp:Label ID="lblApplicationName" Text="Select Application" runat="server"></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:DropDownList class="select-dropdown" ID="ddlApplicationName" runat="server" OnSelectedIndexChanged="ddlApplicationName_SelectedIndexChanged" AutoPostBack="true" CssClass="auto-style4"></asp:DropDownList>
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
                </td>

            </tr>
            <tr>
                <td colspan="3" align="right">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search Records" OnClick="btnSubmit_Click" /></td>
                <td colspan="3" align="left">
                    <asp:Button ID="btnClear" runat="server" Text="Clear Records" /></td>
            </tr>

            <br />
            <br />

            <tr>
                <td colspan="6" align="right">
                    <asp:ImageButton ID="btnExportExcel0" runat="server" ImageUrl="~/Images/test_logo.png" AlternateText="Export to Excel" Height="28px" OnClick="btnExportExcel_Click" />
                </td>

            </tr>
            <tr>
                <td colspan="6"></td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView ID="GridView1" Width="1000px" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        OnPageIndexChanging="OnPageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
                        CssClass="table table-striped table-bordered table-hover"
                        DataKeyNames="ID" enablepagingandcallback="false" PageSize="2" OnRowDataBound="GridView1_RowDataBound">
                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />

                        <PagerSettings Mode="Numeric" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="applicationName" HeaderText="Application Name">
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
                            <%--<asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />--%>
                        </Columns>

                    </asp:GridView>
                </td>
            </tr>
        </table>

        <br />
        <br />
        <br />
    </div>
</asp:Content>

