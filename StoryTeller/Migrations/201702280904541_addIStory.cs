namespace StoryTeller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIStory : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Stories", newName: "BigStories");
            RenameColumn(table: "dbo.AspNetUsers", name: "Story_Id", newName: "BigStory_Id");
            RenameColumn(table: "dbo.Comments", name: "Story_Id", newName: "BigStory_Id");
            RenameColumn(table: "dbo.Posts", name: "Story_Id", newName: "BigStory_Id");
            RenameIndex(table: "dbo.Comments", name: "IX_Story_Id", newName: "IX_BigStory_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_Story_Id", newName: "IX_BigStory_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Story_Id", newName: "IX_BigStory_Id");
            AddColumn("dbo.BigStories", "Text", c => c.String());
            AddColumn("dbo.BigStories", "PostPhoto", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BigStories", "PostPhoto");
            DropColumn("dbo.BigStories", "Text");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_BigStory_Id", newName: "IX_Story_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_BigStory_Id", newName: "IX_Story_Id");
            RenameIndex(table: "dbo.Comments", name: "IX_BigStory_Id", newName: "IX_Story_Id");
            RenameColumn(table: "dbo.Posts", name: "BigStory_Id", newName: "Story_Id");
            RenameColumn(table: "dbo.Comments", name: "BigStory_Id", newName: "Story_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "BigStory_Id", newName: "Story_Id");
            RenameTable(name: "dbo.BigStories", newName: "Stories");
        }
    }
}
