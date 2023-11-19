using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

public partial class sample : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        BindChart();
        BindChartJS();
    }

    private void BindChart()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; // Replace with your actual connection string

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query  = "SELECT ApplicationName, TransactionName, ReleaseID, SLA, TPS " +
               "FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY ApplicationName, TransactionName ORDER BY ReleaseID DESC) as RowNum " +
               "      FROM NFRDetails where ApplicationName = 'OLB') AS Ranked " +
               "WHERE RowNum <= 5";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string applicationName = reader["ApplicationName"].ToString();
                        string transactionName = reader["TransactionName"].ToString();
                        string releaseID = reader["ReleaseID"].ToString();
                        double sla = Convert.ToDouble(reader["SLA"]);
                        double tps = Convert.ToDouble(reader["TPS"]);

                        Series seriesSLA = GetOrCreateSeries(Chart1, applicationName, transactionName + " - SLA", SeriesChartType.Line);
                        // Series seriesTPS = GetOrCreateSeries(Chart1, applicationName, transactionName + " - TPS", SeriesChartType.Line);

                        // Add data points to the chart
                        DataPoint dataPointSLA = new DataPoint();
                        dataPointSLA.SetValueXY(reader["ReleaseID"].ToString(), sla);
                        dataPointSLA.ToolTip = $"{transactionName} - SLA: {sla}";
                        seriesSLA.Points.Add(dataPointSLA);

                        //seriesSLA.Points.AddXY(releaseID, sla);
                       // seriesTPS.Points.AddXY(releaseID, tps);
                    }
                }
            }
        }

        // Customize chart appearance
        foreach (Series series in Chart1.Series)
        {
            series.ToolTip = "#VALY"; // Show values in tooltip
            series.Legend = "Legend1"; // Assign to legend
        }

        // Set X and Y axis labels
        Chart1.ChartAreas[0].AxisX.Title = "Release ID";
        Chart1.ChartAreas[0].AxisY.Title = "Value";

        // Set chart title
        Chart1.Titles[0].Text = "NFR Trends";
    }

    private Series GetOrCreateSeries(Chart chart, string applicationName, string seriesName, SeriesChartType chartType)
    {
        Series series = chart.Series.FindByName(seriesName);

        if (series == null)
        {
            series = new Series(seriesName);
            series.ChartType = chartType;
            series.BorderWidth = 2;
            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 8;

            chart.Series.Add(series);
        }

        return series;
    }


    private void BindChartJS()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; // Replace with your actual connection string

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT ApplicationName, TransactionName, ReleaseID, SLA, TPS " +
               "FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY ApplicationName, TransactionName ORDER BY ReleaseID DESC) as RowNum " +
               "      FROM NFRDetails where ApplicationName = 'OLB') AS Ranked " +
               "WHERE RowNum <= 5";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<string> labels = new List<string>();
                    List<double> slaData = new List<double>();
                    List<double> tpsData = new List<double>();
                    List<string> transactionNames = new List<string>();

                    while (reader.Read())
                    {
                        labels.Add(reader["ReleaseID"].ToString());
                        slaData.Add(Convert.ToDouble(reader["SLA"]));
                        tpsData.Add(Convert.ToDouble(reader["TPS"]));
                        transactionNames.Add(reader["TransactionName"].ToString());

                    }

                    // Serialize data to JSON for JavaScript consumption
                    string labelsJson = new JavaScriptSerializer().Serialize(labels);
                    string slaDataJson = new JavaScriptSerializer().Serialize(slaData);
                    string tpsDataJson = new JavaScriptSerializer().Serialize(tpsData);
                    string transactionNamesJson = new JavaScriptSerializer().Serialize(transactionNames);

                    // Register client-side script to initialize Chart.js
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "InitializeChart", $@"
                            var ctx = document.getElementById('myChart').getContext('2d');
                            var myChart = new Chart(ctx, {{
                                type: 'bar',
                                data: {{
                                    labels: {labelsJson},
                                    datasets: [
                                        {{
                                            label: 'SLA',
                                            data: {slaDataJson},
                                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                            borderColor: 'rgba(75, 192, 192, 1)',
                                            borderWidth: 1
                                        }}
                                    ]
                                }},
                                options: {{
                                    scales: {{
                                        y: {{
                                            beginAtZero: true
                                        }}
                                    }},
                                tooltips: {{
                                    mode: 'index',
                                    intersect: false,
                                    callbacks: {{
                                        label: function (tooltipItem, data) {{
                                            var index = tooltipItem.index;
                                            return 'Transaction Name: ' + {transactionNamesJson}[index] + ', SLA: ' + tooltipItem.yLabel;
                                        }}
                                    }}
                                }}
                                }}
                            }});
                        ", true);
                }
            }
        }
    }
}

    
