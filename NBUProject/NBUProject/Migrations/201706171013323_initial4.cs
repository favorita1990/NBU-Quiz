namespace NBUProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PollQuastions", "UserPoll_Id", "dbo.UserPolls");
            DropIndex("dbo.PollQuastions", new[] { "UserPoll_Id" });
            DropColumn("dbo.PollQuastions", "UserPoll_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PollQuastions", "UserPoll_Id", c => c.Int());
            CreateIndex("dbo.PollQuastions", "UserPoll_Id");
            AddForeignKey("dbo.PollQuastions", "UserPoll_Id", "dbo.UserPolls", "Id");
        }
    }
}
