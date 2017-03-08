namespace biz.dfch.CS.CoffeeTracker.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserModelChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CoffeeOrders", "UserId", "dbo.Users");
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Password = c.String(nullable: false),
                        AspNetUserId = c.String(maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUserId)
                .Index(t => t.AspNetUserId);
            
            AddForeignKey("dbo.CoffeeOrders", "UserId", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CoffeeOrders", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUsers", "AspNetUserId", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUsers", new[] { "AspNetUserId" });
            DropTable("dbo.ApplicationUsers");
            AddForeignKey("dbo.CoffeeOrders", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
