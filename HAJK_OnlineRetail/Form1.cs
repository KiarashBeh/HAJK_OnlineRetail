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
        //Olika variabler för olika val
        private string firstDate;
        private string lastDate;
        private string topOrBot;
        private string valdLand;
        //SQL querie för top 5 respektive bot 5 länder per försäljning.
        private string topFive = "Select top 5 sum(Quantity * UnitPrice) as 'Total Sales', Country from OnlineRetail2 group by Country order by 'Total Sales' desc";
        private string botFive = "Select top 5 sum(Quantity * UnitPrice) as 'Total Sales', Country from OnlineRetail2 group by Country order by 'Total Sales' asc";
        private string valdTopBot;
        //SQL querie för top 5 respektive bot 5 produkter.
        private string topProd = "select top 5 sum(UnitPrice) as TotalSales, sum([Quantity]) as 'Quantity', [Description] from OnlineRetail2 where UnitPrice > 0 and Quantity > 0 and Description not like '%postage%' and Description not like '%fee%' and Description not like '%manual%' and Description not like '%adjust%' group by[Description] order by[TotalSales] desc";
        private string botProd = "select top 5 sum(UnitPrice) as TotalSales, sum([Quantity]) as 'Quantity', [Description] from OnlineRetail2 where UnitPrice > 0 and Quantity > 0 and Description not like '%postage%' and Description not like '%fee%' and Description not like '%manual%' and Description not like '%adjust%' group by[Description] order by[TotalSales] asc";
        string valdTopBotProd;
        //SQL querie för att få fram total försäljning per år och månad.
        private string salesPerYear = "Select cast(datepart(year, InvoiceDate) as varchar) + '.' + cast(datepart(month, InvoiceDate) as varchar) + '.' + cast(datepart(day, InvoiceDate)as varchar) as dagar, sum(Quantity*UnitPrice) as 'TotalSales' from OnlineRetail2 group by datepart(month, InvoiceDate),datepart(year, InvoiceDate),datepart(day, InvoiceDate) order by datepart(year, InvoiceDate), datepart(month, InvoiceDate),datepart(day, InvoiceDate)";


        public Form1()
        {
            InitializeComponent();

            conn.ConnectionString = "Data Source=LAPTOP-7AL6OH88\\SQL2017;Initial Catalog=OnlineRetail;Integrated Security=True;connection timeout=10";
        }

        private void InitData()//Här samlas det som ska köras när Form1 laddas in.
        {

            valdTopBotProd = topProd;
            valdTopBot = topFive;
            getTopCountries();
            getTopProduct();
            getSalesPerYear();

            FirstChart();
            SecondChart();
            ThirdChart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private List<InvoiceRows> getSalesPerYear()
            {
            List<InvoiceRows> totalSales = new List<InvoiceRows>();

            try
            {
                conn.Open();
                SqlCommand myCommand2 = new SqlCommand(salesPerYear, conn);
                SqlDataReader myReader2 = myCommand2.ExecuteReader();

                int years;
                int months;
                int days;
                float sales;
                int allDays;

                while (myReader2.Read())
                {
                    //int.TryParse(myReader2["artal"].ToString(), out years);
                    //int.TryParse(myReader2["manad"].ToString(), out months);
                    //int.TryParse(myReader2["dag"].ToString(), out days);
                    int.TryParse(myReader2["dagar"].ToString(), out allDays);
                    float.TryParse(myReader2["TotalSales"].ToString(), out sales);

                    InvoiceRows tempRows = new InvoiceRows(allDays.ToString(), sales);

                    totalSales.Add(tempRows);
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
            return totalSales;
        }
            

        //Hämta top respektive bot länder beroende på vilket val man gjort (valdTopBot är kopplat till sql queries ovan).
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

        //Hämta top respektive bot pordukter beroende på vilket val man gjort (valdTopBotProd är kopplat till sql queries ovan).
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


        //Första chart som visar top eller bot länder.
        private void FirstChart()
        {
            //rensa och lägg till tom series och chart innan den laddas.
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
        
        //Andra charten som visar top eller bot produkter
        private void SecondChart()
        {
            //rensa och lägg till tom series och chart innan den laddas.
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

        private void ThirdChart()
        {
            List<InvoiceRows> salesList = getSalesPerYear();
            //Här pysslar jag just nu med om det går att få fram 
            var datapoints = from asgag in salesList
                             select new {  asgag.AllDays, asgag.UnitPrice };

            foreach (var sales in datapoints)
            {
                chart3.Series["Series1"].Points.AddXY(sales.AllDays, sales.UnitPrice);
            }
            chart3.Series["Series1"].ChartType = SeriesChartType.Line;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FirstChart();
            SecondChart();
            
        }       

        //Välj land i dropdown lista och sedan sätts värdet till variabeln valdLand som string
        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            valdLand = comboBox1.SelectedItem as string; 
        }

        //Välj antingen bot eller top för att bestämma vilken querie som ska köras och sedan uppdateras chartsen.
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
