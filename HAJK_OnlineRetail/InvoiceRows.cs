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


        public InvoiceRows
            (
            int invoiceNum,
            string stockCode,
            string description,
            int quantity,
            DateTime invoiceDate,
            float unitPrice,
            int customerId,
            string country
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

        }

        public int invoiceNum
        {
            get { return invNum; }
            set { invNum = value; }
        }
        public string stockCode
        {
            get { return stkCode; }
            set { stkCode = value; }
        }
        public string description
        {
            get { return desc; }
            set { desc = value; }
        }
        public int quantity
        {
            get { return qty; }
            set { qty = value; }
        }
        public DateTime invoiceDate
        {
            get { return invDate; }
            set { invDate = value; }
        }
        public float unitPrice
        {
            get { return price; }
            set { price = value; }
        }
        public int customerId
        {
            get { return custId; }
            set { custId = value; }
        }
        public string country
        {
            get { return land; }
            set { land = value; }
        }
    }
}