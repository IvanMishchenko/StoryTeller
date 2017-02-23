namespace StoryTeller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostPhotoField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "PostPhoto", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "PostPhoto");
        }
    }
}
