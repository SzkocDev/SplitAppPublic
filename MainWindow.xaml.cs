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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OperationalMethods operationalMethods = new OperationalMethods();
        public MainWindow()
        {
            InitializeComponent();
           // operationalMethods.CreateMockData();
            operationalMethods.ReadOrCreateSessionFile();
            RefreshMainWindow();
        }

        public void RefreshMainWindow()
        {
            DataOfPeople.ItemsSource = operationalMethods.ListOfPeople;
            ChoosePeopleComboBox.ItemsSource = operationalMethods.ListOfPeople.Select(x => x.Name);

            AvgLabel.Content = $"AVG:{operationalMethods.AverageMoneySpentPerCapita()}";
            operationalMethods.SetCharges();

            DataOfPeople.Items.Refresh();
            ChoosePeopleComboBox.Items.Refresh();

            operationalMethods.WriteToFile();
        }
        #region Click Events
        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if(AddPersonTextBox.Text != "")
            {
                string nameToAdd = AddPersonTextBox.Text;
                int numberOfNameOccurences = operationalMethods.ListOfPeople.Where(element => element.Name == nameToAdd).Count();
                if (numberOfNameOccurences <= 0)
                {
                    operationalMethods.ListOfPeople.Add(new Person(nameToAdd));
                    AddPersonTextBox.Text = "";
                    RefreshMainWindow();
                }
            }
        }

        private void AddContributionButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContributionValueTextBox.Text != "" && ChoosePeopleComboBox.Text != "" && operationalMethods.CheckIfDecimal(ContributionValueTextBox.Text))
            {
                decimal value = Convert.ToDecimal(ContributionValueTextBox.Text);
                string description = Convert.ToString(ContributionDesctiptionTextBox.Text);
                int personIndex = operationalMethods.ListOfPeople.FindIndex(x => x.Name == ChoosePeopleComboBox.Text);
                operationalMethods.ListOfPeople[personIndex].AddContribution(value, description);

                RefreshMainWindow();

                ContributionDesctiptionTextBox.Text = "";
                ContributionValueTextBox.Text = "";
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)// add to operations 
        {
            var owedPeople = new List<Person>();
            var inDebtPeople = new List<Person>();
            owedPeople = operationalMethods.ListOfPeople.FindAll(x => x.Charge >= 0);
            inDebtPeople = operationalMethods.ListOfPeople.FindAll(x => x.Charge < 0);

            DisplayTransfersDialog displayTransfersDialog = new DisplayTransfersDialog(inDebtPeople, owedPeople);
            displayTransfersDialog.Show();
        }

        private void DataOfPeople_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = (sender as ListView).SelectedItem;
            var person = listView as Person;
            var displayDetailsDialog = new DisplayDetailsDialog(person.ListOfContributions);
            displayDetailsDialog.ShowDialog();
            var index = operationalMethods.ListOfPeople.FindIndex(x => x.Name == person.Name);
            if (displayDetailsDialog.RemovePerson == false)
            {
                operationalMethods.ListOfPeople[index].ChangeListOfContributions(displayDetailsDialog.Contributions);
            }
            else
            {
                operationalMethods.ListOfPeople.RemoveAt(index);
            }
            RefreshMainWindow();
        }
        #endregion
    }
}
