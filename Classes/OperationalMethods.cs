using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
namespace WPFUISplitApp
{
    public class OperationalMethods
    {
        public List<Person> ListOfPeople { get; set; }
        string sessionFileName = @"Session.json";
        public OperationalMethods()
        {
            ListOfPeople = new List<Person>();
        }
        public void AddPerson(string name)
        {
            if (ListOfPeople.Exists(x => x.Name == name) != true)
            {
                ListOfPeople.Add(new Person(name));
            }
        }
        public void CreateMockData()
        {

            AddPerson("Adam");
            AddPerson("Wojtek");
            AddPerson("Basia");
            AddPerson("Zosia");
            AddPerson("Asia");
            AddPerson("Gosia");
            AddPerson("Agata");
            AddPerson("Weronika");
            AddPerson("Alicja");

            foreach (var person in ListOfPeople)
            {
                Random rnd = new Random();
                for (int i = 0; i < rnd.Next(2, 5); i++)
                {
                    person.AddContribution(rnd.Next(1, 150), $"test{rnd.Next(1, 150)}{rnd.Next(1, 150)}");
                }
            }
        }

        #region Session Managment(files)
        public void ReadOrCreateSessionFile()
        {
            if (File.Exists(sessionFileName))
            {
                ReadFile();
            }
            else
            {
                CreateFile();
            }            
        }
        public void ReadFile()
        {
            var file = File.ReadAllLines(sessionFileName);
            foreach(string line in file)
            {
                var person = JsonConvert.DeserializeObject<Person>(line); // there has to be better way to do that rather than making every property public, Person : ExportPerson class perhaps?
                ListOfPeople.Add(person);
            }
        }
        public void WriteToFile()
        {
            string output = "";
            foreach(var person in ListOfPeople)
            {
                output += JsonConvert.SerializeObject(person) + "\n";
            }
            File.WriteAllText((sessionFileName), output);
        }
        void CreateFile()
        {
            var file = File.Create(sessionFileName);
            file.Close();
        }
        #endregion

        #region ToolMethods
        public decimal RoundToTwo(decimal value)
        {
            return Math.Round(value, 2);
        }
        public bool CheckIfDecimal(string value)
        {
            decimal result;
            return Decimal.TryParse(value, out result);
        }

        public decimal AverageMoneySpentPerCapita()
        {
            if (ListOfPeople.Count != 0)
            {
                return RoundToTwo(TotalMoneySpent() / ListOfPeople.Count);
            }
            else
            {
                return 0;
            }
        }
        public decimal TotalMoneySpent()
        {
            return ListOfPeople.Sum(x => x.TotalContributions);
        }
        #endregion

        public void SetCharges()
        {
            foreach (var person in ListOfPeople)
            {
                decimal charge = person.TotalContributions - AverageMoneySpentPerCapita();
                person.SetCharge(RoundToTwo(charge));
            }
        }
    }
}
