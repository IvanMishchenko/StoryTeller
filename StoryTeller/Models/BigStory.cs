using StoryTeller.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoryTeller.Models
{
    public class BigStory : IStory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Text { get; set; }
        public byte[] PostPhoto { get; set; }
        public int MaxAmountOffUsers { get; set; }
        public bool IsLocked { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        public int MaxNumberOfPosts { get; set; }
        public int HoursToWrite { get; set; }
        public int HoursToDiscuss { get; set; }
        public int MaxAllowedNumberOfDislikes { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationUser CurrentUser { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ApplicationUser> AllUsers { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }

    }
}