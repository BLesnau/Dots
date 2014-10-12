namespace Dots.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dots.Users", "Test", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dots.Users", "Test");
        }
    }
}
