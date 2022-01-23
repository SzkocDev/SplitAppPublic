using System;
using System.Collections.Generic;
using System.Text;

namespace WPFUISplitApp
{
    class Transfer
    {
        public string Payer { get; private set; }
        public string Beneficiary { get; private set; }
        public decimal Value { get; private set; }
        public string Jank { get; private set; }
        public Transfer(string payer, string beneficiary, decimal value)
        {
            Payer = payer;
            Beneficiary = beneficiary;
            Value = value;
            Jank = "<==";
        }
    }
}
