namespace Dots.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTest2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dots.Users", "Test2");
        }
        
        public override void Down()
        {
            AddColumn("dots.Users", "Test2", c => c.String());
        }
    }
}
