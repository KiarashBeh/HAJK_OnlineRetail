using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAJK_OnlineRetail
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<InvoiceRows> InvoiceData = DataTools.GetInvoiceRows("SELECT * FROM dbo.OnlineRetail2");
            List<Country> CountryData = DataTools.GetCountrySales("SELECT Country, sum(unitPrice) AS Sales FROM OnlineRetail2 GROUP BY Country");


            //test box
            string outText = "";
            foreach (Country cty in CountryData)
            {
                outText = outText + cty.CountryName + "," + Convert.ToString(cty.Sales) + "," + Convert.ToString(cty.Procent) + "******";
            }

            MessageBox.Show(outText);
        }

    }
}
