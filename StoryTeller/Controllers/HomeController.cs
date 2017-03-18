using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StoryTeller.Models;
using StoryTeller.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace StoryTeller.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;


        public HomeController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            UpdateExpiredStories();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        const int storyPerPage = 8;

        public ActionResult Index(int? id)
        {
            var user = manager.FindById(User.Identity.GetUserId());


            var page = id ?? 0;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Home/Partial/_Stories.cshtml", GetPaginatedStories(page));
            }
            List<IStory> listOfStories = db.Posts.AsEnumerable().Where(x => user.Following.Contains(x.User)).ToList<IStory>();

            listOfStories.AddRange(db.BigStories.ToList<BigStory>().Where(x => x.IsLocked == false));

            return View("Index", listOfStories.OrderByDescending(x => x.Created).Take(storyPerPage));
        }

        private List<IStory> GetPaginatedStories(int page = 1)
        {
            var user = manager.FindById(User.Identity.GetUserId());
            var skipRecords = page * storyPerPage;

            List<IStory> listOfStories = db.Posts.AsEnumerable().Where(x => user.Following.Contains(x.User)).ToList<IStory>();
            listOfStories.AddRange(db.BigStories.ToList<BigStory>().Where(x => x.IsLocked == false));

            return listOfStories.
                OrderByDescending(x => x.Created).
                Skip(skipRecords).
                Take(storyPerPage).ToList();
        }

        public void UpdateExpiredStories()
        {
            var expiredStory = (from story in db.BigStories
                                where story.CurrentUser != null &&
                                DateTime.Now > EntityFunctions.AddMinutes(story.WhenLocked, story.HoursToWrite) &&
                                story.UnModeratedPost == null
                                select story).ToList();

            if (expiredStory.Any())
            {
                foreach (var story in expiredStory)
                {
                    story.CurrentUser.isWritting = false;
                    story.CurrentUser = null;
                    story.WhenLocked = null;
                    story.IsLocked = false;
                }

                db.SaveChanges();
            }
        }


    }
}