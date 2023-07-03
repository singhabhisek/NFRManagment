using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sample : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void aa_Click(object sender, EventArgs e)
    {
        lbl.Text=txt1.Text + floatingSelect.SelectedValue.ToString();
    }
}