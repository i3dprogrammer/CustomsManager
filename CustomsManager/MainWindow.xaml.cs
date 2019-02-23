using CustomsManager.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string filterText = "";
        public MainWindow()
        {
            InitializeComponent();
            Globals.MainWindow = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.RefreshTempData();
        }

        public void ReloadCustomers()
        {
            list_customers.Items.Clear();
            foreach (var item in Globals.MainContext.Customers)
            {
                if (item.Name.Contains(filterText))
                    list_customers.Items.Add(item);
            }
        }

        private Customer GetCustomer()
        {
            var record1 = new Models.Operation() { Name = "Record1" };
            var record2 = new Models.Operation() { Name = "Record2" };
            record1.Date = DateTime.Now;
            record2.Date = DateTime.Now;

            record1.InitializeNewOperation();
            record2.InitializeNewOperation();

            record1.Sections.ElementAt(0).Receipt = new PDF() { SectionData = System.IO.File.ReadAllBytes(@"F:\College\Syllabus\1st year & second year\DE\Differential Equations 1.pdf") };
            record1.Sections.ElementAt(1).Receipt = new PDF() { SectionData = System.IO.File.ReadAllBytes(@"F:\College\Syllabus\1st year & second year\DE\Differential Equations 2.pdf") };

            record2.Sections.ElementAt(0).Receipt = new PDF() { SectionData = System.IO.File.ReadAllBytes(@"F:\College\Syllabus\1st year & second year\DE\Differential Equations 3.pdf") };
            record2.Sections.ElementAt(1).Receipt = new PDF() { SectionData = System.IO.File.ReadAllBytes(@"F:\College\Syllabus\1st year & second year\DE\Differential Equations 4.pdf") };
            var company = new Models.Customer() { Name = "A test company" };
            company.Operations = new List<Models.Operation>();

            company.Operations.Add(record1);
            company.Operations.Add(record2);

            return company;
        }

        private void CheckMenuItems(object sender, RoutedEventArgs e)
        {
            if (list_customers.SelectedItems.Count == 0)
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

        private void addNewCustomer_menuClick(object sender, RoutedEventArgs e)
        {
            Windows.AddEditCompany win = new Windows.AddEditCompany();
            win.ShowDialog();
        }

        private void editCustomer_menuClick(object sender, RoutedEventArgs e)
        {
            if (list_customers.SelectedIndex < 0)
                return;

            Windows.AddEditCompany win = new Windows.AddEditCompany();
            win.customer = (Customer)list_customers.SelectedItem;
            win._name = win.customer.Name;
            win._code = win.customer.Code;
            win.ShowDialog();
        }

        private void deleteCustomer_menuClick(object sender, RoutedEventArgs e)
        {
            if (list_customers.SelectedIndex < 0)
                return;

            var customer = (Customer)list_customers.SelectedItem;

            if (MessageBox.Show("Are you sure you want to remove " + customer.Name + " permanently?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            customer = Globals.MainContext.Customers.FirstOrDefault(x => x.Id == customer.Id);

            if (customer != null)
            {
                customer.Operations.Clear();
                Globals.MainContext.Customers.Remove(customer);
                Globals.MainContext.SaveChanges();
                Globals.RefreshTempData();
            } else
            {
                MessageBox.Show("Customer already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = textBox.Text;
            ReloadCustomers();
        }

        private void List_customers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (list_customers.SelectedIndex < 0)
                return;

            var customer = (Customer)list_customers.SelectedItem;
            var win = new Windows.InspectCompany();
            win.customer = customer;
            win.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new Windows.CodeSearch();
            win.ShowDialog();
        }

        private void RefreshCustomers(object sender, RoutedEventArgs e)
        {
            Globals.RefreshTempData();
        }
    }
}
