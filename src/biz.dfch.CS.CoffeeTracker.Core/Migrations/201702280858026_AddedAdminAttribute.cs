namespace biz.dfch.CS.CoffeeTracker.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAdminAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "IsAdmin");
        }
    }
}
