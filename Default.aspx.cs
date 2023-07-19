﻿using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Xml.Linq;



//\r\n
//^(\s*\r\n){2,}

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindApplicationDropdown();

            this.BindGrid();
            //if (Session["CurrentUserRole"] == null)
            //{
            //    String CurrentUserName = Environment.UserName;
            //    String UserRole = CheckUserRole(CurrentUserName);
            //    //
            //    Session["CurrentUserRole"] = UserRole;
            //}

            if (Session["CurrentUserRole"].ToString() == "User")
            {
                int _editColumnIndex = 8;
                int _delColumnIndex = 9;
                GridView1.Columns[_editColumnIndex].Visible = false;
                GridView1.Columns[_delColumnIndex].Visible = false;
            }
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
        ddlReleaseID.Items.Insert(0, new ListItem("-Select Release-", "0"));

    }

    //Added on 07/19

    //private String CheckUserRole(String UserID)
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    string roleName = "User";
    //    using (SqlConnection con = new SqlConnection(constr))
    //    {
    //        using (SqlCommand cmd = new SqlCommand("SELECT distinct Roles FROM UserRoles WHERE UserId = @UserId"))
    //        {
    //            cmd.Parameters.AddWithValue("@UserId", UserID);
    //            cmd.Connection = con;
    //            SqlDataAdapter sda = new SqlDataAdapter(cmd);
    //            DataTable dt = new DataTable();
    //            sda.Fill(dt);
    //            con.Open();
    //            using (SqlDataReader dr = cmd.ExecuteReader())
    //            {
    //                while (dr.Read())
    //                {
    //                    roleName = dr[0].ToString();
    //                }
    //            }

    //        }
    //    }
    //    return roleName;
    //}

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        DataTable dt = Session["gridviewsouce"] as DataTable;
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
            using (SqlCommand cmd = new SqlCommand("DELETE FROM NFRProTable WHERE Id = @Id"))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        BindGrid();
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {

        GridView1.EditIndex = e.NewEditIndex;
        BindGrid();

    }

    //Changed on 07/19 for Row Edit Event
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
        //TextBox textadd = (TextBox)row.FindControl("txtadd");  
        //TextBox textc = (TextBox)row.FindControl("txtc");  
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE NFRProTable SET applicationName = @applicationName, releaseID = @releaseID , businessScenario = @businessScenario , transactionNames = @transactionNames  , SLA = @SLA  , TPS = @TPS  , backendCall = @backendCall  , callType = @callType  WHERE Id = @Id"))
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
        BindGrid();
       
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindGrid();
    }
    protected void ddlApplicationName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlReleaseID.Items.Clear();
        string query = string.Format("select distinct ReleaseID from NFRProTable where ApplicationName = '{0}'", ddlApplicationName.SelectedItem.Value);
        BindDropDownList(ddlReleaseID, query, "ReleaseID", "ReleaseID", "-Select ReleaseID-");
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
        ddl.Items.Insert(0, new ListItem("-All-", "0"));
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

    //Changed on 07/19
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
        try
        {
            if ((GridView1.DataSource as DataTable) == null)
            {
                Label3.Text = "Total Rows: 0";
            }
            else
            {
                iGridViewCount = (GridView1.DataSource as DataTable).Rows.Count;
                Label3.Text = "Total Rows: " + iGridViewCount;
            }

        }
        catch
        {
            Label3.Text = "Total Rows: 0";
        }

        //Change header text for Edit and Delete Columns

        if (e.Row.RowType == DataControlRowType.Header && iGridViewCount>0 && ddlApplicationName.SelectedIndex>0)
        {
            e.Row.Cells[8].Text = "Edit Record";
            e.Row.Cells[9].Text = "Delete Record";
        }


    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        resetGridView();
        BindGrid();
    }

    private void BindGrid()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            String strSearch;
            using (SqlCommand cmd = new SqlCommand())
            {
                if (ddlApplicationName.SelectedIndex > 0)
                {
                    strSearch = "SELECT * FROM [NFRProTable]";
                }
                else
                {
                    strSearch = "SELECT * FROM [NFRProTable] where 1=2";
                }
                if (ddlApplicationName.SelectedIndex > 0)
                {
                    strSearch = strSearch + "WHERE [applicationName]=@applicationName";
                    cmd.Parameters.AddWithValue("applicationName", this.ddlApplicationName.Text);

                    if (ddlReleaseID.SelectedIndex > 0)
                    {
                        strSearch = strSearch + " AND [releaseID]=@releaseID";
                        cmd.Parameters.AddWithValue("releaseID", this.ddlReleaseID.Text);
                    }

                    if (txtTransactionName.Text.Length > 0)
                    {
                        String textSearch;
                        textSearch = txtTransactionName.Text;
                        if (txtTransactionName.Text.EndsWith("*"))
                        {
                            textSearch = txtTransactionName.Text.Remove(txtTransactionName.Text.Length - 1);
                        }
                        strSearch = strSearch + " AND [transactionNames] like @transactionNames ";
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

                if (dt.Rows.Count == 0)
                {
                    resetGridView();
                    //this.Label1.Text = "No Data Found";
                    //return;
                }
                else
                {
                    Session["gridviewsouce"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }

    }

    protected void btnImportExcel_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlApplicationName.SelectedIndex = 0;
        ddlReleaseID.Items.Clear();
        txtTransactionName.Text = "";
        resetGridView();

    }

    //Written 07/19
    public void resetGridView()
    {
        GridView1.DataSource = new List<string>();
        GridView1.DataBind();
    }

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow pagerRow = (GridViewRow)gv.BottomPagerRow;

        if (pagerRow != null && pagerRow.Visible == false)
            pagerRow.Visible = true;
    }
}
