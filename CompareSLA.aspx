<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/MasterPage.master" CodeFile="CompareSLA.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Example</title>
    <link rel="stylesheet" href="Resources/css/Ultimate.css" />
     <%--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous">
     <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.1/css/bootstrap.css"/>
    --%>
    <%-- <link rel="stylesheet" href="Bootstrap.css" />--%>
    <script type="text/javascript" src="Resources/js/CommonJS.js"></script>
    <script src="Resources/js/jquery.min.js"></script>
    <link href="Resources/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Resources/js/jquery-ui.min.js"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="div1" style="height: 500px; position: relative;">

        <div style="margin-top: 80px; text-align: left; margin-left: 170px">

            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <fieldset class="box-border" style="width: 90%; height: 600px; overflow-y: auto">
                        <legend class="box-border">Compare Records</legend>

                        <table style="width: 900px">
                            <tr>

                                <td colspan="2" style="width: 33%; padding: 0px;">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 98%">
                                                <div class="form-floating" style="height: 90%">
                                                    <asp:DropDownList CssClass="form-select form-select-sm" ID="ddlApplicationName" runat="server" OnSelectedIndexChanged="ddlApplicationName_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    <label for="ddlApplicationName">Select Application</label>
                                                </div>
                                            </td>
                                            <td style="width: 2%">
                                                <%--<asp:RequiredFieldValidator ForeColor="Red" ID="rfv1" runat="server" ControlToValidate="ddlApplicationName" InitialValue="0" SetFocusOnError="false">*</asp:RequiredFieldValidator>--%>
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

                                        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>


                                        <asp:TextBox ID="txtTransactionName" Width="90%" Height="14px" runat="server" CssClass="autosuggest form-control"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="getTrxNames"
                                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                            TargetControlID="txtTransactionName" FirstRowSelected="true">
                                        </cc1:AutoCompleteExtender>
                                        <label for="txtTransactionName">Search Transaction</label>
                                    </div>

                                </td>

                            </tr>
                            <tr>


                                <td colspan="2" style="width: 33%">
                                    <div class="form-floating" style="width: 98%">
                                        <asp:DropDownList ID="ddlReleaseID1" Width="98%" runat="server" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="false"></asp:DropDownList>
                                        <label for="ddlReleaseID1">Select Release - 2</label>

                                    </div>
                                </td>



                                <td colspan="2" style="width: 33%">
                                    <div class="form-floating">
                                        <asp:DropDownList ID="ddlReleaseID2" runat="server" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlReleaseID_SelectedIndexChanged" AutoPostBack="false"></asp:DropDownList>
                                        <label for="ddlReleaseID2">Select Release - 3</label>

                                    </div>
                                </td>

                                <td colspan="2" style="width: 34%"></td>

                            </tr>
                            </table>
                        <table style="width:100%;">
                            <tr>
                                <td  style="width:50%;" colspan="4" align="right">
                                    <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="Search Records" OnClick="btnSubmit_Click" /></td>
                                <td  style="width:50%;" colspan="4" align="left">
                                    <asp:Button ID="btnClear" CssClass="btn btn-secondary" runat="server" Text="Clear Records" OnClick="btnClear_Click" /></td>
                            </tr>

                            <tr>
                                <td colspan="5" style="color: #FF0000">
                                    <asp:Label ID="lblError" runat="server"></asp:Label>
                                </td>

                                <td colspan="1" align="right">
                                    <asp:LinkButton ID="lnkkFake" runat="server" style="display:none;"></asp:LinkButton>
                                    <asp:LinkButton ID="Button1" ClientIDMode="Static" UseSubmitBehavior=false CausesValidation="false" runat="server"  Text="Recent Searches" OnClick="Button1_Click" />
                                    <!-- ModalPopupExtender -->
                                    <cc1:ModalPopupExtender ID="mp1" BehaviorID="mpe" runat="server" PopupControlID="Panl1" TargetControlID="lnkkFake"
                                        CancelControlID="Button2" BackgroundCssClass="Background">
                                    </cc1:ModalPopupExtender>
                                </td>

                                <td colspan="1" align="right">
                                    <asp:Label ID="Label5" runat="server" Text="# of Records to display" ForeColor="#333333"></asp:Label>

                                    <asp:DropDownList CssClass="select-dropdown" ID="ddlGridviewPaging" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
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
                                <td colspan="8">
                                    <asp:GridView ID="GridView1" EnableViewState="true" Width="1000px" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" runat="server" AutoGenerateColumns="True" AllowPaging="True"
                                        OnPageIndexChanging="OnPageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
                                        CssClass="table table-striped table-bordered table-hover gvstyling tr th" OnDataBound="GridView1_DataBound"
                                        enablepagingandcallback="false" PageSize="10" OnRowDataBound="GridView1_RowDataBound" OnPreRender="GridView1_PreRender">
                                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Left" />

                                        <PagerSettings Mode="Numeric" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />

                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>

                    </fieldset>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel ID="Panl1" runat="server" CssClass="Popup-Compare" align="center" Style="display: none">

                <fieldset class="box-border" style="margin-bottom: 0px; vertical-align: bottom; width: 90%; height: 150px; overflow-y: auto">
                    <legend class="box-border">Last 5 Compares By You</legend>
                    <div>
                        <asp:PlaceHolder ID="phLinks" runat="server"></asp:PlaceHolder>
                    </div>
                </fieldset>
                 <asp:Button ID="Button2" Style="margin-top: 15px;" CssClass="btn btn-secondary" runat="server" Text="Close" />
            </asp:Panel>

        </div>

    </div>

</asp:Content>

