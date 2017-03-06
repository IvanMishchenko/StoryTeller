using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StoryTeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StoryTeller.Controllers
{

    public class BigStoryController : Controller
    {

        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public BigStoryController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BigStory bigStory = db.BigStories.Find(id);

            if(bigStory == null)
            {
                return HttpNotFound();
            }

            return View(bigStory);
        }
    }
}