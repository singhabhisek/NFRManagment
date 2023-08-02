<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserAdministration.aspx.cs" Inherits="UserAdministration" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Resources/js/CommonJS.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-top: 80px; text-align: left; margin-left: 200px">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>



        <div style="margin-left: 300px">

            <h4 style="align-content: center; text-align: center">User Access Role List</h4>

            <br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound"
                        CssClass="table table-striped table-bordered table-hover gvstyling tr th"
                        DataKeyNames="UserID" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" PageSize="15" AllowPaging="True" OnPageIndexChanging="OnPaging"
                        OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No records has been added."
                        Width="653px" CellPadding="4" ForeColor="#333333" GridLines="None" OnDataBound="GridView1_DataBound" OnPreRender="GridView1_PreRender">
                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Left" />

                        <Columns>
                            <asp:TemplateField HeaderText="User Id" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUserId" runat="server" Text='<%# Eval("UserID") %>' Width="140"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemStyle Width="150px" />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Role">
                                <ItemTemplate>
                                    <asp:Label ID="lblRoles" runat="server" Text='<%# Eval("Roles")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlRoles" runat="server">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>



                            <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true"
                                ItemStyle-Width="150">
                                <ItemStyle Width="150px" />
                            </asp:CommandField>
                        </Columns>

                    </asp:GridView>

                    <br />


                    <fieldset class="box-border">
                        <legend class="box-border">Add User Access</legend>
                        <table>

                            <tr style="font-size: 11px">

                                <td style="text-align: right">User ID:</td>
                                <td>
                                    <asp:TextBox ID="txtUserID" class="select-dropdown" runat="server" Width="140" />
                                </td>
                                <td style="text-align: right">Role Name:</td>
                                <td>
                                    <asp:DropDownList ID="ddlRoles" class="select-dropdown" runat="server"></asp:DropDownList>
                                </td>

                                <td style="width: 150px; text-align: right; margin-left: 10px">
                                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Assign Role" OnClick="Insert" OnClientClick="return confirm('Are you sure want to insert this user?');" />
                                </td>
                            </tr>
                        </table>

                    </fieldset>



                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

