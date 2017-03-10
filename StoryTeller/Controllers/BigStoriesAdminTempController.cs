using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoryTeller.Models;
using System.IO;

namespace StoryTeller.Controllers
{
    public class BigStoriesAdminTempController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BigStoriesAdminTemp
        public ActionResult Index()
        {
            return View(db.BigStories.ToList());
        }

        // GET: BigStoriesAdminTemp/Details/5
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
