namespace StoryTeller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLikesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.String(maxLength: 128),
                        Post_Id = c.Int(),
                        BigStory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .ForeignKey("dbo.BigStories", t => t.BigStory_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Post_Id)
                .Index(t => t.BigStory_Id);
            
            DropColumn("dbo.BigStories", "Likes");
            DropColumn("dbo.BigStories", "Dislakes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BigStories", "Dislakes", c => c.Int(nullable: false));
            AddColumn("dbo.BigStories", "Likes", c => c.Int(nullable: false));
            DropForeignKey("dbo.Likes", "BigStory_Id", "dbo.BigStories");
            DropForeignKey("dbo.Likes", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Likes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Likes", new[] { "BigStory_Id" });
            DropIndex("dbo.Likes", new[] { "Post_Id" });
            DropIndex("dbo.Likes", new[] { "User_Id" });
            DropTable("dbo.Likes");
        }
    }
}
