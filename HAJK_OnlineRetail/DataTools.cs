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
            List<InvoiceRows> outList = new List<InvoiceRows>();
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
                    invoiceNum
                    stockCode
                    description
                    quantity
                    invoiceDate
                    unitPrice
                    customerId
                    country



                    InvoiceRows listRow = new InvoiceRows();


                    outList.Add(listRow);
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
            return outList;
        }
    }
}
