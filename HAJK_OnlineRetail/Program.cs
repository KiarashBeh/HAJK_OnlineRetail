using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace HAJK_OnlineRetail
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = "Data Source=ALPHAG33K\\SQL2017;Initial Catalog=OnlineRetail;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            try //testar connectionen
            {
                conn.Open();

                SqlCommand myQuery = new SqlCommand("select * from OnlineRetail", conn);

                SqlDataReader myReader = myQuery.ExecuteReader();


                string InvoiceDate;
                while (myReader.Read())
                {
                    InvoiceDate = myReader["InvoiceDate"].ToString();

                    MessageBox.Show(InvoiceDate);



                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }
            Console.ReadLine();
        }
    }
}
