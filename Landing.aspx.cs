using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Landing : System.Web.UI.Page
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
        ddlReleaseID.Items.Insert(0, new ListItem("-Select Release-", "0"));

    }


    private void BindGrid1()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT ID, applicationName, releaseID, transactionNames, SLA, TPS, businessScenario, backendCall, callType FROM NFRProTable"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["gridviewsouce"] = dt;
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
        }
    }


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
                        strSearch = strSearch + " AND [transactionNames] like '%' + @transactionNames ";
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
                    GridView1.DataSource = new List<string>();
                    GridView1.DataBind();
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
}