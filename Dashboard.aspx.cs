using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using static CommonLibraryFunctions.CommonMethods;
using System.Text;

public partial class Dashboard : System.Web.UI.Page
{
    int resultSizeLimit = 3;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            BindApplicationDropdown();

            int size = int.Parse(ddlGridviewPaging.SelectedItem.Value.ToString());
            GridView1.PageSize = size;
            pageSize = size;

            BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);
            //added on 07/25
            // ddlGridviewPaging.Items.Insert(0, ("Select"));
            ddlGridviewPaging.SelectedIndex = 0;
            if (Session["CurrentUserRole"] != null)
            {
                if (Session["CurrentUserRole"].ToString() == "User")
                {
                    int _editColumnIndex = 8;
                    int _delColumnIndex = 9;
                    GridView1.Columns[_editColumnIndex].Visible = false;
                    GridView1.Columns[_delColumnIndex].Visible = false;
                }
            }

        }
    }

    [WebMethod]
    public static List<string> getTrxNames(string prefixText)
    {
        List<string> trxNames = new List<string>();
        DataTable dt = HttpContext.Current.Session["TrxTableListDashboard"] as DataTable;
        trxNames = dt.AsEnumerable().Where(x => x.Field<String>("transactionNames").Contains(prefixText)).Select(x => x[0].ToString()).ToList();
        if (trxNames.Count <= 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int result = 0;

                result = LevenshteinDistance(dt.Rows[i][0].ToString(), prefixText);

                if (result < 10)
                {
                    trxNames.Add(dt.Rows[i][0].ToString()); // + result.ToString()
                }

            }
        }
            return trxNames;
    }

    //added on 07/25
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

        int size = int.Parse(ddlGridviewPaging.SelectedItem.Value.ToString());
        GridView1.PageSize = size;
        pageSize = size;
        BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    private void BindApplicationDropdown()
    {
        string query = "select distinct [applicationName] from NFRDetails";
        BindDropDownList(ddlApplicationName, query, "applicationName", "applicationName", "-Select Application-");
        ddlReleaseID.Items.Insert(0, new ListItem("-Select Release-", "0"));
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        DataTable dt = Session["gridviewsouce"] as DataTable;
        totalRowCount = dt.Rows.Count;
        pageSize = int.Parse(ddlGridviewPaging.SelectedItem.Value.ToString());
        GridView1.DataSource = dt;
        GridView1.DataBind();
        //this.BindGrid();
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("DELETE FROM NFRDetails WHERE Id = @Id"))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {

        GridView1.EditIndex = e.NewEditIndex;
        BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);

    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        int Id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]); GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        TextBox applicationName = (TextBox)row.Cells[0].Controls[0];
        TextBox releaseID = (TextBox)row.Cells[1].Controls[0];
        TextBox businessScenario = (TextBox)row.Cells[2].Controls[0];
        TextBox transactionNames = (TextBox)row.Cells[3].Controls[0];
        TextBox SLA = (TextBox)row.Cells[4].Controls[0];
        TextBox TPS = (TextBox)row.Cells[5].Controls[0];
        TextBox backendCall = (TextBox)row.Cells[6].Controls[0];
        TextBox callType = (TextBox)row.Cells[7].Controls[0];
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE NFRDetails SET applicationName = @applicationName, releaseID = @releaseID , businessScenario = @businessScenario , transactionNames = @transactionNames  , SLA = @SLA  , TPS = @TPS  , backendCall = @backendCall  , callType = @callType  WHERE Id = @Id"))
            {
                cmd.Parameters.AddWithValue("@Id", Id);

                cmd.Parameters.AddWithValue("@applicationName", applicationName.Text);
                cmd.Parameters.AddWithValue("@releaseID", releaseID.Text);
                cmd.Parameters.AddWithValue("@businessScenario", businessScenario.Text);
                cmd.Parameters.AddWithValue("@transactionNames", transactionNames.Text);
                cmd.Parameters.AddWithValue("@SLA", SLA.Text);
                cmd.Parameters.AddWithValue("@TPS", TPS.Text);
                cmd.Parameters.AddWithValue("@backendCall", backendCall.Text);
                cmd.Parameters.AddWithValue("@callType", callType.Text);

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        GridView1.EditIndex = -1;
        BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);

    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);
    }
    protected void ddlApplicationName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Added on 07/25
        //Code to reset the gridviews when changing the search criteria
        resetGridView(GridView1);

        //Code to reset the Release Dropdown when changing the search criteria
        ddlReleaseID.Items.Clear();

        //Create Searach string and bind results to ReleaseID Dropdown
        string query = string.Format("select distinct ReleaseID from NFRDetails where ApplicationName = '{0}'", ddlApplicationName.SelectedItem.Value);
        BindDropDownList(ddlReleaseID, query, "ReleaseID", "ReleaseID", "-Select ReleaseID-");

        //Get the details for transactionName auto populate feature
        FillTransactionNameDT(ddlApplicationName.SelectedValue.ToString(), null, "TrxTableListDashboard");
    }

    private void BindDropDownList(DropDownList ddl, string query, string text, string value, string defaultText)
    {
        string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlCommand cmd = new SqlCommand(query);
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                con.Open();
                ddl.DataSource = cmd.ExecuteReader();
                ddl.DataTextField = text;
                ddl.DataValueField = value;
                ddl.DataBind();
                con.Close();
            }
        }
        ddl.Items.Insert(0, new ListItem(defaultText, "0"));
    }

    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        ExportGridToExcel(GridView1);
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Edit)
        {
            for (int ictr = 0; ictr < 8; ictr++)
            {
                TextBox comments = (TextBox)e.Row.Cells[ictr].Controls[0];
                comments.Height = 20;
                comments.Width = 100;
            }

        }

        //Added for Delete Confirmation record
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
        {
            (e.Row.Cells[9].Controls[0] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
        }

        //added for Total Number record display

        int iGridViewCount = 0;
        if ((GridView1.DataSource as DataTable) == null)
        {
            //lblError.Text = "Total Rows: 0";
        }
        else
        {
            iGridViewCount = (GridView1.DataSource as DataTable).Rows.Count;
            //lblError.Text = "Total Rows: " + iGridViewCount;
        }

        //Change header text for Edit and Delete Columns

        if (e.Row.RowType == DataControlRowType.Header && iGridViewCount > 0 && ddlApplicationName.SelectedIndex > 0)
        {
            int rowCount = GridView1.Columns.Count;

            e.Row.Cells[rowCount - 2].Text = "Edit Record";
            e.Row.Cells[rowCount - 1].Text = "Delete Record";
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        resetGridView(GridView1);
        if (txtTransactionName.Text.Trim().Length > 0 && txtTransactionName.Text.Trim().Length < 3)
        {
            lblError.Text = "Minimum of three characters required for search with Transaction Names.";
            txtTransactionName.Focus();

        }
        else { 
            int cntRows = returnCountFromTables(ddlApplicationName);
            if (cntRows < resultSizeLimit)
            {
                lblError.Text = "";
                BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);
            }
            else
            {
                lblError.Text = "Error: More than " + resultSizeLimit + " records found. Please use additional search fields - Release or Transaction";
            }
        }
        
            

        

    }

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow pagerRow = (GridViewRow)gv.BottomPagerRow;

        if (pagerRow != null && pagerRow.Visible == false)
            pagerRow.Visible = true;
    }

    protected void ddlReleaseID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReleaseID.SelectedIndex > 0)
        {
            FillTransactionNameDT(ddlApplicationName.SelectedValue.ToString(), ddlReleaseID.SelectedValue.ToString(), "TrxTableListDashboard");
        }
        else
        {
            FillTransactionNameDT(ddlApplicationName.SelectedValue.ToString(), null, "TrxTableListDashboard");
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlApplicationName.SelectedIndex = 0;
        ddlReleaseID.Items.Clear();
        txtTransactionName.Text = "";
        resetGridView(GridView1);
        Session.Remove("TrxTableListDashboard");
        Session.Remove("gridviewsourceCompare");
        lblError.Text = "";
            
    }

    ////Added in 07/27
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        SetFixedHeightForGridIfRowsAreLess(GridView1);
    }

    //public void SetFixedHeightForGridIfRowsAreLess(GridView gv)
    //{
    //    double headerFooterHeight = gv.HeaderStyle.Height.Value + 35; //we set header height style=35px and there no footer  height so assume footer also same
    //    double rowHeight = gv.RowStyle.Height.Value;
    //    int gridRowCount = gv.Rows.Count;
    //    if (gridRowCount <= gv.PageSize)
    //    {
    //        double height = (gridRowCount * rowHeight) + ((gv.PageSize - gridRowCount) * rowHeight) + headerFooterHeight;
    //        //adjust footer height based on white space removal between footer and last row
    //        height += 40;
    //        gv.Height = new Unit(height);
    //    }
    //}

    //public void createPagingSummaryOnPagerTemplate(object sender, int totalCount, int pageSize)
    //{
    //    GridView gv = sender as GridView;
    //    if (gv != null)
    //    {
    //        //Get Bottom Pager Row from a gridview
    //        GridViewRow row = gv.BottomPagerRow;

    //        if (row != null)
    //        {
    //            //create new cell to add to page strip
    //            TableCell pagingSummaryCell = new TableCell();
    //            pagingSummaryCell.Text = DisplayCusotmPagingSummary(totalCount, gv.PageIndex, pageSize);
    //            pagingSummaryCell.HorizontalAlign = HorizontalAlign.Right;
    //            pagingSummaryCell.VerticalAlign = VerticalAlign.Middle;
    //            pagingSummaryCell.Width = Unit.Percentage(100);
    //            pagingSummaryCell.Height = Unit.Pixel(35);
    //            //Getting table which shows PagingStrip
    //            Table tbl = (Table)row.Cells[0].Controls[0];

    //            gv.BottomPagerRow.Visible = true;
    //            //BottomPagerRow will be visible false if pager doesn't have numbers and page number 1 will be displayed
    //            //if (totalCount <= pageSize)
    //            //{
                    
    //            //    tbl.Rows[0].Cells.Clear();
    //            //    tbl.Width = Unit.Percentage(100);
    //            //}
    //            //Find table and add paging summary text
    //            tbl.Rows[0].Cells.Add(pagingSummaryCell);
    //            //assign header row color to footer row
    //            //tbl.BackColor = Color.Red; // System.Drawing.ColorTranslator.FromHtml("#1AD9F2");
    //            tbl.Width = Unit.Percentage(100);
    //        }
    //    }
    //}

    //public static string DisplayCusotmPagingSummary(int numberOfRecords, int currentPage, int pageSize)
    //{
    //    StringBuilder strDisplaySummary = new StringBuilder();
    //    int numberOfPages;
    //    if (numberOfRecords > pageSize)
    //    {
    //        // Calculating the total number of pages
    //        numberOfPages = (int)Math.Ceiling((double)numberOfRecords / (double)pageSize);
    //    }
    //    else
    //    {
    //        numberOfPages = 1;
    //    }
    //    strDisplaySummary.Append("Showing ");
    //    int floor = (currentPage * pageSize) + 1;
    //    strDisplaySummary.Append(floor.ToString());
    //    strDisplaySummary.Append("-");
    //    int ceil = ((currentPage * pageSize) + pageSize);

    //    if (ceil > numberOfRecords)
    //    {
    //        strDisplaySummary.Append(numberOfRecords.ToString());
    //    }
    //    else
    //    {
    //        strDisplaySummary.Append(ceil.ToString());
    //    }

    //    strDisplaySummary.Append(" of ");
    //    strDisplaySummary.Append(numberOfRecords.ToString());
    //    strDisplaySummary.Append(" results ");
    //    return strDisplaySummary.ToString();
    //}

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        createPagingSummaryOnPagerTemplate(sender, totalRowCount, pageSize);
    }

}