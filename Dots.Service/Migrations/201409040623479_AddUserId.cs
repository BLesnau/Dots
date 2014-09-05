namespace Dots.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dots.Users", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dots.Users", "UserId");
        }
    }
}
