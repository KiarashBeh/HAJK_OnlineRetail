using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingOnlineRetail
{
    class Country
    {
        private string name;
        private int population;

        public Country(string Name, int Population)
        {
            name = Name;
            population = Population;
        }

        public string Name { get => name; set => name = value; }
        public int Population { get => population; set => population = value; }
    }
}
