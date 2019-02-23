using CustomsManager.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
//using System.Data;
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
    /// Interaction logic for InspectRecord.xaml
    /// </summary>
    public partial class InspectRecord : MetroWindow
    {
        public Customer customer;
        public Operation operation;
        public string filterText = "";

        public InspectRecord()
        {
            InitializeComponent();

            Globals.RulesWindow = this;
        }

        public void ReloadRules()
        {
            list_rules.Items.Clear();

            if (customer == null)
            {
                MessageBox.Show("This customer is already removed from database, cannot update it's sections.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            if (operation == null)
            {
                MessageBox.Show("This operation is already removed from database, cannot update it's sections.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            if (operation.Sections == null)
                return;

            foreach (var item in operation.Sections)
            {
                if (item.Name.Contains(filterText))
                    list_rules.Items.Add(item);
            }

            lbl_total.Content = "Total Cost : " + operation.Total;
        }

        private void List_rules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("This is only for testing purposes, there's no file is being saved.");
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (customer == null)
                throw new Exception("Company cannot be null whilest inspecting, please refer to the developer.");

            ReloadRules();

            this.Title = "Inspecting " + operation.Name + " - " + customer.Name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Globals.RefreshTempData();
        }

        private void saveChanges(object sender, RoutedEventArgs e)
        {
            Globals.MainContext.SaveChanges();
            Globals.RefreshTempData();
        }

        private void selectReceipt(object sender, RoutedEventArgs e)
        {
            if (list_rules.SelectedIndex < 0)
                return;

            var section = (Models.Section)list_rules.SelectedItem;

            if (section.Receipt != null)
            {
                if (MessageBox.Show("Are you sure you want to replace " + section.Name + " receipt?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;
            }

            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                section.FullPath = ofd.FileName;

                section.Receipt = new PDF() { SectionData = System.IO.File.ReadAllBytes(section.FullPath) };
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.RefreshTempData();
        }
    }
}
