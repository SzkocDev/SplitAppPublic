using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUISplitApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           // operationalMethods.CreateMockData();
            OperationalMethods.ReadOrCreateSessionFile();
            RefreshMainWindow();
        }

        public void RefreshMainWindow()
        {
            DataOfPeople.ItemsSource = OperationalMethods.ListOfPeople;
            ChoosePeopleComboBox.ItemsSource = OperationalMethods.ListOfPeople.Select(x => x.Name);

            AvgLabel.Content = $"AVG:{OperationalMethods.AverageMoneySpentPerCapita()}";
            OperationalMethods.SetCharges();

            DataOfPeople.Items.Refresh();
            ChoosePeopleComboBox.Items.Refresh();

            OperationalMethods.WriteToFile();
        }
        #region Click Events
        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if(AddPersonTextBox.Text != "")
            {
                string nameToAdd = AddPersonTextBox.Text;
                int numberOfNameOccurences = OperationalMethods.ListOfPeople.Where(element => element.Name == nameToAdd).Count();
                if (numberOfNameOccurences <= 0)
                {
                    OperationalMethods.ListOfPeople.Add(new Person(nameToAdd));
                    AddPersonTextBox.Text = "";
                    RefreshMainWindow();
                }
            }
        }

        private void AddContributionButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContributionValueTextBox.Text != "" && ChoosePeopleComboBox.Text != "" && OperationalMethods.CheckIfDecimal(ContributionValueTextBox.Text))
            {
                decimal value = Convert.ToDecimal(ContributionValueTextBox.Text);
                string description = Convert.ToString(ContributionDesctiptionTextBox.Text);
                int personIndex = OperationalMethods.ListOfPeople.FindIndex(x => x.Name == ChoosePeopleComboBox.Text);
                OperationalMethods.ListOfPeople[personIndex].AddContribution(value, description);

                RefreshMainWindow();

                ContributionDesctiptionTextBox.Text = "";
                ContributionValueTextBox.Text = "";
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)// add to operations 
        {
            var owedPeople = new List<Person>();
            var inDebtPeople = new List<Person>();
            owedPeople = OperationalMethods.ListOfPeople.FindAll(x => x.Charge >= 0);
            inDebtPeople = OperationalMethods.ListOfPeople.FindAll(x => x.Charge < 0);

            DisplayTransfersDialog displayTransfersDialog = new DisplayTransfersDialog(inDebtPeople, owedPeople);
            displayTransfersDialog.Show();
        }

        private void DataOfPeople_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = (sender as ListView).SelectedItem;
            var person = listView as Person;
            var displayDetailsDialog = new DisplayDetailsDialog(person.ListOfContributions);
            displayDetailsDialog.ShowDialog();
            var index = OperationalMethods.ListOfPeople.FindIndex(x => x.Name == person.Name);
            if (displayDetailsDialog.RemovePerson == false)
            {
                OperationalMethods.ListOfPeople[index].ChangeListOfContributions(displayDetailsDialog.Contributions);
            }
            else
            {
                OperationalMethods.ListOfPeople.RemoveAt(index);
            }
            RefreshMainWindow();
        }
        #endregion
    }
}
