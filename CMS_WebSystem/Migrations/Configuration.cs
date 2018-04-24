namespace CMS_WebSystem.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CMS_WebSystem.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<CMS_WebSystem.Models.CMSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CMS_WebSystem.Models.CMSContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var users = new List<User_tbl>
            {
                new User_tbl { Name = "John",   Password = "John123", Type="Admin",Contact=169904179,EmailAddress="john@hotmail.com" },
                new User_tbl { Name = "Ali bin Abi",   Password = "Ali123", Type="Agent",Contact=129388775,EmailAddress="ali@hotmail.com" },
                new User_tbl { Name = "Tsukiya",   Password = "tsukiya123", Type="Agent",Contact=897654167,EmailAddress="tsukiya@hotmail.com" }               
            };
            users.ForEach(s => context.User_tbl.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var ships = new List<Ship_tbl>
            {
                new Ship_tbl {Ship_Name="Sunny101", Container_Limit=5 },
                new Ship_tbl {Ship_Name="Burning102", Container_Limit= 6},
                new Ship_tbl {Ship_Name="Titanic103", Container_Limit=7 },
                new Ship_tbl {Ship_Name="Mercy104", Container_Limit=4 },
                new Ship_tbl {Ship_Name="GGWP105", Container_Limit=8 },
                new Ship_tbl {Ship_Name="Loyal106", Container_Limit= 9},
                new Ship_tbl {Ship_Name="Winning107", Container_Limit= 6},
                new Ship_tbl {Ship_Name="Queen108", Container_Limit= 7},
                new Ship_tbl {Ship_Name="King109", Container_Limit=8 },
                new Ship_tbl {Ship_Name="Knight110", Container_Limit= 10}
            };
            ships.ForEach(s => context.Ship_tbl.AddOrUpdate(p => p.Ship_Name, s));
            context.SaveChanges();

            var customers = new List<Customer_tbl>
            {
                new Customer_tbl {Cust_Name="Alexander2",Cust_Contact=234567534 , Cust_EmailAddress="alex@hotmail.com", Cust_Address="Level 13, Lingkaran Syed Putra, Mid Valley City, 59200 KL"},
                new Customer_tbl {Cust_Name="Lancelot",Cust_Contact=876756755 , Cust_EmailAddress="lance@hotmail.com", Cust_Address="28-01, Jalan Molek 1/10, Taman Molek Taman, 81100 Johor Bahru"}
            };
            customers.ForEach(s => context.Customer_tbl.AddOrUpdate(p => p.Cust_Name, s));
            context.SaveChanges();

        }
    }
}
