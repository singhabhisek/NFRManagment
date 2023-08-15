using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonLibraryFunctions.CommonMethods;

public partial class Default5 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {



            string query = "select distinct [applicationName] from NFRDetails";
            BindDropDownList(ddlApplicationName, query, "applicationName", "applicationName", "-Select Application-");
            resetGridView(Gridview1);

        }
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

    string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //Method for DataBinding  
    protected void ShowData()
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(cs);
        // SqlDataAdapter adapt = new SqlDataAdapter("select ApplicationName, releaseID, transactionName, SLA, IsNull(TotalSyncSLA,0), IsNull(MaxAsyncSLA,0), backendCall, CASE WHEN IsNull(TotalSyncSLA,0) + IsNull(MaxAsyncSLA,0) = 0 then 'NA' ELSE CASE WHEN SLA > TotalSyncSLA + MaxAsyncSLA then 'Higher' else 'Lower' END END as 'Compare' FROM (SELECT ApplicationName, transactionName, releaseID, SLA, backendCall, SUM( CASE WHEN CallType = 'Sync' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY transactionName) AS TotalSyncSLA, MAX( CASE WHEN CallType = 'Async' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY transactionName) AS MaxAsyncSLA FROM ( SELECT ApplicationName, transactionName, backendCall, CallType, SLA, releaseID, CASE WHEN CallType = 'Async' THEN ( SELECT MAX(SLA) FROM NFRDetails WHERE transactionName = t.backendCall AND t.CallType = 'Async' and releaseID= '" + ddlReleaseID.SelectedValue + "') WHEN CallType = 'Sync' THEN ( SELECT SUM(SLA) FROM NFRDetails WHERE transactionName = t.backendCall AND t.CallType = 'Sync' and releaseID= '" + ddlReleaseID.SelectedValue + "') ELSE 0 END AS SLAComparison FROM NFRDetails t where ApplicationName = '" + ddlApplicationName.SelectedValue + "' and releaseID= '" + ddlReleaseID.SelectedValue + "' ) as x ) as p;", con);
        SqlDataAdapter adapt = new SqlDataAdapter("with NFRDetailDepend as (select t.applicationName, t.transactionName, t.releaseID, t.SLA, d.backendCall, d.callType from NFRDetails t, NFROperationDependency d where t.transactionName = d.transactionName) select ApplicationName, releaseID, transactionName, SLA, IsNull(TotalSyncSLA,0) + IsNull(MaxAsyncSLA,0) as 'TotalBackendCallDuration', backendCall, CASE WHEN IsNull(TotalSyncSLA,0) + IsNull(MaxAsyncSLA,0) = 0 then 'NA' ELSE CASE WHEN SLA > TotalSyncSLA + MaxAsyncSLA then 'Higher' else 'Lower' END END as 'Compare' FROM (SELECT ApplicationName, transactionName, releaseID, SLA, backendCall, SUM( CASE WHEN CallType = 'Sync' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY transactionName) AS TotalSyncSLA, MAX( CASE WHEN CallType = 'Async' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY transactionName) AS MaxAsyncSLA FROM ( SELECT ApplicationName, transactionName, backendCall, CallType, SLA, releaseID, CASE WHEN CallType = 'Async' THEN ( SELECT MAX(SLA) FROM NFRDetailDepend WHERE transactionName = t.backendCall AND t.CallType = 'Async') WHEN CallType = 'Sync' THEN ( SELECT SUM(SLA) FROM NFRDetailDepend WHERE transactionName = t.backendCall AND t.CallType = 'Sync' ) ELSE 0 END AS SLAComparison FROM NFRDetailDepend t where ApplicationName = '" + ddlApplicationName.SelectedValue + "' and releaseID= '" + ddlReleaseID.SelectedValue + "' ) as x ) as p;", con); con.Open();
        adapt.Fill(dt);
        con.Close();
        if (dt.Rows.Count > 0)
        {

            DataTable combinedTable = new DataTable();
            combinedTable.Columns.Add("ApplicationName", typeof(string));
            combinedTable.Columns.Add("releaseID", typeof(string));
            combinedTable.Columns.Add("transactionName", typeof(string));
            combinedTable.Columns.Add("SLA", typeof(Double));
            combinedTable.Columns.Add("backendCall", typeof(string));
            combinedTable.Columns.Add("Compare", typeof(string));


            

            // Assuming you have an existing DataTable called "originalDataTable"

            // Create a grouping to group rows based on the combination of columns
            var groups = dt.AsEnumerable()
                .GroupBy(row => new
                {
                    ApplicationName = row.Field<string>("ApplicationName"),
                    ReleaseID = row.Field<string>("ReleaseID"),
                    transactionName = row.Field<string>("transactionName"),
                    SLA = row.Field<Double>("SLA"),
                    Compare = row.Field<string>("Compare"),
                    TotalBackendCallDuration = row.Field<Double>("TotalBackendCallDuration")
                });

            // Iterate through each group and concatenate the BackendCall values
            foreach (var group in groups)
            {
                var backendCalls = group.Select(row => row.Field<string>("BackendCall"))
                                        .Where(call => !string.IsNullOrEmpty(call));

                

                // Combine the multiple backend call values into a string
                string concatenatedBackendCalls = "<b>Backend Duration (sec)</b>: " + group.Key.TotalBackendCallDuration + "<br><b>Backend Calls:</b> <br>";
                    concatenatedBackendCalls +=  string.Join(",<br>", backendCalls);

                // Add a new row to the newDataTable with the combined values
                combinedTable.Rows.Add(
                    group.Key.ApplicationName,
                    group.Key.ReleaseID,
                    group.Key.transactionName,
                    group.Key.SLA,
                    concatenatedBackendCalls,
                    group.Key.Compare
                );
            }

            totalRowCount = combinedTable.Rows.Count;

            Gridview1.DataSource = combinedTable;
            Gridview1.DataBind();
        }
    }

    protected void ddlApplicationName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlReleaseID.Items.Clear();
       

        //Create Searach string and bind results to ReleaseID Dropdown
        string query = string.Format("select distinct ReleaseID from NFRDetails where ApplicationName = '{0}'", ddlApplicationName.SelectedItem.Value);
        BindDropDownList(ddlReleaseID, query, "ReleaseID", "ReleaseID", "-Select ReleaseID-");

    }

    protected void ddlReleaseID_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowData();
    }

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow pagerRow = (GridViewRow)gv.BottomPagerRow;

        if (pagerRow != null && pagerRow.Visible == false)
            pagerRow.Visible = true;
    }

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        createPagingSummaryOnPagerTemplate(sender, totalRowCount, pageSize);
    }


    protected void Gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the value of the "Status" column for the current row
            string status = DataBinder.Eval(e.Row.DataItem, "Compare").ToString();

            // Find the Image control in the "Icon" column
            Image imgStatus = (Image)e.Row.FindControl("imgStatus");

            // Set the image URL based on the "Status" column value
            if (status == "Higher")
            {
                imgStatus.ImageUrl = "Resources/images/uparrow.png";
            }
            else if (status == "Lower")
            {
                imgStatus.ImageUrl = "Resources/images/downarrow.png";
            }
            else if (status == "NA")
            {
                imgStatus.ImageUrl = "Resources/images/nochange.png";
            }
        }

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[4].ToolTip = "Shows higher or lower than backend calls";

        //    // Get the Label and Panel controls
        //    //Label lblMouseover = (Label)e.Row.FindControl("lblMouseover");
        //    //Panel pnlDetails = (Panel)e.Row.FindControl("pnlDetails");

        //    //// Set the client-side script for mouseover and mouseout events
        //    //if (lblMouseover != null && pnlDetails != null)
        //    //{
        //    //    lblMouseover.Attributes["onmouseover"] = $"ShowDetails(this, '{pnlDetails.ClientID}');";
        //    //    lblMouseover.Attributes["onmouseout"] = $"HideDetails(this, '{pnlDetails.ClientID}');";
        //    //}
        //}
    }
}