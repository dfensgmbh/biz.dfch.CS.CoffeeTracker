namespace biz.dfch.CS.CoffeeTracker.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustedUserAddedCoffeeAndCoffeeOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Statistics", "CoffeeMachineId", "dbo.CoffeeMachines");
            DropForeignKey("dbo.Statistics", "UserId", "dbo.Users");
            DropIndex("dbo.Statistics", new[] { "UserId" });
            DropIndex("dbo.Statistics", new[] { "CoffeeMachineId" });
            CreateTable(
                "dbo.CoffeeOrders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CoffeeId = c.Long(nullable: false),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coffees", t => t.CoffeeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CoffeeId);
            
            CreateTable(
                "dbo.Coffees",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Brand = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        LastDelivery = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Password", c => c.String(nullable: false));
            DropTable("dbo.CoffeeMachines");
            DropTable("dbo.Statistics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CoffeeMachineId = c.Long(nullable: false),
                        CoffeesCount = c.Long(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CoffeeMachines",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Brand = c.String(),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CoffeeOrders", "UserId", "dbo.Users");
            DropForeignKey("dbo.CoffeeOrders", "CoffeeId", "dbo.Coffees");
            DropIndex("dbo.CoffeeOrders", new[] { "CoffeeId" });
            DropIndex("dbo.CoffeeOrders", new[] { "UserId" });
            DropColumn("dbo.Users", "Password");
            DropTable("dbo.Coffees");
            DropTable("dbo.CoffeeOrders");
            CreateIndex("dbo.Statistics", "CoffeeMachineId");
            CreateIndex("dbo.Statistics", "UserId");
            AddForeignKey("dbo.Statistics", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Statistics", "CoffeeMachineId", "dbo.CoffeeMachines", "Id", cascadeDelete: true);
        }
    }
}
