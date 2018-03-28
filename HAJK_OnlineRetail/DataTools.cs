using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HAJK_OnlineRetail
{
    public static class DataTools
    {
        //JR method att fylla en list med alla rader från databas
        //
        internal static List<InvoiceRows> GetInvoiceRows(string inputQuery)
        {
            //medlemsvariabler
            List<InvoiceRows> listRow = new List<InvoiceRows>();

            //connection
            string sConnectionString = "Data Source =LAPTOP2\\TESTSQL; Initial Catalog =OnlineRetail; Integrated Security =True;";
            SqlConnection sqlConnection = new SqlConnection(sConnectionString);
            try
            {
                sqlConnection.Open();
                MessageBox.Show("Connection successful(invoice)");
                SqlDataAdapter adapter = new SqlDataAdapter(inputQuery, sqlConnection);
                DataSet dataSet = new DataSet("invoices");
                adapter.Fill(dataSet);

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {

                    int.TryParse(Convert.ToString(dr["InvoiceNo"]), out int invoiceNum);
                    string stockCode = Convert.ToString(dr["StockCode"]);
                    string description = Convert.ToString(dr["Description"]);
                    int.TryParse(Convert.ToString(dr["Quantity"]), out int quantity);
                    DateTime invoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                    float unitPrice = float.Parse((dr["UnitPrice"]).ToString());
                    int.TryParse(Convert.ToString(dr["CustomerID"]), out int customerId);
                    string country = Convert.ToString(dr["Country"]);
                    string region = Convert.ToString(dr["Region"]);
                    int.TryParse(Convert.ToString(dr["population"]), out int population);

                    InvoiceRows currentRow = new InvoiceRows(invoiceNum, stockCode, description, quantity, invoiceDate, unitPrice, customerId,
                                                            country, region, population);

                    listRow.Add(currentRow);
                }
            }
            catch (Exception ex)
            {
                //Exception
                MessageBox.Show("Connection unsuccessful (invoice)");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
            return listRow;
        }

        //JR method at fylla en list av country sales
        internal static List<Country> GetCountrySales(string myQuery)
        {
            //medlemsvariabler
            List<Country> outList = new List<Country>();
            float totalSales = 0;

            //connection
            string sConnectionString = "Data Source =LAPTOP2\\TESTSQL; Initial Catalog =OnlineRetail; Integrated Security =True;";
            SqlConnection sqlConnection = new SqlConnection(sConnectionString);

            try
            {
                sqlConnection.Open();
                MessageBox.Show("Connection successful (country)");
                SqlDataAdapter adapter = new SqlDataAdapter(myQuery, sqlConnection);
                DataSet dataSet = new DataSet("country");
                adapter.Fill(dataSet);

                //Fylla List
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    string country = Convert.ToString(dr["Country"]);
                    float.TryParse(Convert.ToString(dr["Sales"]), out float sales);
                     

                    Country currentRow = new Country(country, sales, 0);
                    outList.Add(currentRow);
                }
                
                //räkna total sales
                foreach (Country ctry in outList)
                {
                    totalSales = totalSales + ctry.Sales;
                }

                //räkna procent av total sales
                foreach (Country ctry in outList)
                {
                    float procent = 0;
                    procent = (ctry.Sales/totalSales)*100;
                    ctry.Procent = procent;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection unsuccessful (country)");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
            return outList;
        }
    }
}
