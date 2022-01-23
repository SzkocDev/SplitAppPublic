using System;
using System.Collections.Generic;
using System.Text;
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
    /// <summary>
    /// Interaction logic for DisplayTransfersDialog.xaml
    /// </summary>

    public partial class DisplayTransfersDialog : Window
    {
        List<TransferInfo> debtPeople = new List<TransferInfo>();
        List<TransferInfo> owedPeople = new List<TransferInfo>();
        List<Transfer> Transfers = new List<Transfer>();

        public DisplayTransfersDialog(List<Person> debt, List<Person> owed)
        {                      
            InitializeComponent();
            foreach (Person person in debt)
            {
                debtPeople.Add(new TransferInfo(person));
            }
            foreach (Person person in owed)
            {
                owedPeople.Add(new TransferInfo(person));
            }
            CreateTransfers();

            TransfersList.ItemsSource = Transfers;
            TransfersList.Items.Refresh();

        }
        void CreateTransfers()
        {
            foreach(TransferInfo personOwed in owedPeople)
            {
                if (personOwed.currentValue > 0)
                {
                    foreach (TransferInfo personDebt in debtPeople)
                    {
                        if (personDebt.currentValue * -1 > 0)
                        {
                            if (personOwed.currentValue > personDebt.currentValue * -1)
                            {
                                if(personDebt.currentValue != 0)
                                {
                                    Transfers.Add(new Transfer(personDebt.name, personOwed.name, personDebt.currentValue * -1));
                                    personOwed.currentValue += personDebt.currentValue;
                                    personDebt.currentValue = 0;
                                }
                            }
                            else
                            {
                                if (personOwed.currentValue != 0)
                                {
                                    Transfers.Add(new Transfer(personDebt.name, personOwed.name, personOwed.currentValue));
                                    personOwed.currentValue = 0;
                                    personDebt.currentValue = personOwed.currentValue + personDebt.currentValue;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
