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

public partial class _Default : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindApplicationDropdown();

            this.BindGrid();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    private void BindApplicationDropdown()
    {
        
        string query = "select distinct [applicationName] from NFRProTable";
        BindDropDownList(ddlApplicationName, query, "applicationName", "applicationName", "-Select Application-");
       // ddlReleaseID.Items.Insert(0, new ListItem("-Select Release-", "0"));

    }

    //private void BindGrid1()
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    using (SqlConnection con = new SqlConnection(constr))
    //    {
    //        using (SqlCommand cmd = new SqlCommand("SELECT a.[applicationName] as 'Application Name',a.[transactionNames] as 'Transaction Name',     MAX(CASE WHEN releaseID='2023.M02' THEN SLA END) M02_SLA,     MAX(CASE WHEN releaseID='2023.M03' THEN SLA END) M03_SLA,     MAX(CASE WHEN releaseID='2023.M02' THEN TPS END) M02_TPS,     MAX(CASE WHEN releaseID='2023.M03' THEN TPS END) M03_TPS FROM [dbo].[NFRProTable] a where a.[transactionNames] = 'OLB_Login' GROUP BY a.[applicationName]       ,a.[transactionNames];"))
    //        {
    //            using (SqlDataAdapter sda = new SqlDataAdapter())
    //            {
    //                cmd.Connection = con;
    //                sda.SelectCommand = cmd;
    //                using (DataTable dt = new DataTable())
    //                {
    //                    sda.Fill(dt);
    //                    
    //                    GridView1.DataSource = dt;
    //                    GridView1.DataBind();
    //                }
    //            }
    //        }
    //    }
    //}

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

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
        SqlCommand cmd = new SqlCommand("delete FROM NFRProTable where id='" + Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString()) + "'", conn);
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
        string query = string.Format("select distinct ReleaseID from NFRProTable where ApplicationName = '{0}'", ddlApplicationName.SelectedItem.Value);
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
        resetGridView(GridView1);
        if (ddlReleaseID.SelectedIndex < 1 && ddlReleaseID1.SelectedIndex < 1 && ddlReleaseID2.SelectedIndex < 1)
        {
            lblError.Text = "Select First Release Item";
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
            fillEmptyDatatable();
            ddlReleaseID1.SelectedIndex= 0;
            ddlReleaseID2.SelectedIndex= 0;
            ddlReleaseID1.Focus();

        }
        else
        {
            BindGrid();
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
                            strSearch = "SELECT a.[applicationName] as 'Application Name',a.[transactionNames] as 'Transaction Name', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID1.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID1.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID2.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID2.SelectedValue.ToString() + "'_SLA, ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_TPS', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID1.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID1.SelectedValue.ToString() + "_TPS', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID2.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID2.SelectedValue.ToString() + "_TPS' FROM [dbo].[NFRProTable] a";  
                        }
                        else
                        {
                            strSearch = "SELECT a.[applicationName] as 'Application Name',a.[transactionNames] as 'Transaction Name', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID1.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" +  ddlReleaseID1.SelectedValue.ToString() + "_SLA', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN TPS END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_TPS', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID1.SelectedValue.ToString()  + "' THEN TPS END)),'NA') '" + ddlReleaseID1.SelectedValue.ToString() + "_TPS' FROM [dbo].[NFRProTable] a ";
                        }
                    }
                    else
                    {
                        strSearch = "SELECT a.[applicationName] as 'Application Name',a.[transactionNames] as 'Transaction Name', ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "' THEN SLA END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_SLA',     ISNULL(str(MAX(CASE WHEN releaseID='" + ddlReleaseID.SelectedValue.ToString() + "'  THEN TPS END)),'NA') '" + ddlReleaseID.SelectedValue.ToString() + "_TPS' FROM [dbo].[NFRProTable] a ";

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
                        strSearch = strSearch + " AND [transactionNames] like '%' + @transactionNames + '%'";
                        cmd.Parameters.AddWithValue("transactionNames", textSearch);
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
                        strSearch = strSearch + " WHERE [transactionNames] like '%' + @transactionNames + '%'";
                        cmd.Parameters.AddWithValue("transactionNames", textSearch);
                    }
                }

              // lblError.Text = strSearch;

                DataTable dt = new DataTable();

                if (strSearch.Length < 1)
                {
                    strSearch = "SELECT * FROM [NFRProTable] where 1=2";
                    dt.Columns.Add("Application Name");
                    dt.Columns.Add("Transaction Name");
                    dt.Columns.Add("Release1_SLA");
                    dt.Columns.Add("Release1_TPS");
                    dt.Columns.Add("Release2_SLA");
                    dt.Columns.Add("Release2_TPS");
                    
                }
                else
                {
                    //"where a.[transactionNames] = 'OLB_Login' ";
                    strSearch = strSearch + "GROUP BY a.[applicationName],a.[transactionNames];";
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
        if(ddlReleaseID.SelectedIndex > 0)
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
        dt.Columns.Add("applicationName");
        dt.Columns.Add("transactionName");
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
        ddlApplicationName.SelectedIndex= 0;
        ddlReleaseID.SelectedIndex= -1;
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

    ////Added in 07/27
    ///

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
    //                //gv.BottomPagerRow.Visible = true;
    //                //tbl.Rows[0].Cells.Clear();
    //                //tbl.Width = Unit.Percentage(100);
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
