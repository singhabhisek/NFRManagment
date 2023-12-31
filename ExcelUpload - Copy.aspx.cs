﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExcelUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void Upload(object sender, EventArgs e)
    {
        String exceptions = "";

        int recordCount = 0;
        int blankFieldRecordCount = 0;
        int sqlExceptionRecordCount = 0;

        FileUpload_Msg.Text = "";

        if (FileUpload1.HasFile)
        {
            
            string excelPath = Server.MapPath("~/Files/F") + DateTime.UtcNow.ToString("HHmmss") + Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.SaveAs(excelPath);

            try
            {
                //String saveFolder = 
                //Upload and save the file
                string connExcelString = string.Empty;
                string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        connExcelString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        connExcelString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;
                    default:
                        exceptions = "Only xls and xlsx files are supported";
                        break;
                }
                connExcelString = string.Format(connExcelString, excelPath);
                using (OleDbConnection excel_con = new OleDbConnection(connExcelString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    DataTable dtExcelData = new DataTable();

                    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                    dtExcelData.Columns.AddRange(new DataColumn[8] { new DataColumn("applicationName", typeof(string)),
                new DataColumn("releaseID", typeof(string)),
                new DataColumn("businessScenario", typeof(string)),
                new DataColumn("transactionName", typeof(string)),
                new DataColumn("SLA", typeof(decimal)),
                new DataColumn("TPS", typeof(decimal)),
                new DataColumn("backendCall", typeof(string)),
                new DataColumn("callType", typeof(string)) });

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                    string connSqlString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                    var fieldIncrementor = 0;

                    /* check the heders for excel before processing */
                    excel_con.Open();
                    OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM [" + sheet1 + "]", excel_con);
                    OleDbDataReader reader1 = oleDbCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader1);

                    string[] stringArray = { "applicationName", "releaseID", "transactionName", "SLA", "TPS", "businessScenario", "backendCall", "callType" };
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        string columnName = column.ColumnName;
                        if (stringArray.Any(columnName.Contains))
                        {

                        }
                        else
                        {
                            exceptions += "'" + columnName + "' column in uploaded document does not match the template. Please validate from template above";
                            goto EndResult;
                        }

                    }
                    excel_con.Close();
                    /*end of header checks in excel*/
                    string query = "SELECT * FROM [" + sheet1 + "]";
                    using (OleDbConnection connection = new OleDbConnection(connExcelString))
                    {
                        OleDbCommand command = new OleDbCommand(query, connection);

                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            recordCount++;
                            using (SqlConnection connection1 = new SqlConnection(connSqlString))
                            {
                                String queryInsert = "INSERT INTO [dbo].[NFRDetails] ([applicationName],[releaseID],[businessScenario],[transactionName],[SLA],[TPS],[backendCall],[callType]) VALUES \r\n(@applicationName, @releaseID, @businessScenario, @transactionName, @SLA, @TPS, @backendCall, @callType) ";
                                using (SqlCommand CCC = new SqlCommand(queryInsert, connection1))
                                {
                                    try
                                    {
                                        if (reader[0].ToString() == "" || reader[1].ToString() == "" || reader[2].ToString() == "" || reader[3].ToString() == "")
                                        {
                                            blankFieldRecordCount++;
                                            exceptions += "<br/> Row# " + recordCount + ": One of the required values is blank or NULL in excel";
                                        }
                                        else
                                        {
                                            connection1.Open();
                                            CCC.CommandType = CommandType.Text;

                                            CCC.Parameters.Add("@applicationName", SqlDbType.VarChar, 255).Value = reader[0].ToString();
                                            CCC.Parameters.Add("@releaseID", SqlDbType.VarChar, 255).Value = reader[1].ToString();
                                            CCC.Parameters.Add("@businessScenario", SqlDbType.VarChar, 255).Value = reader[2].ToString();
                                            CCC.Parameters.Add("@transactionName", SqlDbType.VarChar, 255).Value = reader[3].ToString();
                                            CCC.Parameters.Add("@SLA", SqlDbType.Float, 10).Value = reader[4].ToString();
                                            CCC.Parameters.Add("@TPS", SqlDbType.Float, 100).Value = reader[5].ToString();
                                            CCC.Parameters.Add("@backendCall", SqlDbType.VarChar, 255).Value = reader[6].ToString();
                                            CCC.Parameters.Add("@callType", SqlDbType.VarChar, 255).Value = reader[7].ToString();

                                            CCC.ExecuteNonQuery();
                                        }
                                        //fieldIncrementor++;
                                    }
                                    catch (SqlException sqlEx)
                                    {

                                        sqlExceptionRecordCount++;
                                        if (sqlEx.Number == 2627)
                                        {
                                            int pFrom = sqlEx.Message.IndexOf("key value is"); // + "key value is".Length;
                                            int pTo = sqlEx.Message.LastIndexOf("statement") - 5;

                                            String result = sqlEx.Message.Substring(pFrom, pTo - pFrom);
                                            exceptions += "<br/> Row# " + recordCount + ": Duplicate " + result; 
                                        }
                                        else
                                        {
                                            exceptions += "<br/>" + sqlEx.Message.ToString();
                                        }//exceptions += "<br/>" + sqlEx.Message.ToString().Replace("PK_NFRDetails", "").Replace("dbo.NFRDetails", "");
                                    }
                                }
                                // }
                            }

                        }
                        reader.Close();
                    }
                    if (blankFieldRecordCount > 0 || sqlExceptionRecordCount > 0)
                    {
                        //FileUpload_Msg.Text
                        exceptions = "File processed. Number of records: " + recordCount.ToString() +
                                " <br/> Number or records successfully inserted:" + (recordCount - blankFieldRecordCount - sqlExceptionRecordCount).ToString() +
                                " <br/> Number or records skipped due to blank field error:" + blankFieldRecordCount.ToString() +
                                " <br/> Number or records skipped due to SQL error:" + sqlExceptionRecordCount.ToString() +
                                "<br/> Exceptions are: <br/>" + exceptions;
                    }
                    else
                    {
                        exceptions = "File processed. Number of records: " + recordCount.ToString() +
                                " <br/> Number or records successfully inserted:" + (recordCount - blankFieldRecordCount - sqlExceptionRecordCount).ToString();

                    }

                }
            }
            catch (Exception ex)
            {

                FileUpload_Msg.Text = exceptions.ToString() + "<br/>" + ex.Message.ToString();// "Error - Unable to save file. Please try again.";
            }
        EndResult: FileUpload_Msg.Text = exceptions.ToString();

            File.Delete(excelPath);
        }
        
        else
        {
            FileUpload_Msg.Text = "Error - No file chosen.";

        }
    }

}