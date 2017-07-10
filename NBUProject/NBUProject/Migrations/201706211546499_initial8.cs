namespace NBUProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SentEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Date = c.DateTime(nullable: false),
                        PollId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Polls", t => t.PollId, cascadeDelete: true)
                .Index(t => t.PollId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SentEmails", "PollId", "dbo.Polls");
            DropIndex("dbo.SentEmails", new[] { "PollId" });
            DropTable("dbo.SentEmails");
        }
    }
}
