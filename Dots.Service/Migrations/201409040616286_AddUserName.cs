namespace Dots.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dots.Users", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dots.Users", "UserName");
        }
    }
}
