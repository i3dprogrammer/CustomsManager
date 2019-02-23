using CustomsManager.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;

namespace CustomsManager.Windows
{
    /// <summary>
    /// Interaction logic for InspectCompany.xaml
    /// </summary>
    public partial class InspectCompany : MetroWindow
    {
        public Customer customer;
        public string filterText = "";


        public InspectCompany()
        {
            InitializeComponent();

            Globals.RecordWindow = this;
        }

        public void ReloadRecords()
        {
            list_records.Items.Clear();
            list_deposits.Items.Clear();

            if (customer == null)
            {
                MessageBox.Show("This customer is already removed from database, cannot update it's operations.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            if (customer.Operations == null)
                return;

            foreach (var item in customer.Operations)
            {
                if (item.Name.Contains(filterText))
                    list_records.Items.Add(item);
            }

            lbl_opCount.Content = "Operations Count: " + customer.Operations.Count();
            lbl_totalCost.Content = "Total Cost: " + customer.TotalCost;
            lbl_totalDeposit.Content = "Total Deposit: " + customer.TotalDeposits;


            if (customer.Deposits == null)
                return;

            foreach(var item in customer.Deposits)
            {
                list_deposits.Items.Add(item);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (customer == null)
                throw new Exception("Company cannot be null whilst inspecting, please refer to the developer.");

            ReloadRecords();

            this.Title = customer.Name + " - Operations";
        }

        private void List_records_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (list_records.SelectedIndex < 0)
                return;

            var operation = (Operation)list_records.SelectedItem;
            var win = new InspectRecord();
            win.customer = customer;
            win.operation = operation;
            win.ShowDialog();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = textBox.Text;
            ReloadRecords();
        }

        private void addRecord_menuClick(object sender, RoutedEventArgs e)
        {
            var win = new AddEditRecord();
            win.customer = customer;
            win.ShowDialog();
        }

        private void editRecord_menuItem(object sender, RoutedEventArgs e)
        {
            if (list_records.SelectedIndex < 0)
                return;

            var win = new AddEditRecord();
            win.customer = customer;
            win.operation = (Operation)list_records.SelectedItem;
            win._name = win.operation.Name;
            win._date = win.operation.Date;
            win.ShowDialog();
        }

        private void deleteRecord_menuItem(object sender, RoutedEventArgs e)
        {
            if (list_records.SelectedIndex < 0)
                return;

            var operation = (Operation)list_records.SelectedItem;

            if (MessageBox.Show("Are you sure you want to remove " + operation.Name + " permanently?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            var cust = Globals.MainContext.Customers.FirstOrDefault(x => x.Id == customer.Id);
            if (cust != null)
            {
                operation = cust.Operations.SingleOrDefault(y => y.Id == operation.Id);
                if (operation != null)
                {
                    operation.Sections.Clear();
                    cust.Operations.Remove(operation);
                    Globals.MainContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("This operation is already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The customer this operation belongs to is already removed from database, closing operations window.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Globals.RefreshTempData();
                this.Close();
            }

            Globals.RefreshTempData();
        }

        private void CheckMenuItems(object sender, RoutedEventArgs e)
        {
            if (list_records.SelectedItems.Count == 0)
            {
                menu_edit.IsEnabled = false;
                menu_delete.IsEnabled = false;
            }
            else
            {
                menu_edit.IsEnabled = true;
                menu_delete.IsEnabled = true;
            }
        }

        private void addDeposit_menuClick(object sender, RoutedEventArgs e)
        {
            var win = new Windows.AddEditDeposit();
            win.customer = customer;
            win.ShowDialog();
        }

        private void editDeposit_menuItem(object sender, RoutedEventArgs e)
        {
            if (list_deposits.SelectedIndex < 0)
                return;

            var win = new Windows.AddEditDeposit();
            win.customer = customer;
            win.deposit = (Deposit)list_deposits.SelectedItem;
            win._bank = win.deposit.Bank;
            win._date = win.deposit.Date;
            win._value = win.deposit.Value;
            win.ShowDialog();
        }

        private void deleteDeposit_menuItem(object sender, RoutedEventArgs e)
        {
            if (list_deposits.SelectedIndex < 0)
                return;

            var deposit = (Deposit)list_deposits.SelectedItem;

            if (MessageBox.Show("Are you sure you want to remove " + $"{deposit.Value} ({deposit.Bank} - {deposit.Date})" + " permanently?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            var cust = Globals.MainContext.Customers.FirstOrDefault(x => x.Id == customer.Id);

            if (cust != null)
            {
                deposit = cust.Deposits.SingleOrDefault(y => y.Id == deposit.Id);
                if (deposit != null)
                {
                    cust.Deposits.Remove(deposit);
                    Globals.MainContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("This deposit is already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The customer this deposit belongs to is already removed from database, closing customer profile.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Globals.RefreshTempData();
                this.Close();
            }

            Globals.RefreshTempData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Globals.RefreshTempData();
        }
    }
}
