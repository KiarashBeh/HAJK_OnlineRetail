using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingOnlineRetail
{
    class InvoiceRows
    {
        private int invNum;
        private string stkCode;
        private string desc;
        private int qty;
        private DateTime invDate;
        private float price;
        private int custId;
        private string land;
        private string region;
        private int population;


        public InvoiceRows
            (
            int invoiceNum,
            string stockCode,
            string description,
            int quantity,
            DateTime invoiceDate,
            float unitPrice,
            int customerId,
            string country,
            string r,
            int pop
            )
        {
            invNum = invoiceNum;
            stkCode = stockCode;
            desc = description;
            qty = quantity;
            invDate = invoiceDate;
            price = unitPrice;
            custId = customerId;
            land = country;
            Region = r;
            Population = pop;

        }

        public int InvoiceNum
        {
            get { return invNum; }
            set { invNum = value; }
        }
        public string StockCode
        {
            get { return stkCode; }
            set { stkCode = value; }
        }
        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }
        public int Quantity
        {
            get { return qty; }
            set { qty = value; }
        }
        public DateTime InvoiceDate
        {
            get { return invDate; }
            set { invDate = value; }
        }
        public float UnitPrice
        {
            get { return price; }
            set { price = value; }
        }
        public int CustomerId
        {
            get { return custId; }
            set { custId = value; }
        }
        public string Country
        {
            get { return land; }
            set { land = value; }
        }

        public string Region { get => region; set => region = value; }
        public int Population { get => population; set => population = value; }
    }
}