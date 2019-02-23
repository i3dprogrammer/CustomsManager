using CustomsManager.Models;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddEditRule.xaml
    /// </summary>
    public partial class AddEditDeposit : MetroWindow
    {
        public Customer customer;
        public Deposit deposit;

        public string _bank;
        public float _value;
        public DateTime _date;

        public AddEditDeposit()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = DateTime.Now;
            if (deposit != null)
            {
                this.Title = "Edit " + customer.Name + " Deposit";
                btn_add.Content = "Edit";
                nud_value.Value = deposit.Value;
                tb_bank.Text = deposit.Bank;
                datePicker.SelectedDate = deposit.Date;
            }

            nud_value.Focus();
        }

        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            if(datePicker.SelectedDate == null)
            {
                MessageBox.Show("Date cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(nud_value.Value == null)
            {
                MessageBox.Show("Deposit value cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var cust = Globals.MainContext.Customers.FirstOrDefault(x => x.Id == customer.Id);
            if (cust == null)
            {
                MessageBox.Show("This customer is already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(deposit == null)
            {
                var d = new Deposit();
                d.Bank = tb_bank.Text;
                d.Date = datePicker.SelectedDate.Value;
                d.Value = (float)nud_value.Value.Value;

                if (cust.Deposits == null)
                    cust.Deposits = new List<Deposit>();

                cust.Deposits.Add(d);
                Globals.MainContext.SaveChanges();
            } else
            {
                var dbValues = Globals.MainContext.Entry(deposit).GetDatabaseValues();
                if(dbValues != null)
                {
                    if(dbValues.GetValue<string>("Bank") == _bank && dbValues.GetValue<DateTime>("Date") == _date && dbValues.GetValue<float>("Value") == _value)
                    {
                        deposit.Bank = tb_bank.Text;
                        deposit.Date = datePicker.SelectedDate.Value;
                        deposit.Value = (float)nud_value.Value.Value;
                        Globals.MainContext.SaveChanges();
                    } else
                    {
                        MessageBox.Show("This deposit has been edited outside of this program, this will close now.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                } else
                {
                    MessageBox.Show("This deposit is already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Globals.RefreshTempData();
            this.Close();

            //if (section == null)
            //{
            //    var r = new Models.Section();
            //    r.Name = tb_name.Text;
            //    r.SectionData = System.IO.File.ReadAllBytes(tb_path.Text);
            //    if (op.Sections == null)
            //        op.Sections = new List<Models.Section>();
            //    r.SectionNo = op.Sections.Count + 1;
            //    op.Sections.Add(r);
            //    Globals.MainContext.SaveChanges();
            //}
            //else
            //{
            //    var dbValues = Globals.MainContext.Entry(section).GetDatabaseValues();
            //    if (dbValues != null)
            //    {
            //        if (dbValues.GetValue<string>("Name") == _name && dbValues.GetValue<byte[]>("SectionData").Length == _length)
            //        {
            //            section.Name = tb_name.Text;
            //            section.SectionData = System.IO.File.ReadAllBytes(tb_path.Text);
            //            Globals.MainContext.SaveChanges();
            //        } else
            //            MessageBox.Show("This section has been edited outside of this program, this will close now.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    else
            //        MessageBox.Show("This section is already removed from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            //Globals.RefreshTempData();
            //this.Close();
        }
    }
}
