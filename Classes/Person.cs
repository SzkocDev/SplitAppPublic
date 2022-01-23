using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace WPFUISplitApp
{
    public class Person
    {
        public decimal Charge { get;  set; }
        public string Name { get;  set; }
        public decimal TotalContributions { get;  set; }
        public decimal TotalCountributionsRounded { get;  set; }
        public List<Contribution> ListOfContributions { get;  set; }


        public Person(string name)
        {
            Name = name;
            TotalContributions = 0;
            ListOfContributions = new List<Contribution>();
            Charge = 0;
        }
        public void AddContribution(decimal contribution, string description)
        {
            ListOfContributions.Add(new Contribution(contribution, description));
            RecalculateContributions();
        }

        public void RecalculateContributions()
        {
            TotalContributions = ListOfContributions.Sum(x => x.ContributionValue);
            TotalCountributionsRounded = Math.Round(TotalContributions, 2);
        }
        public void SetCharge(decimal value)
        {
            Charge = value;
        }
        public void ChangeListOfContributions(List<Contribution> listOfContributions)
        {
            ListOfContributions = listOfContributions;
            RecalculateContributions();
        }
    }
}
