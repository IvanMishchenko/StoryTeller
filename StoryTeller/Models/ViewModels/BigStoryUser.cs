using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryTeller.Models
{
    public class BigStoryUser
    {
        public ApplicationUser loginUser { get; set; }
        public BigStory bigStory{ get; set; }
    }
}