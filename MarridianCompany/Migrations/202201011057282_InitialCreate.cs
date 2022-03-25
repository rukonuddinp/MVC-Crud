namespace MarridianCompany.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodDetail",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Image = c.Binary(storeType: "image"),
                        EntryDate = c.DateTime(nullable: false, storeType: "date"),
                        Quantity = c.Long(nullable: false),
                        FoodGroupID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FoodGroup", t => t.FoodGroupID, cascadeDelete: true)
                .Index(t => t.FoodGroupID);
            
            CreateTable(
                "dbo.FoodGroup",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodDetail", "FoodGroupID", "dbo.FoodGroup");
            DropIndex("dbo.FoodDetail", new[] { "FoodGroupID" });
            DropTable("dbo.FoodGroup");
            DropTable("dbo.FoodDetail");
        }
    }
}
