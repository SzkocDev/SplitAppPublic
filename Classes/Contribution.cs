using System;
using System.Collections.Generic;
using System.Text;

namespace WPFUISplitApp
{
    public class Contribution
    {
        public decimal ContributionValue { get;  set; }
        public string Description { get;  set; }


        public Contribution(decimal contributionValue, string description)
        {
            ContributionValue = contributionValue;
            Description = description;
        }
    }
}
