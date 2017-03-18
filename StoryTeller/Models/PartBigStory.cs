using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryTeller.Models
{
    public class PartBigStory
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}