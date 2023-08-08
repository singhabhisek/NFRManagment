using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonLibraryFunctions.CommonMethods;

public partial class Default4 : System.Web.UI.Page
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
        
        retrieveStack();
    }

   

    void retrieveStack()
    {
        HttpCookie cookieObj = Request.Cookies["mru"];
        //--- Check for null 
        if (cookieObj != null)
        {
            string[] myArray = cookieObj.Value.Split(',');
            String result1 = "";
            ss.Text = myArray[0].ToString();
            String result="";
            for (int i = 0; i < myArray.Length; i++)
            {
                string[] myArray2 = myArray[i].Split('&');
                for (int j = 0; j < myArray2.Length; j++)
                {
                    switch (j)
                    {
                        case 0:
                            result = "Selected application=" + ddlApplicationName.Items[int.Parse(myArray2[j])].Value;
                            break;
                        case 1:
                            if (int.Parse(myArray2[j]) > 0)
                            {
                                result = "ReleaseName=" + ddlReleaseID.Items[int.Parse(myArray2[j])].Value;
                            }
                            break;

                        case 2:
                            result = "TrxName=" + myArray2[j];
                            break;

                    }

                    result1 = result1 + ";" + result;
                    result = "";
                }

                ListBox1.Items.Add(result1.Substring(1));
                AddLinkURL(result1.Substring(1), result1.Substring(1));
                
                result1 = "";
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
        String[] finalResult = null;
        finalResult = h.Text.Split(';');
        for (int j = 0; j < finalResult.Length; j++)
        {
            switch(finalResult[j].Split('=')[0]){
                case "Selected application":
                    ddlApplicationName.Items.FindByText(finalResult[j].Split('=')[1]).Selected = true;
                    break;
                case "ReleaseName":
                    ddlReleaseID.Items.FindByText(finalResult[j].Split('=')[1]).Selected = true;
                    break;
                case "TrxName":
                    txtTransactionName.Text = finalResult[j].Split('=')[1];
                    break;
            }
        }
        //;Selected application=Text13;ReleaseName=Text22;TrxName=ss


    }


    void retrieveCookie()
    {
        HttpCookie cookieObj = Request.Cookies["NameOfCookie"];

        //--- Check for null 
        if (cookieObj != null)
        {

            ddlApplicationName.ClearSelection();
            ddlReleaseID.ClearSelection();


            //--- To read values from cookie collection we will use Keys used while creating cookie.
            string applicationName = cookieObj["ApplicationName"];
            string releaseID = cookieObj["ReleaseID"];
            string transactionName = cookieObj["TransactionName"];

            ddlApplicationName.Items.FindByValue(applicationName).Selected = true;
            ddlReleaseID.Items.FindByValue(releaseID).Selected = true;
            txtTransactionName.Text = transactionName;

            string result = "RETRIEVE: Saved ApplicationName " + applicationName + " with ReleaseID is " + releaseID;
            ss.Text = result;
        }
    }
    void SaveCookie()
    {
       
        //--- Create Cookie Object.
        HttpCookie cookieObject = new HttpCookie("NameOfCookie");

        //--- Add values to cookie in Key,Value format.
        cookieObject["ApplicationName"] = ddlApplicationName.SelectedValue.ToString();
        cookieObject["ReleaseID"] = ddlReleaseID.SelectedValue.ToString();
        cookieObject["TransactionName"] = txtTransactionName.Text.ToString();

        //---- Set expiry time of cookie.
        cookieObject.Expires.AddDays(3);

        //---- Add cookie to cookie collection.
        Response.Cookies.Add(cookieObject);
    }

    protected void save_Click(object sender, EventArgs e)
    {
        
        addCookiesInStack("mru",ddlApplicationName.SelectedIndex + "&" + ddlReleaseID.SelectedIndex + "&" + txtTransactionName.Text);
        //SaveCookie();
        ddlApplicationName.SelectedIndex = -1;
        ddlReleaseID.SelectedIndex= -1;
        txtTransactionName.Text = "";
       // retrieveStack();
    }

    protected void retrieve_Click(object sender, EventArgs e)
    {
        //enable = true;

       // retrieveStack();
       // retrieveCookie();
    }

    //void createLinkButton()
    //{
    //    for (int i = 1; i <= 5; i++)
    //    {
    //        LinkButton linkButton = new LinkButton();
    //        linkButton.ID = "linkButton" + i.ToString();
    //        linkButton.Text = "Link Button " + i.ToString();
    //        linkButton.Click += new EventHandler(linkButton_Click);

    //        // Add the link button to the placeholder control on your page
    //        upd1.ContentTemplateContainer.Controls.Add(phLinks);
    //        phLinks.Controls.Add(linkButton);
    //    }
    //}

    //protected void linkButton_Click(object sender, EventArgs e)
    //{
    //    // Handle the click event here
    //    // You can identify which link button was clicked using the ID property
    //    LinkButton clickedButton = (LinkButton)sender;
    //    string buttonID = clickedButton.ID;
    //    // Perform any desired action based on the clicked button
    //}


    protected void delete_Click(object sender, EventArgs e)
    {
        if (Request.Cookies["mruDashboard"] != null)
        {
            Response.Cookies["mruDashboard"].Expires = DateTime.Now.AddDays(-1);
        }
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ss.Text=ListBox1.SelectedValue.ToString();
    }
}