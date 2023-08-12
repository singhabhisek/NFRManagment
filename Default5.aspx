<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default5.aspx.cs" Inherits="Default5" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="Resources/css/Ultimate.css" />

    <script src="https://code.jquery.com/jquery-3.6.0.js"></script>
  <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.js"></script>
<script type="text/javascript">
    function InitializeToolTip() {
        $(".gridViewToolTip").tooltip({
            track: true,
            delay: 0,
            showURL: false,
            fade: 100,
            bodyHandler: function () {
                return $($(this).next().html());
            },
            showURL: false
        });
    }
</script>

<script type="text/javascript">
    $(function () {
        InitializeToolTip();
    }
    
</script>
    <style>
          .modalPopup
    {
        background-color: #FFFFFF;
        border-width: 3px;
        border-style: solid;
        border-color: black;
        margin: 10px;
        padding: 10px;
        width: auto;
        height: auto;
    }
          #tooltip {
position: absolute;
z-index: 3000;
border: 1px solid #111;
background-color: #FEE18D;
padding: 5px;
opacity: 0.85;
}
#tooltip h3, #tooltip div { margin: 0; }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-top: 200px; margin-left: 150px;">
        <div class="form-floating form-control-position" style="margin-left: 35px;">
            <asp:DropDownList CssClass="form-select form-select-sm" ID="ddlApplicationName" runat="server" OnSelectedIndexChanged="ddlApplicationName_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            <label for="ddlApplicationName">Select Application</label>
        </div>

        <div class="form-floating form-control-position">
            <asp:DropDownList ID="ddlReleaseID" runat="server" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            <label for="ddlReleaseID">Select Release</label>
        </div>

        <div style="margin-top: 100px; margin-left: 100px">
             <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                           
            <asp:GridView ID="Gridview1" CssClass="table table-striped table-bordered table-hover gvstyling tr th"
                ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="False" AllowPaging="True" OnRowDataBound="Gridview1_RowDataBound"
                OnPreRender="GridView1_PreRender" OnDataBound="GridView1_DataBound">


                <PagerStyle CssClass="pagination-ys" HorizontalAlign="Left" />

                <PagerSettings Mode="Numeric" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />
                <Columns>
                    <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="ApplicationName" HeaderText="Application Name">
                        <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="releaseID" HeaderText="Release Id">
                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="transactionNames" HeaderText="Transaction Name">
                        <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="SLA" HeaderText="SLA (Sec)">
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
                    <asp:TemplateField HeaderText="Comparison">
                        <ItemTemplate>
                            <asp:Image ID="imgStatus" Height="18px" Width="18px" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                <ItemTemplate>
                    <asp:Panel ID="Panel1" runat="server">
                        <asp:Image ID="Label4" runat="server" ImageUrl="~/Resources/images/user-solid.svg" Width="15px" Height="15px"></asp:Image>
                    </asp:Panel>
                    <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="PopupMenu"
                        TargetControlID="Panel1" PopupPosition="Bottom">
                    </cc1:HoverMenuExtender>
                    <asp:Panel ID="PopupMenu" runat="server" CssClass="modalPopup">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("backendCall") %>'></asp:Label>
                        <br />
                        <hr />
                        
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>


                    <asp:TemplateField HeaderText="UserId">
<ItemStyle Width="30px" HorizontalAlign="Center" />
<ItemTemplate>
    <a href="#" class="gridViewToolTip"><%# Eval("SLA")%></a>

<div id="tooltip" style="display: none;">
<table>
<tr>
<td style="white-space: nowrap;"><b>UserName:</b>&nbsp;</td>
<td><%# Eval("backendCall")%></td>
</tr>

</table>
</div>
</ItemTemplate>
</asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>
    </div>
</asp:Content>

