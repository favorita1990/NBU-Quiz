namespace NBUProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserPollAnswers", "PollQuastionId", "dbo.UserPollQuestions");
            DropIndex("dbo.UserPollAnswers", new[] { "PollQuastionId" });
            AddColumn("dbo.UserPollAnswers", "UserPollQuastionId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserPollAnswers", "UserPollQuastionId");
            AddForeignKey("dbo.UserPollAnswers", "UserPollQuastionId", "dbo.UserPollQuestions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPollAnswers", "UserPollQuastionId", "dbo.UserPollQuestions");
            DropIndex("dbo.UserPollAnswers", new[] { "UserPollQuastionId" });
            DropColumn("dbo.UserPollAnswers", "UserPollQuastionId");
            CreateIndex("dbo.UserPollAnswers", "PollQuastionId");
            AddForeignKey("dbo.UserPollAnswers", "PollQuastionId", "dbo.UserPollQuestions", "Id", cascadeDelete: true);
        }
    }
}
