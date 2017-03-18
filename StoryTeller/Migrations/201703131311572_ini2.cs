namespace StoryTeller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ini2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BigStories", "Deadline", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BigStories", "Deadline", c => c.DateTime(nullable: false));
        }
    }
}
