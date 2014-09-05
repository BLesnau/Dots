namespace Dots.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAuth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dots.Users", "Authenticator", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dots.Users", "Authenticator");
        }
    }
}
