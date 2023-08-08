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
using System.Web.Services;
using System.Web.SessionState;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Runtime.Remoting.Contexts;

namespace CommonLibraryFunctions
{

    /// <summary>
    /// Summary description for CommonMethods
    /// </summary>
    public class CommonMethods
    {
        public static int totalRowCount = 0;
        public static int pageSize = 5;
        const int listSize = 5;
        public static string[] _mru;

        public static void resetGridView(GridView gridView)
        {
            gridView.DataSource = new List<string>();
            gridView.DataBind();
        }


        public static DataSet GetData(string query)
        {
            string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlCommand cmd = new SqlCommand(query);
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        //Method to get the transaction nales based on application and release. Stored in memory for a single call to DB.
        public static void FillTransactionNameDT(String ApplicationName, String ReleaseID, String TrxViewName)
        {
            //Add to dtTrxNames
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT DISTINCT transactionNames FROM NFRDetails where " +
                    "applicationName = @applicationName";
                    cmd.Parameters.AddWithValue("@applicationName", ApplicationName);
                    if (!string.IsNullOrEmpty(ReleaseID))
                    {
                        cmd.CommandText += " AND releaseID = @releaseID";
                        cmd.Parameters.AddWithValue("@releaseID", ReleaseID);
                    }
                    //cmd.Parameters.AddWithValue("@SearchText", prefixText);

                    //Open the connection and execute
                    cmd.Connection = conn;
                    conn.Open();
                    DataTable dtTrxNames = new DataTable();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dtTrxNames);

                        //Store datatable in memory for frequent use to avoid multiple database calls
                        HttpContext.Current.Session[TrxViewName] = dtTrxNames;
                    }

                    //Close Connection
                    conn.Close();
                }

            }
        }

        //Method to get the transaction Coutns for a given application. Based on this user will be notified for addtional search parameters
        public static int returnCountFromTables(DropDownList ddlObjectName)
        {
            int count = 0;

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection thisConnection = new SqlConnection(constr))
            {
                String strSearch;
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (ddlObjectName.SelectedIndex > 0)
                    {
                        strSearch = "SELECT count(*) FROM [NFRDetails]";
                    }
                    else
                    {
                        strSearch = "SELECT * FROM [NFRDetails] where 1=2";
                    }
                    if (ddlObjectName.SelectedIndex > 0)
                    {
                        strSearch = strSearch + " WHERE [applicationName]=@applicationName";
                        cmd.Parameters.AddWithValue("applicationName", ddlObjectName.Text);

                    }

                    thisConnection.Open();
                    cmd.CommandText = strSearch;
                    //create parameters with specified name and values
                    cmd.Connection = thisConnection;

                    count = (int)cmd.ExecuteScalar();

                    thisConnection.Close();
                }
            }
            return count;
        }



        public static void ExportGridToExcel(GridView gridView)
        {
            string FileName = "ExportExcel_" + DateTime.Now + ".xls";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gridView.AllowPaging = false;
                //BindGrid(gridView);

                gridView.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gridView.HeaderRow.Cells)
                {
                    cell.BackColor = gridView.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gridView.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gridView.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gridView.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gridView.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }

        }

        public static int LevenshteinDistance(string s, string t)
        {
            // Special cases
            if (s == t) return 0;
            if (s.Length == 0) return t.Length;
            if (t.Length == 0) return s.Length;
            // Initialize the distance matrix
            int[,] distance = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i <= s.Length; i++) distance[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) distance[0, j] = j;
            // Calculate the distance
            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }
            // Return the distance
            return distance[s.Length, t.Length];
        }

        /////
        ///


        public static void SetFixedHeightForGridIfRowsAreLess(GridView gv)
        {
            double headerFooterHeight = gv.HeaderStyle.Height.Value + 35; //we set header height style=35px and there no footer  height so assume footer also same
            double rowHeight = gv.RowStyle.Height.Value;
            int gridRowCount = gv.Rows.Count;
            if (gridRowCount <= gv.PageSize)
            {
                double height = (gridRowCount * rowHeight) + ((gv.PageSize - gridRowCount) * rowHeight) + headerFooterHeight;
                //adjust footer height based on white space removal between footer and last row
                height += 40;
                gv.Height = new Unit(height);
            }
        }

        public static void createPagingSummaryOnPagerTemplate(object sender, int totalCount, int pageSize)
        {
            GridView gv = sender as GridView;
            if (gv != null)
            {
                //Get Bottom Pager Row from a gridview
                GridViewRow row = gv.BottomPagerRow;

                if (row != null)
                {
                    //create new cell to add to page strip
                    TableCell pagingSummaryCell = new TableCell();
                    pagingSummaryCell.Text = DisplayCusotmPagingSummary(totalCount, gv.PageIndex, pageSize);
                    pagingSummaryCell.HorizontalAlign = HorizontalAlign.Right;
                    pagingSummaryCell.VerticalAlign = VerticalAlign.Middle;
                    pagingSummaryCell.Width = Unit.Percentage(100);
                    pagingSummaryCell.Height = Unit.Pixel(35);
                    //Getting table which shows PagingStrip
                    Table tbl = (Table)row.Cells[0].Controls[0];

                    gv.BottomPagerRow.Visible = true;
                    //BottomPagerRow will be visible false if pager doesn't have numbers and page number 1 will be displayed
                    //if (totalCount <= pageSize)
                    //{
                    //gv.BottomPagerRow.Visible = true;
                    //tbl.Rows[0].Cells.Clear();
                    //tbl.Width = Unit.Percentage(100);
                    //}
                    //Find table and add paging summary text
                    tbl.Rows[0].Cells.Add(pagingSummaryCell);
                    //assign header row color to footer row
                    //tbl.BackColor = Color.Red; // System.Drawing.ColorTranslator.FromHtml("#1AD9F2");
                    tbl.Width = Unit.Percentage(100);





                }
            }
        }

        public static string DisplayCusotmPagingSummary(int numberOfRecords, int currentPage, int pageSize)
        {
            StringBuilder strDisplaySummary = new StringBuilder();
            int numberOfPages;
            if (numberOfRecords > pageSize)
            {
                // Calculating the total number of pages
                numberOfPages = (int)Math.Ceiling((double)numberOfRecords / (double)pageSize);
            }
            else
            {
                numberOfPages = 1;
            }
            strDisplaySummary.Append("Showing ");
            int floor = (currentPage * pageSize) + 1;
            strDisplaySummary.Append(floor.ToString());
            strDisplaySummary.Append("-");
            int ceil = ((currentPage * pageSize) + pageSize);

            if (ceil > numberOfRecords)
            {
                strDisplaySummary.Append(numberOfRecords.ToString());
            }
            else
            {
                strDisplaySummary.Append(ceil.ToString());
            }

            strDisplaySummary.Append(" of ");
            strDisplaySummary.Append(numberOfRecords.ToString());
            strDisplaySummary.Append(" results ");
            return strDisplaySummary.ToString();
        }

        public static void addCookiesInStack(String cookieName, String CheckStringInCookie)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            Queue<string> mru;
            if (cookie != null)
            {
                mru = new Queue<string>(cookie.Value.Split(','));
            }
            else
            {
                mru = new Queue<string>();
                cookie = new HttpCookie(cookieName);
            }
            String seletedValuesONScreen = CheckStringInCookie; // ddlApplicationName.SelectedIndex + "&" + ddlReleaseID.SelectedIndex + "&"  + txtTransactionName.Text;

            if (!mru.Contains(seletedValuesONScreen))
            {
                if (mru.Count >= listSize) mru.Dequeue();

                mru.Enqueue(seletedValuesONScreen);
            }

            _mru = mru.ToArray();

            cookie.Value = String.Join(",", _mru);
            cookie.Expires = DateTime.Now.AddDays(3);

            try
            {
                var guid = Guid.NewGuid();
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(guid.ToString(), string.Empty));
                HttpContext.Current.Response.Cookies.Remove(guid.ToString());

            }
            catch (HttpException)
            {
                //This means the headers were already written,
                //in which case we need not do anything.
            }

        }
    }
}