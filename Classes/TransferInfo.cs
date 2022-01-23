using System;
using System.Collections.Generic;
using System.Text;

namespace WPFUISplitApp
{
    class TransferInfo
    {
        public decimal currentValue {get;set;}
        public string name { get; set; }
        public TransferInfo(Person person)
        {
            name = person.Name;
            currentValue = person.Charge;
        }
    }
}
