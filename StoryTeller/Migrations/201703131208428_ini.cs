namespace StoryTeller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ini : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BigStories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        StoryPhoto = c.Binary(),
                        IsLocked = c.Boolean(nullable: false),
                        WhenLocked = c.DateTime(),
                        Created = c.DateTime(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        MaxNumberOfPosts = c.Int(nullable: false),
                        HoursToWrite = c.Int(nullable: false),
                        Administrator_Id = c.String(maxLength: 128),
                        Category_Id = c.Int(),
                        CurrentUser_Id = c.String(maxLength: 128),
                        UnModeratedPost_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Administrator_Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CurrentUser_Id)
                .ForeignKey("dbo.PartBigStories", t => t.UnModeratedPost_Id)
                .Index(t => t.Administrator_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.CurrentUser_Id)
                .Index(t => t.UnModeratedPost_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StoryTellerName = c.String(),
                        UserPhoto = c.Binary(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        MyUserInfo_Id = c.Int(),
                        BigStory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MyUserInfoes", t => t.MyUserInfo_Id)
                .ForeignKey("dbo.BigStories", t => t.BigStory_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.MyUserInfo_Id)
                .Index(t => t.BigStory_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MyUserInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Likes = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        StoryPhoto = c.Binary(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        Post_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                        BigStory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.BigStories", t => t.BigStory_Id)
                .Index(t => t.Post_Id)
                .Index(t => t.User_Id)
                .Index(t => t.BigStory_Id);
            
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartBigStories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                        BigStory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.BigStories", t => t.BigStory_Id)
                .Index(t => t.User_Id)
                .Index(t => t.BigStory_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.User_Follow",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FollowerID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.FollowerID })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerID)
                .Index(t => t.UserId)
                .Index(t => t.FollowerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BigStories", "UnModeratedPost_Id", "dbo.PartBigStories");
            DropForeignKey("dbo.PartBigStories", "BigStory_Id", "dbo.BigStories");
            DropForeignKey("dbo.PartBigStories", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "BigStory_Id", "dbo.BigStories");
            DropForeignKey("dbo.BigStories", "CurrentUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "BigStory_Id", "dbo.BigStories");
            DropForeignKey("dbo.BigStories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.AspNetUsers", "BigStory_Id", "dbo.BigStories");
            DropForeignKey("dbo.BigStories", "Administrator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Likes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.AspNetUsers", "MyUserInfo_Id", "dbo.MyUserInfoes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.User_Follow", "FollowerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.User_Follow", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.User_Follow", new[] { "FollowerID" });
            DropIndex("dbo.User_Follow", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PartBigStories", new[] { "BigStory_Id" });
            DropIndex("dbo.PartBigStories", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Likes", new[] { "BigStory_Id" });
            DropIndex("dbo.Likes", new[] { "Post_Id" });
            DropIndex("dbo.Likes", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "BigStory_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "BigStory_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "MyUserInfo_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.BigStories", new[] { "UnModeratedPost_Id" });
            DropIndex("dbo.BigStories", new[] { "CurrentUser_Id" });
            DropIndex("dbo.BigStories", new[] { "Category_Id" });
            DropIndex("dbo.BigStories", new[] { "Administrator_Id" });
            DropTable("dbo.User_Follow");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PartBigStories");
            DropTable("dbo.Categories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Likes");
            DropTable("dbo.Comments");
            DropTable("dbo.Posts");
            DropTable("dbo.MyUserInfoes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.BigStories");
        }
    }
}
