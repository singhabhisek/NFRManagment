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
using Org.BouncyCastle.Asn1.Crmf;
using MySqlX.XDevAPI.Relational;

public partial class Dashboard : System.Web.UI.Page
{
    int resultSizeLimit = 100;
    static bool enable = false;

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
                int _editColumnIndex = GridView1.Columns.Count - 2;
                int _delColumnIndex = GridView1.Columns.Count - 1;
                if (Session["CurrentUserRole"].ToString() == "User")
                {
                    
                    GridView1.Columns[_editColumnIndex].Visible = false;
                    GridView1.Columns[_delColumnIndex].Visible = false;
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
            trxNames = dt.AsEnumerable().Where(x => x.Field<String>("transactionNames").ToLower().Contains(prefixText.ToLower())).Select(x => x[0].ToString()).ToList();
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

        BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);
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
        BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);

    }

    // Common method to show a gray alert using jQuery
    private void ShowGrayAlert(string message)
    {
       
        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", $"showAlert('{message}')", true);

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

        
        // Validate if the text is a number
        int result;
        if (!int.TryParse(SLA.Text, out result))
        {
            // Display an error message (you can customize this part based on your needs)
            //ScriptManager.RegisterStartupScript(this, GetType(), "validation", "alert('Please enter valid numeric SLA.');", true);
            ShowGrayAlert("This is a sample alert message.");

            // Cancel the update operation
            e.Cancel = true;
        }

        else
        {
            // Get the TextBox control from the GridView cell
            TextBox textBox = (TextBox)GridView1.Rows[e.RowIndex].Cells[0].Controls[0];

            // Validate if the text is a number

            if (!int.TryParse(TPS.Text, out result))
            {
                // Display an error message (you can customize this part based on your needs)
                //ScriptManager.RegisterStartupScript(this, GetType(), "validation", "alert('Please enter valid numeric TPS.');", true);
                ShowGrayAlert("This is a sample alert message for TPS.");

                // Cancel the update operation
                e.Cancel = true;
            }
            else
            {

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
        }
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
        //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
        //{
        //    (e.Row.Cells[9].Controls[0] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
        //}

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

        //if (e.Row.RowType == DataControlRowType.Header && iGridViewCount > 0 && ddlApplicationName.SelectedIndex > 0)
        //{
        //    int rowCount = GridView1.Columns.Count;

        //    e.Row.Cells[rowCount - 2].Text = "Edit Record";
        //    e.Row.Cells[rowCount - 1].Text = "Delete Record";
        //}

    }

    void BindGrid(GridView gridView, DropDownList ddlApplicationName, DropDownList ddlReleaseID,
            TextBox txtTransactionName)
    {

        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            String strSearch;
            using (SqlCommand cmd = new SqlCommand())
            {
                if (ddlApplicationName.SelectedIndex > 0)
                {
                    strSearch = "SELECT TOP 100 * FROM [NFRDetails]";
                }
                else
                {
                    strSearch = "SELECT * FROM [NFRDetails] where 1=2";
                }
                if (ddlApplicationName.SelectedIndex > 0)
                {
                    strSearch = strSearch + " WHERE [applicationName]=@applicationName";
                    cmd.Parameters.AddWithValue("applicationName", ddlApplicationName.Text);

                    if (ddlReleaseID.SelectedIndex > 0)
                    {
                        strSearch = strSearch + " AND [releaseID]=@releaseID";
                        cmd.Parameters.AddWithValue("releaseID", ddlReleaseID.Text);
                    }

                    if (txtTransactionName.Text.Length > 0)
                    {
                        String textSearch;
                        textSearch = txtTransactionName.Text;
                        if (txtTransactionName.Text.EndsWith("*"))
                        {
                            textSearch = txtTransactionName.Text.Remove(txtTransactionName.Text.Length - 1);
                        }
                        strSearch = strSearch + " AND (([transactionNames] like '%' + @transactionNames) )";
                        //OR (SOUNDEX([transactionNames]) like  SOUNDEX(@transactionNames))
                        cmd.Parameters.AddWithValue("transactionNames", textSearch);
                    }

                }

                cmd.CommandText = strSearch;
                //create parameters with specified name and values
                cmd.Connection = con;

                DataTable dt = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
                //retrive data
                totalRowCount = dt.Rows.Count;
                if (dt.Rows.Count == 0)
                {
                    resetGridView(gridView);
                    //this.Label1.Text = "No Data Found";
                    //return;
                }
                else
                {
                    HttpContext.Current.Session["gridviewsouce"] = dt;
                    gridView.DataSource = dt;
                    gridView.DataBind();
                }
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
                BindGrid(GridView1, ddlApplicationName, ddlReleaseID, txtTransactionName);

                //returnCountFromTables(ddlApplicationName)
                int cntRows = 0;
                try
                {
                    DataTable dt = Session["gridviewsouce"] as DataTable;
                    cntRows = dt.Rows.Count;
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

        addCookiesInStack("mruDashboard", cookieToInsert);


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
        HttpCookie cookieObj = Request.Cookies["mruDashboard"];
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




}