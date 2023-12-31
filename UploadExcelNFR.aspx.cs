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

public partial class UploadExcelNFR : System.Web.UI.Page
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

            string excelPath = Server.MapPath("~/DownloadFile/F") + DateTime.UtcNow.ToString("HHmmss") + Path.GetFileName(FileUpload1.PostedFile.FileName);
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
                    dtExcelData.Columns.AddRange(new DataColumn[7] { new DataColumn("applicationName", typeof(string)),
                new DataColumn("releaseID", typeof(string)),
                new DataColumn("businessScenario", typeof(string)),
                new DataColumn("transactionName", typeof(string)),
                new DataColumn("SLA", typeof(decimal)),
                new DataColumn("TPS", typeof(decimal)),
                new DataColumn("Comments", typeof(string)) });

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                    string connSqlString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                    //var fieldIncrementor = 0;

                    /* check the heders for excel before processing */
                    excel_con.Open();
                    OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM [" + sheet1 + "]", excel_con);
                    OleDbDataReader reader1 = oleDbCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader1);

                    string[] stringArray = { "ApplicationName", "ReleaseID", "BusinessScenario", "TransactionName", "SLA", "TPS",  "Comments" };
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
                    string resultProcedure = "";
                    int insertCount = 0;
                    int updateCount = 0;
                    using (OleDbConnection connection = new OleDbConnection(connExcelString))
                    {
                        OleDbCommand command = new OleDbCommand(query, connection);

                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();

                        //start writing in database for insert or update

                        while (reader.Read())
                        {
                            recordCount++;
                            using (SqlConnection connection1 = new SqlConnection(connSqlString))
                            {
                                //String queryInsert = "INSERT INTO [dbo].[NFRDetails] ([applicationName],[releaseID],[businessScenario],[transactionName],[SLA],[TPS],[backendCall],[callType]) VALUES \r\n(@applicationName, @releaseID, @businessScenario, @transactionName, @SLA, @TPS, @backendCall, @callType) ";
                                using (SqlCommand cmd = new SqlCommand("NFRDetails_InsertUpdate", connection1))
                                {
                                    {
                                        try
                                        {
                                            
                                            if (reader[0].ToString() == "" || reader[1].ToString() == "" || reader[2].ToString() == "" || reader[3].ToString() == "")
                                            {
                                                blankFieldRecordCount++;
                                                exceptions += "<br/> Row# " + recordCount + ": Error - One of the required values is blank or NULL in excel - "  + reader[0].ToString() + "," + reader[1].ToString() + "," +  reader[2].ToString() + "," + reader[3].ToString();
                                            }
                                            else
                                            {
                                                connection1.Open();
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                String createdBy = "ABHISEK";
                                                String updatedBy = "ABHISEK";
                                                cmd.Parameters.Add("@applicationName", SqlDbType.VarChar, 255).Value = reader[0].ToString();
                                                cmd.Parameters.Add("@releaseID", SqlDbType.VarChar, 255).Value = reader[1].ToString();
                                                cmd.Parameters.Add("@businessScenario", SqlDbType.VarChar, 255).Value = reader[2].ToString();
                                                cmd.Parameters.Add("@transactionName", SqlDbType.VarChar, 255).Value = reader[3].ToString();
                                                cmd.Parameters.Add("@SLA", SqlDbType.Float, 10).Value = reader[4].ToString();
                                                cmd.Parameters.Add("@TPS", SqlDbType.Float, 100).Value = reader[5].ToString();
                                                cmd.Parameters.Add("@Comments", SqlDbType.VarChar, 255).Value = reader[6].ToString();
                                                cmd.Parameters.Add("@createdBy", SqlDbType.VarChar, 255).Value = createdBy;
                                                cmd.Parameters.Add("@modifiedBy", SqlDbType.VarChar, 255).Value = updatedBy;
                                                cmd.Parameters.Add("@retValue", SqlDbType.VarChar, 50);
                                                cmd.Parameters["@retValue"].Direction = ParameterDirection.Output;


                                                cmd.ExecuteNonQuery();

                                                resultProcedure = (string)cmd.Parameters["@retValue"].Value;

                                                if (resultProcedure == "INSERTED")
                                                {
                                                    insertCount++;
                                                }
                                                else if (resultProcedure == "UPDATED")
                                                {
                                                    updateCount++;
                                                }
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
                                                exceptions += "<br/> Row# " + recordCount + ": Duplicate - " + result;
                                            }
                                            else
                                            {
                                                exceptions += "<br/> Row# " + recordCount + "<br/>" + sqlEx.Message.ToString();
                                            }//exceptions += "<br/>" + sqlEx.Message.ToString().Replace("PK_NFRDetails", "").Replace("dbo.NFRDetails", "");
                                        }
                                        catch (Exception ex)
                                        {
                                            sqlExceptionRecordCount++;
                                            exceptions += "<br/> Row# " + recordCount + ": Error - " + ex.Message.ToString();
                                        }

                                    }
                                    // }
                                }

                            }

                        }
                        reader.Close();
                        if (blankFieldRecordCount > 0 || sqlExceptionRecordCount > 0)
                        {
                            //FileUpload_Msg.Text
                            exceptions = "File processed. Number of records: " + recordCount.ToString() +
                                    " <br/> Number or records successfully inserted:" + (insertCount).ToString() +
                                    " <br/> Number or records successfully updated:" + (updateCount).ToString() +
                                    " <br/> Number or records skipped due to blank field error:" + blankFieldRecordCount.ToString() +
                                    " <br/> Number or records skipped due to SQL error:" + sqlExceptionRecordCount.ToString() +
                                    "<br/> Exceptions are: <br/>" + exceptions;
                        }
                        else
                        {
                            exceptions = "File processed. Number of records: " + recordCount.ToString() +
                                    " <br/> Number or records successfully inserted:" + (insertCount).ToString() +
                                    " <br/> Number or records successfully updated:" + (updateCount).ToString();
                        }

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