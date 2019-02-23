using CustomsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomsManager
{
    class Globals
    {
        //public static List<Customer> Customers;
        public static CustomsContext MainContext;
        public static MainWindow MainWindow;
        public static Windows.InspectCompany RecordWindow;
        public static Windows.InspectRecord RulesWindow;

        public static void RefreshTempData()
        {
            if (MainContext != null)
                MainContext.Dispose();

            MainContext = new CustomsContext();
            foreach(var cust in MainContext.Customers)
                MainContext.Entry(cust).Reload();

            try
            {
                MainWindow.ReloadCustomers();
            }
            catch { }

            try
            {
                RulesWindow.customer = MainContext.Customers.SingleOrDefault(x => x.Id == RecordWindow.customer.Id);
                RulesWindow.operation = RulesWindow.customer.Operations.SingleOrDefault(x => x.Id == RulesWindow.operation.Id);
                RulesWindow.ReloadRules();
            }
            catch { }

            try
            {
                RecordWindow.customer = MainContext.Customers.SingleOrDefault(x => x.Id == RecordWindow.customer.Id);
                RecordWindow.ReloadRecords();
            }
            catch { }
        }
    }
}
