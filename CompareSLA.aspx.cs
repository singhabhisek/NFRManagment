using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Text;
using static CommonLibraryFunctions.CommonMethods;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web;
using System.Net;

public partial class _Default : System.Web.UI.Page
{
    static bool enableCompare = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

            BindApplicationDropdown();

            this.BindGrid();
            retrieveStack();
        }
        else if (enableCompare)
        {
            //  retrieveStack();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    private void BindApplicationDropdown()
    {
        string query = "select distinct [applicationName] from NFRDetails";
        BindDropDownList(ddlApplicationName, query, "applicationName", "applicationName", "-Select Application-");
        // ddlReleaseID.Items.Insert(0, new ListItem("-Select Release-", "0"));
    }


    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridViewRow pagerRow = GridView1.BottomPagerRow;
        //DropDownList ddlGridviewPaging = (DropDownList)pagerRow.Cells[0].FindControl("ddlGridviewPaging");

        int size = int.Parse(ddlGridviewPaging.SelectedItem.Value.ToString());
        GridView1.PageSize = size;
        pageSize = size;
        DataTable dt = Session["gridviewsourceCompare"] as DataTable;

        GridView1.DataSource = dt;
        GridView1.DataBind();

    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        DataTable dt = Session["gridviewsourceCompare"] as DataTable;
        totalRowCount = dt.Rows.Count;
        GridView1.DataSource = dt;
        GridView1.DataBind();
        //this.BindGrid();
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);
        GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        Label lbldeleteid = (Label)row.FindControl("lblID");
        conn.Open();
        SqlCommand cmd = new SqlCommand("delete FROM NFRDetails where id='" + Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString()) + "'", conn);
        cmd.ExecuteNonQuery();
        conn.Close();
        BindGrid();
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {

        GridView1.EditIndex = e.NewEditIndex;
        BindGrid();

    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int userid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
        GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
        Label lblID = (Label)row.FindControl("lblID");
        //TextBox txtname=(TextBox)gr.cell[].control[];  
        TextBox textName = (TextBox)row.Cells[0].Controls[0];
        TextBox textadd = (TextBox)row.Cells[1].Controls[0];
        TextBox textc = (TextBox)row.Cells[2].Controls[0];
        //TextBox textadd = (TextBox)row.FindControl("txtadd");  
        //TextBox textc = (TextBox)row.FindControl("txtc");  
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);
        GridView1.EditIndex = -1;
        conn.Open();
        //SqlCommand cmd = new SqlCommand("SELECT * FROM detail", conn);  
        SqlCommand cmd = new SqlCommand("update detail set name='" + textName.Text + "',address='" + textadd.Text + "',country='" + textc.Text + "'where id='" + userid + "'", conn);
        cmd.ExecuteNonQuery();
        conn.Close();
        BindGrid();
        //GridView1.DataBind();  
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindGrid();
    }
    protected void ddlApplicationName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlReleaseID.Items.Clear();
        ddlReleaseID1.Items.Clear();
        ddlReleaseID2.Items.Clear();
        txtTransactionName.Text = "";
        fillEmptyDatatable();
        //resetGridView(GridView1);
        lblError.Text = "";
        string query = string.Format("select distinct ReleaseID from NFRDetails where ApplicationName = '{0}'", ddlApplicationName.SelectedItem.Value);
        BindDropDownList(ddlReleaseID, query, "ReleaseID", "ReleaseID", "-Select ReleaseID-");
        BindDropDownList(ddlReleaseID1, query, "ReleaseID", "ReleaseID", "-Select ReleaseID-");
        BindDropDownList(ddlReleaseID2, query, "ReleaseID", "ReleaseID", "-Select ReleaseID-");
        FillTransactionNameDT(ddlApplicationName.SelectedValue.ToString(), null, "TrxTableListCompare");
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
        // ddl.Items.Insert(0, new ListItem("-All-", "0"));
        ddl.Items.Insert(0, new ListItem(defaultText, "0"));
    }

    private void ExportGridToExcel()
    {
        string FileName = "ExportExcel_" + DateTime.Now + ".xls";

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            GridView1.AllowPaging = false;
            this.BindGrid();

            GridView1.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in GridView1.HeaderRow.Cells)
            {
                cell.BackColor = GridView1.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in GridView1.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = GridView1.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }

    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        ExportGridToExcel();
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

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //resetGridView(GridView1);
        fillEmptyDatatable();
        if (ddlApplicationName.SelectedIndex < 1)
        {
            lblError.Text = "Please select Application Name with First Release ID.";
            ddlApplicationName.Focus();
        }
        else if (ddlReleaseID.SelectedIndex < 1 && ddlReleaseID1.SelectedIndex < 1 && ddlReleaseID2.SelectedIndex < 1)
        {
            lblError.Text = "Select First Release ID to start compare.";
            ddlReleaseID.Focus();
        }

        else if ((ddlReleaseID.SelectedIndex > 0
            && ddlReleaseID.SelectedValue.ToString() == ddlReleaseID1.SelectedValue.ToString()) ||
            (ddlReleaseID.SelectedIndex > 0 &&
                ddlReleaseID.SelectedValue.ToString() == ddlReleaseID2.SelectedValue.ToString()) ||
            (ddlReleaseID1.SelectedIndex > 0 &&
                ddlReleaseID1.SelectedValue.ToString() == ddlReleaseID2.SelectedValue.ToString())
            )
        {
            lblError.Text = "Error: Release Values cannot be same. Please check and change it.";
            ddlReleaseID1.SelectedIndex = 0;
            ddlReleaseID2.SelectedIndex = 0;
            ddlReleaseID1.Focus();

        }
        else
        {
            BindGrid();

            String cookieToInsertCompare = "Compare application=" + ddlApplicationName.SelectedValue.ToString();

            cookieToInsertCompare += "|ReleaseID1=" + ddlReleaseID.SelectedValue.ToString();
            cookieToInsertCompare += "|ReleaseID2=" + ddlReleaseID1.SelectedValue.ToString();
            cookieToInsertCompare += "|ReleaseID3=" + ddlReleaseID2.SelectedValue.ToString();
            cookieToInsertCompare += "|TransactionName=" + txtTransactionName.Text.ToString();

            addCookiesInStack("mruComparison", cookieToInsertCompare);

            cookieToInsertCompare = "";
        }
    }


    private void BindGrid()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            String strSearch = "";

            using (SqlCommand cmd = new SqlCommand())
            {
                if (ddlReleaseID.SelectedIndex > 0)
                {
                    if (ddlReleaseID1.SelectedIndex > 0)
                    {
                        if (ddlReleaseID2.SelectedIndex > 0)
                        {
                            strSearch = "SELECT a.[applicationName] as 'Application Name',a.[transactionName] as 'Transaction Name', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID1.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID1.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID2.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID2.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_TPS', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID1.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID1.SelectedValue.ToString() + "_TPS', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID2.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID2.SelectedValue.ToString() + "_TPS' FROM [dbo].[NFRDetails] a";
                        }
                        else
                        {
                            strSearch = "SELECT a.[applicationName] as 'Application Name',a.[transactionName] as 'Transaction Name', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID1.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID1.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_TPS', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID1.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID1.SelectedValue.ToString() + "_TPS' FROM [dbo].[NFRDetails] a ";
                        }
                    }
                    else
                    {
                        strSearch = "SELECT a.[applicationName] as 'Application Name',a.[transactionName] as 'Transaction Name', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_SLA',     ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "'  THEN TPS END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_TPS' FROM [dbo].[NFRDetails] a ";

                    }
                }

                if (ddlApplicationName.SelectedIndex > 0 && ddlReleaseID.SelectedIndex > 0)
                {
                    strSearch = strSearch + " WHERE applicationName = @applicationName ";
                    cmd.Parameters.AddWithValue("applicationName", ddlApplicationName.SelectedValue.ToString());
                    if (txtTransactionName.Text.Length > 0)
                    {
                        String textSearch;
                        textSearch = txtTransactionName.Text;
                        if (txtTransactionName.Text.EndsWith("*"))
                        {
                            textSearch = txtTransactionName.Text.Remove(txtTransactionName.Text.Length - 1);
                        }
                        strSearch = strSearch + " AND [transactionName] like '%' + @transactionName + '%'";
                        cmd.Parameters.AddWithValue("transactionName", textSearch);
                    }

                }
                else
                {
                    if (txtTransactionName.Text.Length > 0)
                    {
                        String textSearch;
                        textSearch = txtTransactionName.Text;
                        if (txtTransactionName.Text.EndsWith("*"))
                        {
                            textSearch = txtTransactionName.Text.Remove(txtTransactionName.Text.Length - 1);
                        }
                        strSearch = strSearch + " WHERE [transactionName] like '%' + @transactionName + '%'";
                        cmd.Parameters.AddWithValue("transactionName", textSearch);
                    }
                }

                // lblError.Text = strSearch;

                //Query to filter out all NA
                /*select distinct n.[Application Name], n.[Transaction Name],n.[2022.4_SLA]
                ,n.[2022.4_TPS],n.[2023.6_SLA],n.[2023.6_TPS],n.[2023.7_SLA],n.[2023.7_TPS]
                from [NFRDetails] m, (
                SELECT a.[applicationName] as 'Application Name',a.[transactionName] as 'Transaction Name', ISNULL(str(MAX(CASE WHEN releaseID='2022.4' THEN SLA END)),'NA') '2022.4_SLA', ISNULL(str(MAX(CASE WHEN releaseID='2023.6' THEN SLA END)),'NA') '2023.6_SLA',ISNULL(str(MAX(CASE WHEN releaseID='2023.7' THEN SLA END)),'NA') '2023.7_SLA', ISNULL(str(MAX(CASE WHEN releaseID='2022.4' THEN TPS END)),'NA') '2022.4_TPS', ISNULL(str(MAX(CASE WHEN releaseID='2023.6' THEN TPS END)),'NA') '2023.6_TPS', ISNULL(str(MAX(CASE WHEN releaseID='2023.7' THEN TPS END)),'NA') '2023.7_TPS'  FROM [dbo].[NFRDetails] a WHERE applicationName = 'Mobile' GROUP BY a.[applicationName],a.[transactionName]
                ) n
                where n.[Application Name] = m.applicationName
                and n.[Transaction Name] = m.transactionName
                and COALESCE(n.[2022.4_SLA], n.[2023.6_SLA] ,n.[2023.7_SLA]) != 'NA'
                ;
                */

                DataTable dt = new DataTable();

                if (strSearch.Length < 1)
                {
                    strSearch = "SELECT * FROM [NFRDetails] where 1=2";
                    dt.Columns.Add("Application Name");
                    dt.Columns.Add("Transaction Name");
                    dt.Columns.Add("Release1_SLA");
                    dt.Columns.Add("Release1_TPS");
                    dt.Columns.Add("Release2_SLA");
                    dt.Columns.Add("Release2_TPS");

                }
                else
                {
                    //"where a.[transactionName] = 'OLB_Login' ";
                    strSearch = strSearch + "GROUP BY a.[applicationName],a.[transactionName];";
                    cmd.CommandText = strSearch;
                    //create parameters with specified name and values
                    cmd.Connection = con;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);
                    }
                }

                //retrive data

                if (dt.Rows.Count == 0)
                {
                    GridView1.DataSource = dt; // new List<string>();
                    Session["gridviewsourceCompare"] = dt;
                    GridView1.DataBind();
                    //this.Label1.Text = "No Data Found";
                    //return;
                }
                else
                {
                    totalRowCount = dt.Rows.Count;
                    //                    pageSize = int.Parse(ddlGridviewPaging.SelectedItem.Value.ToString());

                    Session["gridviewsourceCompare"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }

    }

    protected void btnImportExcel_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void ddlReleaseID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReleaseID.SelectedIndex > 0)
        {
            lblError.Text = "";
        }
        //ddlReleaseID1.Items.FindByValue(ddlReleaseID.SelectedValue.ToString()).Attributes.Add("disabled", "disabled");
        //ddlReleaseID2.Items.FindByValue(ddlReleaseID.SelectedValue.ToString()).Attributes.Add("disabled", "disabled");

        // ddlReleaseID1.Items.Remove(ddlReleaseID.SelectedValue.ToString());
        // ddlReleaseID2.Items.Remove(ddlReleaseID.SelectedValue.ToString());
    }

    protected void ddlReleaseID1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlReleaseID2.Items.FindByValue(ddlReleaseID.SelectedValue.ToString()).Attributes.Add("disabled", "disabled");
        //ddlReleaseID2.Items.FindByValue(ddlReleaseID1.SelectedValue.ToString()).Attributes.Add("disabled", "disabled");
    }
    public void fillEmptyDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Application Name");
        dt.Columns.Add("Transaction Name");
        dt.Columns.Add("Release1_SLA");
        dt.Columns.Add("Release1_TPS");
        dt.Columns.Add("Release2_SLA");
        dt.Columns.Add("Release2_TPS");
        GridView1.DataSource = dt; // new List<string>();
        GridView1.DataBind();

    }

    public void resetAll()
    {
        fillEmptyDatatable();
        ddlReleaseID.Items.Clear();
        ddlReleaseID1.Items.Clear();
        ddlReleaseID2.Items.Clear();
        ddlApplicationName.SelectedIndex = 0;
        ddlReleaseID.SelectedIndex = -1;
        ddlReleaseID1.SelectedIndex = -1;
        ddlReleaseID2.SelectedIndex = -1;
        txtTransactionName.Text = "";
        lblError.Text = "";
        Session.Remove("TrxTableListCompare");
        Session.Remove("gridviewsourceCompare");
        ddlApplicationName.Focus();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        resetAll();
        
    }

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow pagerRow = (GridViewRow)gv.BottomPagerRow;
        if (pagerRow != null)
            pagerRow.Visible = true;
        //if (pagerRow != null && pagerRow.Visible == false)
        //    pagerRow.Visible = true;
    }

    [WebMethod]
    public static List<string> getTrxNames(string prefixText)
    {
        List<string> trxNames = new List<string>();
        DataTable dt = HttpContext.Current.Session["TrxTableListCompare"] as DataTable;
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
        return trxNames;
    }

    ////Added in 07/27
    ///

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        SetFixedHeightForGridIfRowsAreLess(GridView1);
    }



    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        createPagingSummaryOnPagerTemplate(sender, totalRowCount, pageSize);

    }

    void retrieveStack()
    {
        phLinks.Controls.Clear();

        HttpCookie cookieObj = Request.Cookies["mruComparison"];
        string[] myArray;
        //--- Check for null 
        if (cookieObj != null)
        {
            enableCompare = true;
            myArray = cookieObj.Value.Split(',');
            for (int i = 0; i < myArray.Length; i++)
            {
                //lblError.Text = myArray[i]; 
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
        finalResult = h.Text.Split('|');

        for (int j = 0; j < finalResult.Length; j++)
        {
            switch (finalResult[j].Split('=')[0])
            {

                case "Compare application":
                    ddlApplicationName.ClearSelection();
                    ddlApplicationName.Items.FindByText(finalResult[j].Split('=')[1]).Selected = true;
                    ddlApplicationName_SelectedIndexChanged(this, EventArgs.Empty);
                    break;
                case "ReleaseID1":
                    ddlReleaseID.ClearSelection();
                    if (finalResult[j].Split('=')[1] != "0")
                    {
                        ddlReleaseID.Items.FindByText(finalResult[j].Split('=')[1]).Selected = true;
                    }
                    break;

                case "ReleaseID2":
                    ddlReleaseID1.ClearSelection();
                    if (finalResult[j].Split('=')[1] != "0")
                    {
                        ddlReleaseID1.Items.FindByText(finalResult[j].Split('=')[1]).Selected = true;
                    }
                    break;
                case "ReleaseID3":
                    ddlReleaseID2.ClearSelection();
                    if (finalResult[j].Split('=')[1] != "0")
                    {
                        ddlReleaseID2.Items.FindByText(finalResult[j].Split('=')[1]).Selected = true;
                    }

                    break;
                case "TransactionName":
                    txtTransactionName.Text = string.Empty;
                    txtTransactionName.Text = finalResult[j].Split('=')[1];
                    break;
            }
        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        retrieveStack();
        mp1.Show();

    }
}
