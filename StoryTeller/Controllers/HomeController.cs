using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StoryTeller.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StoryTeller.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        const int postPerPage = 8;

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
            var page = id ?? 0;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Home/Partial/_Posts.cshtml", GetPaginatedPosts(page));
            }

            return View("Index", db.Posts.OrderByDescending(x=>x.Created).Take(postPerPage));
        }

        private List<Post> GetPaginatedPosts(int page = 1)
        {
            var skipRecords = page * postPerPage;

            var listOfPosts = db.Posts;

            return listOfPosts.
                OrderByDescending(x => x.Created).
                Skip(skipRecords).
                Take(postPerPage).ToList();
        }


       
    }
}