namespace Dots.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAllUsers : DbMigration
    {
        public override void Up()
        {
            DropColumn("dots.Users", "UserId");
            DropColumn("dots.Users", "UserName");
            DropColumn("dots.Users", "Authenticator");
        }
        
        public override void Down()
        {
            AddColumn("dots.Users", "Authenticator", c => c.String());
            AddColumn("dots.Users", "UserName", c => c.String());
            AddColumn("dots.Users", "UserId", c => c.String());
        }
    }
}
