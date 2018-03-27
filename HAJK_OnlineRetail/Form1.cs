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
    //hej hej
{
    

    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection();
        List<InvoiceRows> World = new List<InvoiceRows>();
        List<string> World2 = new List<string>();

        private string topOrBot;
        private string valdLand;

        private string topFive = "Select top 5 sum(Quantity * UnitPrice) as 'Total Sales', Country from OnlineRetail2 group by Country order by 'Total Sales' desc";
        private string botFive = "Select top 5 sum(Quantity * UnitPrice) as 'Total Sales', Country from OnlineRetail2 group by Country order by 'Total Sales' asc";
        private string valdTopBot;

        private string topProd = "select top 5 sum(UnitPrice) as TotalSales, sum([Quantity]) as 'Quantity', [Description] from OnlineRetail2 where UnitPrice > 0 and Quantity > 0 and Description not like '%postage%' and Description not like '%fee%' and Description not like '%manual%' and Description not like '%adjust%' group by[Description] order by[TotalSales] desc";
        private string botProd = "select top 5 sum(UnitPrice) as TotalSales, sum([Quantity]) as 'Quantity', [Description] from OnlineRetail2 where UnitPrice > 0 and Quantity > 0 and Description not like '%postage%' and Description not like '%fee%' and Description not like '%manual%' and Description not like '%adjust%' group by[Description] order by[TotalSales] asc";
        //private string topBotProd;
        string valdTopBotProd;



        public Form1()
        {
            InitializeComponent();

            conn.ConnectionString = "Data Source=LAPTOP-7AL6OH88\\SQL2017;Initial Catalog=OnlineRetail;Integrated Security=True;connection timeout=10";
        }

        private void InitData()
        {

            valdTopBotProd = topProd;
            valdTopBot = topFive;
            getTopCountries();
            getTopProduct();

            FirstChart();
            SecondChart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private List<InvoiceRows> getTopCountries()
        {
            List<InvoiceRows> topCountry = new List<InvoiceRows>();

            try
            { 
                conn.Open();
                SqlCommand myCommand2 = new SqlCommand(valdTopBot, conn);
                SqlDataReader myReader2 = myCommand2.ExecuteReader();

                float unitPrice;
                string Country;

                while (myReader2.Read())
                {
                    float.TryParse(myReader2["Total Sales"].ToString(), out unitPrice);
                    Country = myReader2["Country"].ToString();

                    InvoiceRows tempRows = new InvoiceRows(Country, unitPrice);

                    topCountry.Add(tempRows);
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

            return topCountry;            
        }

        private List<InvoiceRows> getTopProduct()
        {
            List<InvoiceRows> topProduct = new List<InvoiceRows>();

            try
            {
                conn.Open();
                SqlCommand myCommand = new SqlCommand(valdTopBotProd, conn);
                SqlDataReader myReader = myCommand.ExecuteReader();

                float unitPrice;
                string Description;

                while (myReader.Read())
                {
                    float.TryParse(myReader["Quantity"].ToString(), out unitPrice);
                    Description = myReader["Description"].ToString();

                    InvoiceRows tempRows = new InvoiceRows(unitPrice, Description);

                    topProduct.Add(tempRows);
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

            return topProduct;
        }


        /*private List<InvoiceRows> GetList()
        {
            List<InvoiceRows> fillOrderLines = new List<InvoiceRows>();
        
        try
            { 
                conn.Open();
                SqlCommand myCommand = new SqlCommand("select * from OnlineRetail2;", conn);

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
                string region;
                int population;

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
                    region = myReader["Region"].ToString();
                    int.TryParse(myReader["Population"].ToString(), out population);


                    InvoiceRows temRows = new InvoiceRows(
                        invoiceNum, stockCode,
                        description, quantity,
                        invoiceDate, unitPrice,
                        customerId, country,
                        region, population);

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

        }*/



        private void FirstChart()
        {
            chart1.Series.Clear();
            chart1.Series.Add("Series1");
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("ChartArea1");

            DateTime StartDate = DateTime.Parse(dateTimePicker1.Text);
            DateTime EndDate = DateTime.Parse(dateTimePicker2.Text);
            

            List<InvoiceRows> ChartList = getTopCountries();

            var datapoints = from asd in ChartList
                             select new { asd.Country, asd.UnitPrice };

            foreach (var sales in datapoints)
            {
                chart1.Series["Series1"].Points.AddXY(sales.Country, sales.UnitPrice);
            }
            chart1.Series["Series1"].ChartType = SeriesChartType.Column;
            //chart1.ChartAreas[0].AxisX.Minimum = StartDate.ToOADate();
            //chart1.ChartAreas[0].AxisX.Maximum = EndDate.ToOADate();
        }
        
        private void SecondChart()
        {
            chart2.Series.Clear();
            chart2.Series.Add("Series1");
            chart2.ChartAreas.Clear();
            chart2.ChartAreas.Add("ChartArea1");

            DateTime StartDate = DateTime.Parse(dateTimePicker1.Text);
            DateTime EndDate = DateTime.Parse(dateTimePicker2.Text);


            List<InvoiceRows> ChartList = getTopProduct();

            var datapoints = from asd in ChartList
                             select new { asd.Description, asd.UnitPrice };

            foreach (var sales in datapoints)
            {
                chart2.Series["Series1"].Points.AddXY(sales.Description, sales.UnitPrice);
            }
            chart2.Series["Series1"].ChartType = SeriesChartType.Column;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FirstChart();
            SecondChart();
            
        }       

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            valdLand = comboBox1.SelectedItem as string; 
        }

        private void comboBox2_DropDownClosed(object sender, EventArgs e)
        {
            topOrBot = comboBox2.SelectedItem as string;
            
            


            if (topOrBot == "Top5")
            {
                valdTopBot = topFive;
                valdTopBotProd = topProd;
            }
            else
            {
                valdTopBot = botFive;
                valdTopBotProd = botProd;
            }

            FirstChart();
            SecondChart();
        }
    }
}
