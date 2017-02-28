namespace StoryTeller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIStoryV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BigStories", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.BigStories", "User_Id");
            AddForeignKey("dbo.BigStories", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BigStories", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.BigStories", new[] { "User_Id" });
            DropColumn("dbo.BigStories", "User_Id");
        }
    }
}
