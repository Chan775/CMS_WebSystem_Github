namespace CMS_WebSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Booking_tbl",
                c => new
                    {
                        Bk_Id = c.Int(nullable: false, identity: true),
                        Sch_Id = c.Int(nullable: false),
                        Bk_Date = c.DateTime(nullable: false, storeType: "date"),
                        Agent_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Bk_Id);
            
            CreateTable(
                "dbo.BookingItem_tbl",
                c => new
                    {
                        BI_Id = c.Int(nullable: false, identity: true),
                        Bk_Id = c.Int(nullable: false),
                        Itm_Id = c.Int(nullable: false),
                        Container_No = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BI_Id);
            
            CreateTable(
                "dbo.Customer_tbl",
                c => new
                    {
                        Cust_Id = c.Int(nullable: false, identity: true),
                        Cust_Name = c.String(nullable: false, maxLength: 100),
                        Cust_Contact = c.Int(nullable: false),
                        Cust_EmailAddress = c.String(nullable: false, maxLength: 100),
                        Cust_Address = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Cust_Id);
            
            CreateTable(
                "dbo.Item_tbl",
                c => new
                    {
                        Itm_Id = c.Int(nullable: false, identity: true),
                        Cust_Id = c.Int(nullable: false),
                        Itm_Name = c.String(nullable: false, maxLength: 50),
                        Itm_Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Itm_Id);
            
            CreateTable(
                "dbo.Schedule_tbl",
                c => new
                    {
                        Sch_Id = c.Int(nullable: false, identity: true),
                        Departure_Location = c.String(nullable: false, maxLength: 50),
                        Arrival_Location = c.String(nullable: false, maxLength: 50),
                        Dept_DateTime = c.DateTime(nullable: false),
                        Arrvl_DateTime = c.DateTime(nullable: false),
                        Ship_Id = c.Int(nullable: false),
                        Available_Container = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Sch_Id);
            
            CreateTable(
                "dbo.Ship_tbl",
                c => new
                    {
                        Ship_Id = c.Int(nullable: false, identity: true),
                        Ship_Name = c.String(nullable: false, maxLength: 50),
                        Container_Limit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Ship_Id);
            
            CreateTable(
                "dbo.User_tbl",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 10),
                        Type = c.String(nullable: false, maxLength: 50),
                        Contact = c.Int(nullable: false),
                        EmailAddress = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User_tbl");
            DropTable("dbo.Ship_tbl");
            DropTable("dbo.Schedule_tbl");
            DropTable("dbo.Item_tbl");
            DropTable("dbo.Customer_tbl");
            DropTable("dbo.BookingItem_tbl");
            DropTable("dbo.Booking_tbl");
        }
    }
}
