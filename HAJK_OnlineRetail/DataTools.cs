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

        internal static List<InvoiceRows> GetInvoiceRows(string inputQuery)
        {
            List<InvoiceRows> listRow = new List<InvoiceRows>();

            string sConnectionString = "Data Source =LAPTOP2\\TESTSQL; Initial Catalog =OnlineRetail; Integrated Security =True;";
            SqlConnection sqlConnection = new SqlConnection(sConnectionString);
            try
            {
                sqlConnection.Open();
                MessageBox.Show("Connection successful");
                SqlDataAdapter adapter = new SqlDataAdapter(inputQuery, sqlConnection);
                DataSet dataSet = new DataSet("invoices");
                adapter.Fill(dataSet);

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    int invoiceNum = Convert.ToInt32(dr["InvoiceNo"]);
                    string stockCode = Convert.ToString(dr["StockCode"]);
                    string description = Convert.ToString(dr["Description"]);
                    int quantity = Convert.ToInt32(dr["Quantity"]);
                    DateTime invoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                    float unitPrice = float.Parse((dr["UnitPrice"]).ToString());
                    int customerId = Convert.ToInt32(dr["CustomerID"]);
                    string country = Convert.ToString(dr["Country"]);
                    string region = Convert.ToString(dr["Region"]);
                    int population = Convert.ToInt32(dr["population"]);

                    InvoiceRows currentRow = new InvoiceRows(invoiceNum,stockCode,description,quantity,invoiceDate,unitPrice,customerId,
                                                            country,region,population);

                    listRow.Add(currentRow);
                }
            }
            catch (Exception ex)
            {
                //Exception
                MessageBox.Show("Connection unsuccessful");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
            return listRow;
        }
    }
}
