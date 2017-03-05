using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StoryTeller.Models;
using StoryTeller.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StoryTeller.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        const int storyPerPage = 8;

        public HomeController()
        {   
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
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

        public ActionResult Index(int? id)
        {
            var user = manager.FindById(User.Identity.GetUserId());

            var page = id ?? 0;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Home/Partial/_Stories.cshtml", GetPaginatedStories(page));
            }

            List<IStory> listOfStories = db.Posts.ToList<IStory>().Where(x => user.Following.Contains(x.User)).ToList();
            listOfStories.AddRange(db.BigStories.ToList<BigStory>().Where(x =>x.IsLocked == false ));

            return View("Index", listOfStories.OrderByDescending(x=>x.Created).Take(storyPerPage));
        }

        private List<IStory> GetPaginatedStories(int page = 1)
        {
            var user = manager.FindById(User.Identity.GetUserId());
            var skipRecords = page * storyPerPage;

            List<IStory> listOfStories = db.Posts.ToList<IStory>().Where(x => user.Following.Contains(x.User)).ToList();
            listOfStories.AddRange(db.BigStories.ToList<BigStory>().Where(x => x.IsLocked == false));

            return listOfStories.
                OrderByDescending(x => x.Created).
                Skip(skipRecords).
                Take(storyPerPage).ToList();
        }


       
    }
}