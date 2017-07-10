namespace NBUProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserPollQuestions", "PollId", "dbo.UserPolls");
            DropIndex("dbo.UserPollQuestions", new[] { "PollId" });
            AddColumn("dbo.UserPollQuestions", "UserPollId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserPollQuestions", "UserPollId");
            AddForeignKey("dbo.UserPollQuestions", "UserPollId", "dbo.UserPolls", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPollQuestions", "UserPollId", "dbo.UserPolls");
            DropIndex("dbo.UserPollQuestions", new[] { "UserPollId" });
            DropColumn("dbo.UserPollQuestions", "UserPollId");
            CreateIndex("dbo.UserPollQuestions", "PollId");
            AddForeignKey("dbo.UserPollQuestions", "PollId", "dbo.UserPolls", "Id", cascadeDelete: true);
        }
    }
}
