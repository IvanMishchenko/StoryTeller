using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryTeller.Models
{
    public class PostsUser
    {
        public ApplicationUser userToSubsribe { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}