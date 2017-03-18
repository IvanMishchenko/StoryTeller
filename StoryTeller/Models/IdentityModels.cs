using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI;
using System.Collections.Generic;
using StoryTeller.Models.Interfaces;

namespace StoryTeller.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual MyUserInfo MyUserInfo { get; set; }
        public string StoryTellerName { get; set; }
        public bool isWritting { get;set;}
        public byte[] UserPhoto { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ApplicationUser> Followers { get; set; }
        public virtual ICollection<ApplicationUser> Following { get; set; }
        public virtual ICollection<IStory> Bookmarks { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim("StoryTellerName", this.StoryTellerName));
            return userIdentity;
        }
    }

    public class MyUserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasMany(m => m.Followers).WithMany(p => p.Following).Map(w => w.ToTable("User_Follow").MapLeftKey("UserId").MapRightKey("FollowerID"));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BigStory> BigStories { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<MyUserInfo> myUserInfos { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<PartBigStory> PartsBigStory { get; set; }


    }
}