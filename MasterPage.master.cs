using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //lblUser.Text = "Hallo : " + System.Security.Principal.WindowsIdentity.GetCurrent().User.Translate(typeof(System.Security.Principal.NTAccount));
        

        if (Session["CurrentUserRole"] == null)
        {
            String CurrentUserName = Environment.UserName;
            String UserRole = CheckUserRole(CurrentUserName);
            //
            Session["CurrentUserRole"] = UserRole;

        }

        if (Session["CurrentUserRole"].ToString() == "User")
        {
            div1.Visible = false;
        }
        else
        {
            div1.Visible = true;
        }

        lblUser.Text = "Hallo : " + Session["CurrentUserRole"].ToString();

    }

    private String CheckUserRole(String UserID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        string roleName = "User";
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT distinct Roles FROM UserRoles WHERE UserId = @UserId"))
            {
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.Connection = con;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        roleName = dr[0].ToString();
                    }
                }

            }
        }
        return roleName;
    }

}
