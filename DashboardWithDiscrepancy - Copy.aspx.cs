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

public partial class DashboardWithDiscrepancy : System.Web.UI.Page
{
    int resultSizeLimit = 100;
    static bool enable = false;
    int iGridViewCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            

            BindApplicationDropdown();

            int size = int.Parse(ddlGridviewPaging.SelectedItem.Value.ToString());
            GridView1.PageSize = size;
            pageSize = size;

            //ShowData();
            //added on 07/25
            // ddlGridviewPaging.Items.Insert(0, ("Select"));
            ddlGridviewPaging.SelectedIndex = 0;
            if (Session["CurrentUserRole"] != null)
            {
                //int _editColumnIndex = GridView1.Columns.Count - 2;
                //int _delColumnIndex = GridView1.Columns.Count - 1;
                if (Session["CurrentUserRole"].ToString() == "User")
                {
                    
                   // GridView1.Columns[_editColumnIndex].Visible = false;
                   // GridView1.Columns[_delColumnIndex].Visible = false;
                }
               
            }
            
            retrieveStack();


        }
        else if (enable)
        {
            retrieveStack();

        }
    }
    [WebMethod]
    public static List<string> getTrxNames(string prefixText)
    {
        List<string> trxNames = new List<string>();
        DataTable dt = HttpContext.Current.Session["TrxTableListDashboard"] as DataTable;
        try
        {
            trxNames = dt.AsEnumerable().Where(x => x.Field<String>("transactionName").ToLower().Contains(prefixText.ToLower())).Select(x => x[0].ToString()).ToList();
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
        }
        catch { }
        return trxNames;
    }



    string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //Method for DataBinding  
    protected void ShowData()
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(cs);
        String strSQL = "with NFRDetailDepend as (select t.applicationName, t.transactionName, t.releaseID, t.SLA, t.TPS, d.backendCall, d.callType from NFRDetails t LEFT JOIN       NFROperationDependency d ON t.transactionName = d.transactionName) select ApplicationName, releaseID, transactionName, SLA, TPS, IsNull(TotalSyncSLA,0) + IsNull(MaxAsyncSLA,0) as 'TotalBackendCallDuration', backendCall, CASE WHEN IsNull(TotalSyncSLA,0) + IsNull(MaxAsyncSLA,0) = 0 then 'NA' ELSE CASE WHEN SLA > TotalSyncSLA + MaxAsyncSLA then 'Higher' else 'Lower' END END as 'Compare' FROM (SELECT ApplicationName, transactionName, releaseID, SLA, TPS, backendCall, SUM( CASE WHEN CallType = 'Sync' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY transactionName) AS TotalSyncSLA, MAX( CASE WHEN CallType = 'Async' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY transactionName) AS MaxAsyncSLA FROM ( SELECT ApplicationName, transactionName, backendCall, CallType, SLA, releaseID, TPS, CASE WHEN CallType = 'Async' THEN ( SELECT MAX(SLA) FROM NFRDetailDepend WHERE transactionName = t.backendCall AND t.CallType = 'Async') WHEN CallType = 'Sync' THEN ( SELECT SUM(SLA) FROM NFRDetailDepend WHERE transactionName = t.backendCall AND t.CallType = 'Sync' ) ELSE 0 END AS SLAComparison FROM NFRDetailDepend t where ApplicationName = '" + ddlApplicationName.SelectedValue + "' and releaseID= '" + ddlReleaseID.SelectedValue + "' ) as x ) as p;";
        // SqlDataAdapter adapt = new SqlDataAdapter("select ApplicationName, releaseID, transactionName, SLA, IsNull(TotalSyncSLA,0), IsNull(MaxAsyncSLA,0), backendCall, CASE WHEN IsNull(TotalSyncSLA,0) + IsNull(MaxAsyncSLA,0) = 0 then 'NA' ELSE CASE WHEN SLA > TotalSyncSLA + MaxAsyncSLA then 'Higher' else 'Lower' END END as 'Compare' FROM (SELECT ApplicationName, transactionName, releaseID, SLA, backendCall, SUM( CASE WHEN CallType = 'Sync' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY transactionName) AS TotalSyncSLA, MAX( CASE WHEN CallType = 'Async' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY transactionName) AS MaxAsyncSLA FROM ( SELECT ApplicationName, transactionName, backendCall, CallType, SLA, releaseID, CASE WHEN CallType = 'Async' THEN ( SELECT MAX(SLA) FROM NFRDetails WHERE transactionName = t.backendCall AND t.CallType = 'Async' and releaseID= '" + ddlReleaseID.SelectedValue + "') WHEN CallType = 'Sync' THEN ( SELECT SUM(SLA) FROM NFRDetails WHERE transactionName = t.backendCall AND t.CallType = 'Sync' and releaseID= '" + ddlReleaseID.SelectedValue + "') ELSE 0 END AS SLAComparison FROM NFRDetails t where ApplicationName = '" + ddlApplicationName.SelectedValue + "' and releaseID= '" + ddlReleaseID.SelectedValue + "' ) as x ) as p;", con);
        SqlDataAdapter adapt = new SqlDataAdapter(strSQL, con); con.Open();
        adapt.Fill(dt);
        con.Close();
        if (dt.Rows.Count > 0)
        {

            DataTable combinedTable = new DataTable();
            combinedTable.Columns.Add("ApplicationName", typeof(string));
            combinedTable.Columns.Add("releaseID", typeof(string));
            combinedTable.Columns.Add("transactionName", typeof(string));
            combinedTable.Columns.Add("SLA", typeof(Double));
            combinedTable.Columns.Add("TPS", typeof(Double));
            combinedTable.Columns.Add("backendCall", typeof(string));
            combinedTable.Columns.Add("Compare", typeof(string));




            // Assuming you have an existing DataTable called "originalDataTable"

            // Create a grouping to group rows based on the combination of columns
            var groups = dt.AsEnumerable()
                .GroupBy(row => new
                {
                    ApplicationName = row.Field<string>("ApplicationName"),
                    ReleaseID = row.Field<string>("ReleaseID"),
                    transactionName = row.Field<string>("transactionName"),
                    SLA = row.Field<Double>("SLA"),
                    TPS = row.Field<Double>("TPS"),
                    Compare = row.Field<string>("Compare"),
                    TotalBackendCallDuration = row.Field<Double>("TotalBackendCallDuration")
                });

            // Iterate through each group and concatenate the BackendCall values
            foreach (var group in groups)
            {
                var backendCalls = group.Select(row => row.Field<string>("BackendCall"))
                                        .Where(call => !string.IsNullOrEmpty(call));



                // Combine the multiple backend call values into a string
                string concatenatedBackendCalls = "<b>Backend Duration (sec)</b>: " + group.Key.TotalBackendCallDuration + "<br><b>Backend Calls:</b> <br>";
                concatenatedBackendCalls += string.Join(",<br>", backendCalls);

                // Add a new row to the newDataTable with the combined values
                combinedTable.Rows.Add(
                    group.Key.ApplicationName,
                    group.Key.ReleaseID,
                    group.Key.transactionName,
                    group.Key.SLA,
                    group.Key.TPS,
                    concatenatedBackendCalls,
                    group.Key.Compare
                );
            }

            totalRowCount = combinedTable.Rows.Count;

            GridView1.DataSource = combinedTable;
            GridView1.DataBind();
        }
        else
        {
                resetGridView(GridView1);
                //this.Label1.Text = "No Data Found";
                //return;
         
        }
    }


    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

        int size = int.Parse(ddlGridviewPaging.SelectedItem.Value.ToString());
        GridView1.PageSize = size;
        pageSize = size;
        ShowData();

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

    protected void DeleteRecord(object sender, EventArgs e)
    {
        int Id = int.Parse((sender as LinkButton).CommandArgument);
       // GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
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

        ShowData();
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int Id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        //GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        //string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //SqlConnection conn = new SqlConnection(constr);

        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlCommand cmd = new SqlCommand("DELETE FROM NFRDetails WHERE Id = @Id"))
        //    {
        //        cmd.Parameters.AddWithValue("@Id", Id);
        //        cmd.Connection = con;
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        //BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {

        GridView1.EditIndex = e.NewEditIndex;
        ShowData();

    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        int Id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]); GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        TextBox applicationName = (TextBox)row.Cells[0].Controls[0];
        TextBox releaseID = (TextBox)row.Cells[1].Controls[0];
        TextBox businessScenario = (TextBox)row.Cells[2].Controls[0];
        TextBox transactionName = (TextBox)row.Cells[3].Controls[0];
        TextBox SLA = (TextBox)row.Cells[4].Controls[0];
        TextBox TPS = (TextBox)row.Cells[5].Controls[0];
        TextBox Comments = (TextBox)row.Cells[6].Controls[0];
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE NFRDetails SET applicationName = @applicationName, releaseID = @releaseID , businessScenario = @businessScenario , transactionName = @transactionName  , SLA = @SLA  , TPS = @TPS  , Comments = @Comments  WHERE Id = @Id"))
            {
                cmd.Parameters.AddWithValue("@Id", Id);

                cmd.Parameters.AddWithValue("@applicationName", applicationName.Text);
                cmd.Parameters.AddWithValue("@releaseID", releaseID.Text);
                cmd.Parameters.AddWithValue("@businessScenario", businessScenario.Text);
                cmd.Parameters.AddWithValue("@transactionName", transactionName.Text);
                cmd.Parameters.AddWithValue("@SLA", SLA.Text);
                cmd.Parameters.AddWithValue("@TPS", TPS.Text);
                cmd.Parameters.AddWithValue("@Comments", Comments.Text);
                
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        GridView1.EditIndex = -1;
        ShowData();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        ShowData();
    }
    protected void ddlApplicationName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Added on 07/25
        //Code to reset the gridviews when changing the search criteria
        resetGridView(GridView1);

        //Code to reset the Release Dropdown when changing the search criteria
        ddlReleaseID.Items.Clear();
        txtTransactionName.Text = "";

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
        if ((GridView1.DataSource as DataTable) == null)
        {
            //lblError.Text = "Total Rows: 0";
        }
        else
        {
            iGridViewCount = (GridView1.DataSource as DataTable).Rows.Count;
            lblError.Text = "Total Rows: " + iGridViewCount;
        }

        //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Edit)
        //{
        //    for (int ictr = 0; ictr < iGridViewCount; ictr++)
        //    {
        //        TextBox comments = (TextBox)e.Row.Cells[ictr].Controls[0];
        //        comments.Height = 20;
        //        comments.Width = 100;
        //    }

        //}

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the value of the "Status" column for the current row
            string status = DataBinder.Eval(e.Row.DataItem, "Compare").ToString();

            // Find the Image control in the "Icon" column
            Image imgStatus = (Image)e.Row.FindControl("imgStatus");

            // Set the image URL based on the "Status" column value
            if (status == "Higher")
            {
                imgStatus.ImageUrl = "Resources/images/uparrow.png";
            }
            else if (status == "Lower")
            {
                imgStatus.ImageUrl = "Resources/images/downarrow.png";
            }
            else if (status == "NA")
            {
                imgStatus.ImageUrl = "Resources/images/nochange.png";
            }
        }

    }

   

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        resetGridView(GridView1);
        if (ddlApplicationName.SelectedIndex < 1)
        {
            lblError.Text = "Application Name is mandatory field with a combination of ReleaseID OR TransactionName.";
            ddlApplicationName.Focus();
           
        }
        else if (txtTransactionName.Text.Trim().Length > 0 && txtTransactionName.Text.Trim().Length < 3)
        {
            lblError.Text = "Minimum of three characters required for search with Transaction Names.";
            txtTransactionName.Focus();

        }
        else
        {

            lblError.Text = "";
            if (txtTransactionName.Text.Trim().Length == 0 && ddlReleaseID.SelectedIndex < 1)
            {
                lblError.Text = "With Application, please filter by one of the other criterias - Release or Transaction";
            }
            else
            {
                ShowData();

                //returnCountFromTables(ddlApplicationName)
                int cntRows = 0;
                try
                {
                    DataTable dt = Session["gridviewsouce"] as DataTable;
                    cntRows = iGridViewCount; //dt.Rows.Count;
                }
                catch (Exception ex)
                {
                    cntRows = 0;
                }
                if (cntRows == resultSizeLimit)

                {
                    lblError.Text = "Showing top 100 records. More than " + resultSizeLimit + " records may exist. Please use additional search fields - Release or Transaction to narrow search results";
                }
            }


        }
        String cookieToInsert = "Selected application=" + ddlApplicationName.SelectedValue.ToString();
        if (ddlReleaseID.SelectedIndex > 0)
            cookieToInsert += "&ReleaseID=" + ddlReleaseID.SelectedValue.ToString();
        if (txtTransactionName.Text.Length > 0)
            cookieToInsert += "&TransactionName=" + txtTransactionName.Text.ToString();

        addCookiesInStack("mruDashboardDiscrepancy", cookieToInsert);


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
        resetGridView(GridView1);
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
        //if (Request.Cookies["mruDashboard"] != null)
        //{
        //    Response.Cookies["mruDashboard"].Expires = DateTime.Now.AddDays(-1);
        //}

    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        SetFixedHeightForGridIfRowsAreLess(GridView1);
    }



    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        createPagingSummaryOnPagerTemplate(sender, totalRowCount, pageSize);
    }

    //added for last 5 searches

    void retrieveStack()
    {
        HttpCookie cookieObj = Request.Cookies["mruDashboardDiscrepancy"];
        //--- Check for null 
        if (cookieObj != null)
        {
            enable = true;
            string[] myArray = cookieObj.Value.Split(',');
            for (int i = 0; i < myArray.Length; i++)
            {
                //Create hyperlink for each cookie
                AddLinkURL(myArray[i], myArray[i]);
            }

        }

    }

    private void AddLinkURL(string text, string url)
    {
        LinkButton hyperlink = new LinkButton();
        hyperlink.ID = "HyperLink" + (phLinks.Controls.Count + 1); // Generate a unique ID for each link
        hyperlink.Text = text;

        hyperlink.Click += new EventHandler(getMethod);

        phLinks.Controls.Add(hyperlink); // Add the link to a placeholder control (phLinks)
        phLinks.Controls.Add(new LiteralControl("<br />"));

        ScriptManager.RegisterClientScriptBlock(hyperlink as LinkButton, this.GetType(), "alert", "", true);
    }

    protected void getMethod(object sender, EventArgs e)
    {
        try
        {


            //  Response.Write("<script>alert('done'); </script>");
            ddlApplicationName.ClearSelection();
            ddlReleaseID.ClearSelection();
            txtTransactionName.Text = string.Empty;

            LinkButton h = sender as LinkButton;
            //lblError.Text = h.Text;
            String[] finalResult = null;
            finalResult = h.Text.Split('&');

            for (int j = 0; j < finalResult.Length; j++)
            {
                //lblError.Text = finalResult[j];
                //lblError.Text = finalResult[j].Split('=')[1];
                switch (finalResult[j].Split('=')[0])
                {

                    case "Selected application":
                        ddlApplicationName.ClearSelection();
                        ddlApplicationName.Items.FindByText(finalResult[j].Split('=')[1]).Selected = true;
                        ddlApplicationName_SelectedIndexChanged(this, EventArgs.Empty);
                        break;
                    case "ReleaseID":
                        ddlReleaseID.ClearSelection();
                        ddlReleaseID.Items.FindByText(finalResult[j].Split('=')[1]).Selected = true;
                        ddlReleaseID_SelectedIndexChanged(this, EventArgs.Empty);
                        break;
                    case "TransactionName":
                        txtTransactionName.Text = string.Empty;
                        txtTransactionName.Text = finalResult[j].Split('=')[1];
                        break;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }




}