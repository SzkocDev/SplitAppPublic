using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFUISplitApp
{
    public partial class DisplayDetailsDialog : Window
    {
        public List<Contribution> Contributions { get; private set; }
        public bool RemovePerson { get; private set; } 

        public DisplayDetailsDialog(List<Contribution> contributions)
        {
            InitializeComponent();
            Contributions = contributions;
            DetailsList.ItemsSource = Contributions;
        }

        private void DetailsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = (sender as ListView).SelectedItem;
            var contribution = listView as Contribution;
            Contributions.Remove(contribution);
            DetailsList.Items.Refresh();
        }

        private void RemovePersonButton_Click(object sender, RoutedEventArgs e)
        {
            RemovePerson = true;
            Close();
        }
    }
}
