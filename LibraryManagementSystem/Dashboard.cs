using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace LibraryManagementSystem
{
    public partial class Dashboard : UserControl
    {
        SqlConnection connect = new SqlConnection(ConnectionString.getConnection);

        public Dashboard()
        {

            InitializeComponent();

            displayAB();
            displayIB();
            displayRB();
            LoadChartData();

        }

        public void refreshData()
        {

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }

            displayAB();
            displayIB();
            displayRB();
            LoadChartData();
        }

        public void displayAB()
        {
            if (connect.State == ConnectionState.Closed)
            {
                try
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) FROM books " +
                        "WHERE status = 'Available' AND date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        int tempAB = 0;

                        if (reader.Read())
                        {
                            tempAB = Convert.ToInt32(reader[0]);

                            dashboard_AB.Text = tempAB.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        public void displayIB()
        {
            if (connect.State == ConnectionState.Closed)
            {
                try
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) FROM issues " +
                        "WHERE date_delete IS NULL ";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        int tempIB = 0;

                        if (reader.Read())
                        {
                            tempIB = Convert.ToInt32(reader[0]);

                            dashboard_IB.Text = tempIB.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        public void displayRB()
        {
            if (connect.State == ConnectionState.Closed)
            {
                try
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) FROM issues " +
                        " WHERE status = 'Return' AND date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        int tempRB = 0;

                        if (reader.Read())
                        {
                            tempRB = Convert.ToInt32(reader[0]);

                            dashboard_RB.Text = tempRB.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            
            displayAB();
            displayIB();
            displayRB();
            LoadChartData();


        }

        private void LoadChartData()
        {
            if (connect.State == ConnectionState.Closed)
            {
                try
                {
                    connect.Open();

                    string selectData = @"
                SELECT 
                    FORMAT(issue_date, 'dd/MM/yyyy') AS IssueDate, 
                    COUNT(id) AS TotalIssues
                FROM 
                    issues
                WHERE 
                    date_delete IS NULL
                GROUP BY 
                    FORMAT(issue_date, 'dd/MM/yyyy')
                ORDER BY 
                    IssueDate";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

           
                        chart1.Series["Book"].Points.Clear();

                        while (reader.Read())
                        {
              
                            string issueDate = reader["IssueDate"].ToString();
                            int totalIssues = Convert.ToInt32(reader["TotalIssues"]);

                            // Add the data points to the chart
                            chart1.Series["Book"].Points.AddXY(issueDate, totalIssues);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }


        private void btnRemove_Click(object sender, EventArgs e)
        {
            chart1.Series["Book"].Points.RemoveAt(chart1.Series["Book"].Points.Count - 1);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (connect.State == ConnectionState.Closed)
            {
                try
                {
                    connect.Open();
                    string selectData = @"
                SELECT 
                    FORMAT(issue_date, 'dd/MM/yyyy') AS IssueDate, 
                    COUNT(id) AS TotalIssues
                FROM 
                    issues
                WHERE 
                    date_delete IS NULL
                GROUP BY 
                    FORMAT(issue_date, 'dd/MM/yyyy')
                ORDER BY 
                    IssueDate";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        chart1.Series["Book"].Points.Clear();

                        chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                        chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                        while (reader.Read())
                        {
                            string issueDate = reader["IssueDate"].ToString();
                            int totalIssues = Convert.ToInt32(reader["TotalIssues"]);
                            chart1.Series["Book"].Points.AddXY(issueDate, totalIssues);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}