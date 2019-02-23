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
    /// Interaction logic for AddNewCompany.xaml
    /// </summary>
    public partial class AddEditCompany : MetroWindow
    {
        public Customer customer;

        public string _name;
        public int _code;
        public AddEditCompany()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            nud_code.Value = Globals.MainContext.Customers.Count();

            if (customer != null)
            {
                this.Title = "Edit " + customer.Name;
                button.Content = "Edit";
                textBox.Text = customer.Name;
                nud_code.Value = customer.Code;
            }
             
            textBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(nud_code == null)
            {
                MessageBox.Show("Customer code cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if(textBox.Text == null)
            {
                MessageBox.Show("Customer name cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //using(var trans = Globals.MainContext.Database.BeginTransaction())
            //{

            //    Globals.MainContext.Database.ExecuteSqlCommand("SELECT TOP 0 NULL FROM Customers WITH (TABLOCKX)");
            //    trans.Commit();
            //}

            if (customer == null)
            {   //Check if the code ain't used.
                if (Globals.MainContext.Customers.Any(x => x.Code == (int)nud_code.Value.Value))
                {
                    MessageBox.Show("There exists already a customer with that code, please choose a higher value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var cust = new Customer();
                cust.Name = textBox.Text;
                cust.Code = (int)nud_code.Value.Value;
                Globals.MainContext.Customers.Add(cust);
                Globals.MainContext.SaveChanges();
            } else
            {
                var dbValues = Globals.MainContext.Entry(customer).GetDatabaseValues();
                if (dbValues != null)
                {
                    if(dbValues.GetValue<string>("Name") == _name && dbValues.GetValue<int>("Code") == _code)
                    {
                        //If we used a different customer code in editing.
                        if(_code != (int)nud_code.Value.Value)
                        {   //Check if the new code aint used.
                            if (Globals.MainContext.Customers.Any(x => x.Code == (int)nud_code.Value.Value))
                            {
                                MessageBox.Show("There exists already a customer with that code, please choose a higher value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        customer.Name = textBox.Text;
                        customer.Code = (int)nud_code.Value.Value;
                        Globals.MainContext.SaveChanges();
                    } else
                    {   //If database values doesnt match our temp customer values. It has been edited outside of here.
                        MessageBox.Show("This customer has been edited outside of this program, this will close now.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
                else
                {   //If there's no database values for this customer, it has been removed then.
                    MessageBox.Show("This customer is already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Globals.RefreshTempData();
            this.Close();
        }
    }
}
