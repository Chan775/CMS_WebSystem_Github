namespace CMS_WebSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrateName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Booking_tbl", "Bk_Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.User_tbl", "Password", c => c.String(maxLength: 10));
            AlterColumn("dbo.User_tbl", "Type", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User_tbl", "Type", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User_tbl", "Password", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Booking_tbl", "Bk_Date", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
