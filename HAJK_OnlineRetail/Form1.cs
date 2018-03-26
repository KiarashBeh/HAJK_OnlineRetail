using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestingOnlineRetail
{
    

    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection();

        public Form1()
        {
            InitializeComponent();

            conn.ConnectionString = "Data Source=LAPTOP-7AL6OH88\\SQL2017;Initial Catalog=OnlineRetail;Integrated Security=True;Connection timeout=10";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitData();            
        }

        private List<InvoiceRows> GetList()
        {
            List<InvoiceRows> fillOrderLines = new List<InvoiceRows>();
        
        try
            {
                conn.Open();
                SqlCommand myCommand = new SqlCommand("select * from OnlineRetail;", conn);

                SqlDataReader myReader = myCommand.ExecuteReader();
                //MessageBox.Show("Connection fungerar");

                int invoiceNum;
                string stockCode;
                string description;
                int quantity;
                DateTime invoiceDate;
                float unitPrice;
                int customerId;
                string country;

                while (myReader.Read())
                {
                    int.TryParse(myReader["invoiceNo"].ToString(), out invoiceNum);
                    stockCode =  myReader["StockCode"].ToString();
                    description = myReader["Description"].ToString();
                    int.TryParse(myReader["Quantity"].ToString(), out quantity);
                    DateTime.TryParse(myReader["InvoiceDate"].ToString(), out invoiceDate);
                    float.TryParse(myReader["UnitPrice"].ToString(), out unitPrice);
                    int.TryParse(myReader["CustomerID"].ToString(), out customerId);
                    country = myReader["Country"].ToString();

                    InvoiceRows temRows = new InvoiceRows(
                        invoiceNum, stockCode,
                        description, quantity,
                        invoiceDate, unitPrice,
                        customerId, country);

                    fillOrderLines.Add(temRows);
                }
}
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return fillOrderLines;

        }
        private void InitData()
        {
            List<InvoiceRows> OrderLines1 = GetList();
        }
        private void FirstChart()
        {
            
            DateTime StartDate = DateTime.Parse(dateTimePicker1.Text);
            DateTime EndDate = DateTime.Parse(dateTimePicker2.Text);
            

            List<InvoiceRows> ChartList = GetList();

            var datapoints = from asd in ChartList
                             where asd.country == "Sweden"
                             where asd.unitPrice > 0
                             where asd.quantity > 0
                             //where asd.invoiceDate > StartDate
                             //where asd.invoiceDate < EndDate
                             select new { sale = asd.quantity * asd.unitPrice, asd.invoiceDate };

            foreach (var sales in datapoints)
            {
                chart1.Series["Series1"].Points.AddXY(sales.invoiceDate, sales.sale);
            }
            chart1.Series["Series1"].ChartType = SeriesChartType.Column;
            chart1.ChartAreas[0].AxisX.Minimum = StartDate.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = EndDate.ToOADate();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FirstChart();
            
            
        }

        private void Countries()
        {
            Country Sweden = new Country("Sweden", 90000000);
        }
      
    }
}
