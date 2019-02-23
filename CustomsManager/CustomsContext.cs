using CustomsManager.Models;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomsManager
{
    class CustomsContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        //public DbSet<Models.Company> Companies { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    MessageBox.Show("DAFUQ!");
        //    var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<CustomsContext>(modelBuilder);
        //    Database.SetInitializer(sqliteConnectionInitializer);
        //    Console.WriteLine("AM HERE!");
        //}
    }
}
