﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.Models.Interfaces
{
    public interface IStory
    {
         int Id { get; set; }
         string Title { get; set; }
         string Text { get; set; }
         DateTime Created { get; set; }
         byte[] StoryPhoto { get; set; }

         ApplicationUser User { get; set; }
    }
}
