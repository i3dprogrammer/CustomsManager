using MahApps.Metro.Controls;
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
    /// Interaction logic for CodeSearch.xaml
    /// </summary>
    public partial class CodeSearch : MetroWindow
    {
        public CodeSearch()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //OP.USER.YEAR.RULE
            if (!textBox.Text.Contains("."))
                return;
            if (textBox.Text.Split('.').Count() != 3)
                return;
            if (!CheckCode(textBox.Text))
                return;

            var splits = textBox.Text.Split('.');
            try
            {
                int opId = int.Parse(splits[0]);
                int userId = int.Parse(splits[1]);
                int ruleId = int.Parse(splits[2]);

                var user = Globals.MainContext.Customers.First(x => x.Id == userId);
                var op = user.Operations.First(x => x.Id == opId);
                var rule = op.Sections.First(x => x.Id == ruleId);

                //var c1 = Globals.Context.Customers.ToList().Exists(x => x.Id == userId) && Globals.Context.Customers.First(x => x.Id == userId).Records.ToList().Exists(y => y.Id == opId) && Globals.Context.Customers.First(x => x.Id == userId).Records.First(y => y.Id == opId).Rules.ToList().Exists(z => z.Id == ruleId);
                bg_status.Background = Brushes.Green;
                status.Content = "FOUND";
            }
            catch (Exception ex)
            {
                bg_status.Background = Brushes.Red;
                status.Content = "NOT FOUND";
            }
        }

        private bool CheckCode(string str)
        {
            foreach (var c in str)
            {
                if (!char.IsDigit(c) && c != '.')
                    return false;
            }

            return true;
        }
    }
}
