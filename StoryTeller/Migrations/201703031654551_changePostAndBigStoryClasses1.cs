namespace StoryTeller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePostAndBigStoryClasses1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BigStories", "StoryPhoto", c => c.Binary());
            AddColumn("dbo.Posts", "StoryPhoto", c => c.Binary());
            DropColumn("dbo.BigStories", "PostPhoto");
            DropColumn("dbo.Posts", "PostPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "PostPhoto", c => c.Binary());
            AddColumn("dbo.BigStories", "PostPhoto", c => c.Binary());
            DropColumn("dbo.Posts", "StoryPhoto");
            DropColumn("dbo.BigStories", "StoryPhoto");
        }
    }
}
