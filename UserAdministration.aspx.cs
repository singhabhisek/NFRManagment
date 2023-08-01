
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonLibraryFunctions.CommonMethods;

public partial class UserAdministration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["authenticate"] != null)
        {
            if (Session["authenticate"].ToString() == "TRUE")
            {


                if (!IsPostBack)
                {
                    ddlRoles.DataSource = fillDropdownlist();
                    ddlRoles.DataTextField = "Roles";
                    ddlRoles.DataValueField = "Roles";
                    ddlRoles.DataBind();
                    this.BindGrid();
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }

    }

    protected void Insert(object sender, EventArgs e)
    {
        string UserID = txtUserID.Text;
        string Roles = ddlRoles.SelectedValue.ToString();

        txtUserID.Text = "";
        ddlRoles.SelectedIndex = -1;

        string query = "INSERT INTO UserRoles VALUES(@UserID, @Roles)";
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        this.BindGrid();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
        {
            //  string item = e.Row.Cells[1].Text;
            string item = ((Label)e.Row.FindControl("lblUserID")).Text;
            foreach (LinkButton button in e.Row.Cells[2].Controls.OfType<LinkButton>())
            {
                if (button.CommandName == "Delete")
                {

                    button.Attributes["onclick"] = "if(!confirm('Do you want to delete - " + item + "?')){ return false; };";
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex == e.Row.RowIndex)
        {
            DropDownList ddlRoles = (DropDownList)e.Row.FindControl("ddlRoles");
            ddlRoles.DataSource = fillDropdownlist();
            ddlRoles.DataTextField = "Roles";
            ddlRoles.DataValueField = "Roles";
            ddlRoles.DataBind();

            //Add Default Item in the DropDownList.
            //ddlRoles.Items.Insert(0, new ListItem("Please select"));

            //Select the Country of Customer in DropDownList.
            string selectedRole = DataBinder.Eval(e.Row.DataItem, "Roles").ToString().ToUpper();
            ddlRoles.Items.FindByValue(selectedRole).Selected = true;
        }
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditRowStyle.BackColor = System.Drawing.Color.LightYellow;

        GridView1.EditIndex = e.NewEditIndex;
        this.BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = GridView1.Rows[e.RowIndex];
        string UserID = (row.FindControl("txtUserId") as TextBox).Text;
        string Roles = (row.FindControl("ddlRoles") as DropDownList).SelectedValue.ToString();
        string query = "UPDATE UserRoles SET Roles=@Roles WHERE UserID=@UserID";
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {




        string UserId = GridView1.DataKeys[e.RowIndex].Values[0].ToString();
        string query = "DELETE FROM UserRoles WHERE UserID=@UserId";
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        this.BindGrid();

    }

    private void BindGrid()
    {

        GridView1.DataSource = GetData("SELECT UserID, Roles FROM UserRoles");
        totalRowCount = (GridView1.DataSource as DataSet).Tables[0].Rows.Count;

        GridView1.DataBind();
    }

    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        this.BindGrid();
    }

    public DataTable fillDropdownlist()
    {
        ddlRoles.Items.Clear();

        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn("Roles", typeof(String));
        dt.Columns.Add(dc);
        dt.Rows.Add("ADMIN");
        dt.Rows.Add("POWERUSR");
        dt.Rows.Add("USERS");

        return dt;
    }


    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow pagerRow = (GridViewRow)gv.BottomPagerRow;
        if (pagerRow != null)
            pagerRow.Visible = true;
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


}