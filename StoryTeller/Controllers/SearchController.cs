using StoryTeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoryTeller.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private ApplicationDbContext db;

        public SearchController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View(db.Users);
        }

        public ActionResult SearchPeople(string searchText)
        {
            var term = searchText.ToLower();
            var result = db.Users
                .Where(p =>
                    p.StoryTellerName.ToLower().Contains(term)
                );
            return PartialView("~/Views/Search/Partial/_SearchStoryTellerUsers.cshtml", result);
        }
    }
}