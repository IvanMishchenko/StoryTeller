namespace StoryTeller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePostAndBigStoryClasses : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BigStories", "Subtitle");
            DropColumn("dbo.BigStories", "MaxAmountOffUsers");
            DropColumn("dbo.BigStories", "HoursToDiscuss");
            DropColumn("dbo.BigStories", "MaxAllowedNumberOfDislikes");
            DropColumn("dbo.Posts", "Subtitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "Subtitle", c => c.String());
            AddColumn("dbo.BigStories", "MaxAllowedNumberOfDislikes", c => c.Int(nullable: false));
            AddColumn("dbo.BigStories", "HoursToDiscuss", c => c.Int(nullable: false));
            AddColumn("dbo.BigStories", "MaxAmountOffUsers", c => c.Int(nullable: false));
            AddColumn("dbo.BigStories", "Subtitle", c => c.String());
        }
    }
}
