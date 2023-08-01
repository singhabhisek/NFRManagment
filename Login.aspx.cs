
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("select UserName, Password from credentials where UserName=@username and Password=@password", con);
        cmd.Parameters.AddWithValue("@username", txtUserName.Text);
        cmd.Parameters.AddWithValue("@password", txtPwd.Text);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        con.Open();
        int i = cmd.ExecuteNonQuery();
        con.Close();

        if (dt.Rows.Count > 0)
        {

            //Messagebox("Hello Login");
            //string url = "UserAdministration.aspx";

            //string s = "self.close();window.open('" + url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=no');";

            //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            //Label1.Text = "";

            Response.Redirect("UserAdministration.aspx");
            Session["authenticate"] = "TRUE";
           

        }
        else
        {
            //Messagebox(" Login failed ");
            Label1.Text = "Your username and password is incorrect";
            Label1.ForeColor = System.Drawing.Color.Red;

        }

    }

}