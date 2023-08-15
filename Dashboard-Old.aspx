<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard-Old.aspx.cs" Inherits="Dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Example</title>

  
    <%-- <link rel="stylesheet" href="Bootstrap.css" />--%>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous">
     <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.1/css/bootstrap.css"/>
      <link rel="stylesheet" href="Resources/css/Ultimate.css" />
    <script src="Resources/js/CommonJS.js" type="text/javascript"></script>
    <script src="Resources/js/jquery.min.js"></script>
    <link href="Resources/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Resources/js/jquery-ui.min.js"></script>
    <style type="text/css">
        .verticalSpace{
            margin-top: 15px;
        }
    </style>

    <script type="text/javascript">
        //function pageLoad() {
        //    var modalPopup = $find('mpe');
        //    modalPopup.add_shown(function () {
        //        modalPopup._backgroundElement.addEventListener("click", function () {
        //            modalPopup.hide();
        //        });
        //    });
        //};
    </script>
    <style type="text/css">
      
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="div1" style="margin-left:10px;">

        <div<%-- style="margin-top: 80px; text-align: left; margin-left: 150px;"--%>>
            <%-- <h4 style="align-content: center; text-align: center">Search Non-Functional Records for Application</h4>

        <br />--%>

            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                     <%--overflow-y: auto--%>
                    <fieldset class="box-border" style="width:100%; height:300px;overflow-y: auto;">
                        <legend  class="box-border">Search Records</legend>

                        <div class="form-floating form-control-position">
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
                        
                        <table style="margin-top: 10px;width:90%;">
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
                                    <asp:ImageButton ID="btnExportExcel0" runat="server" ImageUrl="~/Resources/images/xl_down.png" AlternateText="Export to Excel" Height="40px" Width="40px" OnClick="btnExportExcel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView1" Width="1050px" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="False" AllowPaging="True"
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
                                            <asp:BoundField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="transactionName" HeaderText="Transaction Name">
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
                                            <asp:CommandField ShowEditButton="True" Visible="true" HeaderText="Edit" />


                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton  ID="lnkDelete" CommandArgument='<%# Eval("ID") %>' OnClick="DeleteRecord" runat="server" Text="Delete"></asp:LinkButton>
                                                    <cc1:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="lnkDelete"></cc1:ConfirmButtonExtender>
                                                    <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkDelete" OkControlID="btnYes"
                                                        CancelControlID="btnNo" BackgroundCssClass="modalBackground">
                                                    </cc1:ModalPopupExtender>
                                                    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                                                        <div class="header">
                                                            Confirmation
                                                        </div>
                                                        <div class="body">
                                                            Do you want to delete this record?
                                                        </div>
                                                        <div class="footer" align="center">
                                                            <asp:Button ID="btnYes" class="btn btn-primary btn-xs" runat="server" Text="Yes" />
                                                            <asp:Button ID="btnNo" class="btn btn-primary btn-xs" runat="server" Text="No" />
                                                        </div>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>

                                </td>
                            </tr>
                        </table>
                    </fieldset>
                   

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel ID="Panl1"  runat="server" CssClass="Popup" align="center" Style="display: none">

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
</asp:Content>

