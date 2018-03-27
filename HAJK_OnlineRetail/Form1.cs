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

            conn.ConnectionString = "Data Source=ALPHAG33K\\SQL2017;Initial Catalog=OnlineRetail;Integrated Security=True;Connection timeout=10";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetList();
            Countries();
        }

        private List<InvoiceRows> GetList() //Metod för att fylla InvoiceRows från SQL. Returnerar en lista.
        {
            List<InvoiceRows> fillOrderLines = new List<InvoiceRows>();
        
        try
            {
                conn.Open();
                SqlCommand myCommand = new SqlCommand("select * from OnlineRetail2;", conn);

                SqlDataReader myReader = myCommand.ExecuteReader();
                //MessageBox.Show("Connection fungerar");

                int invoiceNum;             //Variabler deklareras till listan som ska returneras
                string stockCode;
                string description;
                int quantity;
                DateTime invoiceDate;
                float unitPrice;
                int customerId;
                string country;
                string region;
                int population;

                while (myReader.Read())     //Läser från SQL och konverterar datatyperna
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


                    InvoiceRows temRows = new InvoiceRows(          //Skapar nya objekt med den importerade datan ovan
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

        }
        /*
        private void InitData()
        {
            List<InvoiceRows> OrderLines1 = GetList();
        }
        */
        private void FirstChart()
        {
            chart1.Series.Clear();
            chart1.Series.Add("Series1");
            DateTime StartDate = DateTime.Parse(dateTimePicker1.Text);
            DateTime EndDate = DateTime.Parse(dateTimePicker2.Text);
            

            List<InvoiceRows> ChartList = GetList();

            var datapoints = from asd in ChartList
                             where asd.Country == valdLand
                             where asd.UnitPrice > 0
                             where asd.Quantity > 0
                             //where asd.invoiceDate > StartDate
                             //where asd.invoiceDate < EndDate
                             select new { sale = (asd.Quantity * asd.UnitPrice), asd.InvoiceDate };

            foreach (var sales in datapoints)
            {
                chart1.Series["Series1"].Points.AddXY(sales.InvoiceDate, sales.sale);
            }
            chart1.Series["Series1"].ChartType = SeriesChartType.Column;
            //chart1.ChartAreas[0].AxisX.Minimum = StartDate.ToOADate();
            //chart1.ChartAreas[0].AxisX.Maximum = EndDate.ToOADate();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FirstChart();

        }

        List<InvoiceRows> World = new List<InvoiceRows>();

        private void Countries() //Metod för att fylla komboboxen med länder
        {

            World = GetList(); //Variabeln World anropar metoden GetList som anropar SQL databasen. Nedan filtrerar vi ut enbart länderna.

            var getCountries = World.Select(s => s.Country).Distinct();
            foreach (var x in getCountries)
            {            
                comboBox1.Items.Add(x);
            }
            ///World.Add(Enumerable.Cast<string>(getCountries).ToList());
 
        }

        private string valdLand;

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            valdLand = comboBox1.SelectedItem as string;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
