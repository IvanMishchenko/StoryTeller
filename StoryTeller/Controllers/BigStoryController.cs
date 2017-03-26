using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StoryTeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (bigStory == null)
            {
                return HttpNotFound();
            }

            UpdateBigStory(bigStory);

            BigStoryUser bigStoryUser = new BigStoryUser()
            {
                bigStory = bigStory,
                loginUser = currentUser
            };

            return View(bigStoryUser);
        }

        public void UpdateBigStory(BigStory bigStory)
        {
            if (bigStory.WhenLocked != null)
            {
                DateTime timeToFinish = bigStory.WhenLocked.Value.AddHours(bigStory.HoursToWrite);

                if (DateTime.Now.Ticks > timeToFinish.Ticks && bigStory.UnModeratedPost == null)
                {
                    bigStory.IsLocked = false;
                    bigStory.CurrentUser.isWritting = false;
                    bigStory.CurrentUser = null;
                    bigStory.WhenLocked = null;

                    db.SaveChanges();
                }
            }
        }

        [Authorize]
        public ActionResult LockStory(string bigStoryId)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var bigStory = db.BigStories.FirstOrDefault(x => x.Id.ToString() == bigStoryId);
            if (!bigStory.IsLocked && bigStory.UnModeratedPost == null && currentUser.isWritting == false)
            {
                currentUser.isWritting = true;
                bigStory.IsLocked = true;
                bigStory.CurrentUser = currentUser;
                bigStory.WhenLocked = DateTime.Now;
                db.SaveChanges();
                return PartialView("~/Views/BigStory/Partial/_WriteSection.cshtml");
            }

            return RedirectToAction("Index", bigStoryId);
        }

        [Authorize]
        public ActionResult Write(string text, string bigStoryId)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var bigStory = db.BigStories.FirstOrDefault(x => x.Id.ToString() == bigStoryId);
            UpdateBigStory(bigStory);

            if (bigStory.IsLocked == true && bigStory.CurrentUser != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PartBigStory post = new PartBigStory()
            {
                User = currentUser,
                Created = DateTime.Now,
                Text = text,
            };

            currentUser.isWritting = false;
            bigStory.UnModeratedPost = post;
            db.SaveChanges();

            return null;
        }

    }
}