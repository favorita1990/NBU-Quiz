namespace NBUProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Polls", "Url");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Polls", "Url", c => c.String());
        }
    }
}
