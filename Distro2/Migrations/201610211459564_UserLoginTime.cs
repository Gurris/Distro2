namespace Distro2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLoginTime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLoginTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        loginDate = c.DateTime(nullable: false),
                        user_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id, cascadeDelete: true)
                .Index(t => t.user_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLoginTimes", "user_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserLoginTimes", new[] { "user_Id" });
            DropTable("dbo.UserLoginTimes");
        }
    }
}
