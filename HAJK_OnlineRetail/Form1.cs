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
        SqlDataReader myReader;
         
        //Lista för att fylla combobox med val av länder.
        List<InvoiceRows> World = new List<InvoiceRows>();
        //Olika variabler för olika val
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
        string prodTitle = "Top 5";
        //SQL querie för att få fram total försäljning per år och månad.
        private string salesPerYear = "select CONVERT(date, InvoiceDate) as dagar, sum(Quantity*UnitPrice) as 'TotalSales' from OnlineRetail2 group by CONVERT(date, InvoiceDate) order by CONVERT(date, InvoiceDate), TotalSales";
        //SQL querie för country, population och försäljning per capita.
        private string totalSaleForEachCountry = "SELECT Country, Population, SUM(quantity * UnitPrice)/Population As 'TotalSalePerPopulation' FROM[OnlineRetail].[dbo].[OnlineRetail2] group by Population, Country order by [TotalSalePerPopulation]  Desc";
        private double totalSalePerPopulation;


        public Form1()
        {
            InitializeComponent();

            conn.ConnectionString = "Data Source=LAPTOP-7AL6OH88\\SQL2017;Initial Catalog=OnlineRetail;Integrated Security=True;connection timeout=10";
        }

        //Här samlas det som ska köras när Form1 laddas in.
        private void InitData()
        {

            valdTopBotProd = topProd;
            valdTopBot = topFive;
            getTopCountries();
            getTopProduct();
            getSalesPerYear();
            KpiTotalSalePerPopulation();
            Countries();

            FirstChart();
            SecondChart();
            ThirdChart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitData();
            this.BackColor = Color.White;
        }

        private SqlDataReader openConnection(string Name)
        {
            conn.Open();
            SqlCommand myCommand2 = new SqlCommand(Name, conn);
            myReader = myCommand2.ExecuteReader();
            return myReader;
        }

        private List<InvoiceRows> getTotalSalePerPop()
        {
            List<InvoiceRows> totalSale = new List<InvoiceRows>();

            try
            {
                //openConnection(totalSaleForEachCountry);
                conn.Open();
                SqlCommand myCommand2 = new SqlCommand(totalSaleForEachCountry, conn);
                myReader = myCommand2.ExecuteReader();

                float totalSale1;
                string Country;

                while (myReader.Read())
                {
                    Country = myReader["Country"].ToString();
                    float.TryParse(myReader["TotalSalePerPopulation"].ToString(), out totalSale1);

                    InvoiceRows tempRows = new InvoiceRows(totalSale1, Country, "sss");

                    totalSale.Add(tempRows);
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

            return totalSale;
        }

        private List<InvoiceRows> getSalesPerYear()
            {
            List<InvoiceRows> totalSales = new List<InvoiceRows>();

            try
            {
                conn.Open();
                SqlCommand myCommand2 = new SqlCommand(salesPerYear, conn);
                SqlDataReader myReader2 = myCommand2.ExecuteReader();

                
                float sales;
                DateTime allDays;

                while (myReader2.Read())
                {
                   
                    DateTime.TryParse(myReader2["dagar"].ToString(), out allDays);
                    float.TryParse(myReader2["TotalSales"].ToString(), out sales);

                    InvoiceRows tempRows = new InvoiceRows(allDays, sales);

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

        private void KpiTotalSalePerPopulation()
        {
            List<InvoiceRows> ChartList1 = getTotalSalePerPop();
            List<InvoiceRows> TotSale = getSalesPerYear();
            DateTime StartDate = DateTime.Parse(dateTimePicker1.Text);
            DateTime EndDate = DateTime.Parse(dateTimePicker2.Text);

            double totalDays = (EndDate - StartDate).TotalDays;
            float totalSale = 0;

            foreach (var item in ChartList1)
            {
                if (item.Country.ToString() == valdLand)
                    totalSalePerPopulation = item.TotalPricePerPop;

            }
            
            var salePerDay = from das in TotSale
                             where das.AllDays < EndDate
                             where das.AllDays >= StartDate
                             select das;

            foreach (var item in salePerDay)
            {
                totalSale += item.UnitPrice;
            }           

            textBox1.Text = (Math.Round(totalSalePerPopulation, 5)).ToString();
            textBox2.Text = (Convert.ToDouble(textBox1.Text) * 1.05).ToString();
            textBox3.Text = (Math.Round((totalSale / totalDays), 0)).ToString();
            textBox4.Text = totalSale.ToString();
        }

        //Första chart som visar top eller bot länder.
        private void FirstChart()
        {
            //rensa och lägg till tom series och chart innan den laddas.
            chart1.Series.Clear();
            chart1.Series.Add("Series1");
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("ChartArea1");
                                 

            List<InvoiceRows> ChartList = getTopCountries();

            var datapoints = from asd in ChartList
                             select new { asd.Country, asd.UnitPrice, asd.AllDays };
                        
            foreach (var sales in datapoints)
            {
                chart1.Series["Series1"].Points.AddXY(sales.Country, sales.UnitPrice);
            }
            chart1.Series["Series1"].ChartType = SeriesChartType.Column;
            chart1.Series["Series1"].IsVisibleInLegend = false;
            chart1.Titles.Clear();
            chart1.Titles.Add(prodTitle + " Countries");
            chart1.ChartAreas["ChartArea1"].BackColor = Color.WhiteSmoke;
            chart1.Series["Series1"].Color = Color.DeepSkyBlue;
            chart1.Series["Series1"].BorderWidth = 1;
            chart1.Series["Series1"].BorderColor = Color.DarkBlue;
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;


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
            chart2.Series["Series1"].IsVisibleInLegend = false;
            chart2.Titles.Clear();
            chart2.Titles.Add(prodTitle + " Products");
            chart2.Series["Series1"].Color = Color.DeepSkyBlue;
            chart2.Series["Series1"].BorderWidth = 1;
            chart2.Series["Series1"].BorderColor = Color.DarkBlue;
            chart2.ChartAreas["ChartArea1"].BackColor = Color.WhiteSmoke;
            chart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart2.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

        }

        //Linechart som visar hur försäljningen går upp och ner på en timeline.
        private void ThirdChart()
        {
            chart3.Series.Clear();
            chart3.Series.Add("Series1");
            chart3.ChartAreas.Clear();
            chart3.ChartAreas.Add("ChartArea1");
            chart3.Titles.Clear();
            List<InvoiceRows> salesList = getSalesPerYear();

            DateTime StartDate = DateTime.Parse(dateTimePicker1.Text);
            DateTime EndDate = DateTime.Parse(dateTimePicker2.Text);

            var datapoints = from asgag in salesList
                             select new {  asgag.AllDays, asgag.UnitPrice };

            foreach (var sales in datapoints)
            {
                chart3.Series["Series1"].Points.AddXY(sales.AllDays, sales.UnitPrice);
            }
            chart3.Series["Series1"].ChartType = SeriesChartType.Line;
            chart3.ChartAreas["ChartArea1"].AxisX.Interval = 20;
            chart3.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart3.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            chart3.ChartAreas[0].AxisX.Minimum = StartDate.ToOADate();
            chart3.ChartAreas[0].AxisX.Maximum = EndDate.ToOADate();
            chart3.Series["Series1"].IsVisibleInLegend = false;            
            chart3.Titles.Add("Total Sales timeline");
            chart3.Series["Series1"].BorderWidth = 2;
            chart3.Series["Series1"].Color = Color.DeepSkyBlue;
            chart3.ChartAreas["ChartArea1"].BackColor = Color.WhiteSmoke;

                        
        }

      

        //Välj land i dropdown lista och sedan sätts värdet till variabeln valdLand som string
        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            valdLand = comboBox1.SelectedItem as string;
            KpiTotalSalePerPopulation();
            label4.Text = valdLand;

            //Eftersom country "Unspecified" och European Community" inte har någon population så sätter vi dessa värden till totalförsäljning.
            if (valdLand == "Unspecified" || valdLand == "European Community")
            {
                label1.Text = "Total sales";
                textBox2.Visible = false;
                label2.Visible = false;
            }
            else
            {
                label1.Text = "Sales per Capita";
                textBox2.Visible = true;
                label2.Visible = true;
            }
        }

        //Välj vilket lands försäljning per capita du vill se.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            valdLand = comboBox1.SelectedItem as string;
            KpiTotalSalePerPopulation();
        }      

        //Välj antingen bot eller top för att bestämma vilken querie som ska köras och sedan uppdateras chartsen.
        private void comboBox2_DropDownClosed(object sender, EventArgs e)
        {
            topOrBot = comboBox2.SelectedItem as string;

            if (topOrBot == "Top5")
            {
                valdTopBot = topFive;
                valdTopBotProd = topProd;
                chart2.Titles.Clear();
                prodTitle = "Top 5";
            }
            else
            {
                valdTopBot = botFive;
                valdTopBotProd = botProd;
                chart2.Titles.Clear();
                prodTitle = "Bot 5";
            }

            FirstChart();
            SecondChart();
        }

        //Lägg till länderna från databasen i combobox.
        private void Countries()
        {

            World = getTotalSalePerPop();

            var getCountries = World.Select(s => s.Country).Distinct();

            foreach (var x in getCountries)
            {
                comboBox1.Items.Add(x);
            }
            ///World.Add(Enumerable.Cast<string>(getCountries).ToList());

        }

        //När värdet i datumbäljaren ändras så ändras även linechart och försäljning per capita.
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            ThirdChart();
            KpiTotalSalePerPopulation();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            ThirdChart();
            KpiTotalSalePerPopulation();
        }
    }
}
