using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAJK_OnlineRetail
{
    class Country
    {
        private string countryName;
        private float sales;
        private float procent;

        public Country(string CountryName, float Sales, float Procent)
        {
            countryName = CountryName;
            sales = Sales;
        }
        public string CountryName
        {
            get { return countryName; }
            set { countryName = value; }
        }
        public float Sales
        {
            get { return sales; }
            set { sales = value; }
        }
        public float Procent
        {
            get { return procent; }
            set { procent = value; }
        }
    }
}
