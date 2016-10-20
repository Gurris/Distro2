namespace Distro2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedAttrubute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageModels", "removed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessageModels", "removed");
        }
    }
}
