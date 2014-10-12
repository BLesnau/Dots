namespace Dots.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dots.Users", "Test");
        }
        
        public override void Down()
        {
            AddColumn("dots.Users", "Test", c => c.String());
        }
    }
}
