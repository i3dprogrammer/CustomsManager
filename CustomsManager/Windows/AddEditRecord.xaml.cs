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

namespace CustomsManager.Windows
{
    /// <summary>
    /// Interaction logic for AddEditRecord.xaml
    /// </summary>
    public partial class AddEditRecord : MetroWindow
    {
        public Customer customer;
        public Operation operation;
        public string _name;
        public DateTime _date;

        public AddEditRecord()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = DateTime.Now;

            if (operation != null)
            {
                this.Title = "Edit " + operation.Name;
                button.Content = "Edit";
                tb_name.Text = operation.Name;
                datePicker.SelectedDate = operation.Date;
            }

            tb_name.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate == null)
            {
                MessageBox.Show("Date cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var cust = Globals.MainContext.Customers.FirstOrDefault(x => x.Id == customer.Id);
            if (cust == null)
            {
                MessageBox.Show("This customer is already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (operation == null)
            {
                var op = new Operation();
                op.Name = tb_name.Text;
                op.Date = datePicker.SelectedDate.Value;
                if (cust.Operations == null)
                    cust.Operations = new List<Operation>();
                op.Number = cust.Operations.Where(x => x.Date.Year == datePicker.SelectedDate.Value.Year).Count() + 1;
                op.InitializeNewOperation();
                cust.Operations.Add(op);
                Globals.MainContext.SaveChanges();
            }
            else
            {
                var dbValues = Globals.MainContext.Entry(operation).GetDatabaseValues();
                if (dbValues != null)
                {
                    if (dbValues.GetValue<string>("Name") == _name && dbValues.GetValue<DateTime>("Date") == _date)
                    {
                        operation.Name = tb_name.Text;
                        operation.Date = _date;
                        Globals.MainContext.SaveChanges();
                    }
                    else
                        MessageBox.Show("This operation has been edited outside of this program, this will close now.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("This operation is already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Globals.RefreshTempData();
            this.Close();
        }
    }
}
