namespace biz.dfch.CS.CoffeeTracker.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNameToBaseEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CoffeeOrders", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CoffeeOrders", "Name");
        }
    }
}
