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
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BigStory bigStory = db.BigStories.Find(id);
            if (bigStory == null)
            {
                return HttpNotFound();
            }
            return View(bigStory);
        }

        // GET: BigStoriesAdminTemp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BigStoriesAdminTemp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Text,MaxNumberOfPosts,IsLocked,Created,Deadline", Exclude = "StoryPhoto")] BigStory bigStory)
        {
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["StoryPhoto"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            if (ModelState.IsValid)
            {
                bigStory.Created = DateTime.Now;
                bigStory.StoryPhoto = imageData;
                db.BigStories.Add(bigStory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bigStory);
        }

        // GET: BigStoriesAdminTemp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BigStory bigStory = db.BigStories.Find(id);
            if (bigStory == null)
            {
                return HttpNotFound();
            }
            return View(bigStory);
        }

        // POST: BigStoriesAdminTemp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Subtitle,Text,PostPhoto,MaxAmountOffUsers,IsLocked,Created,Deadline,MaxNumberOfPosts,HoursToWrite,HoursToDiscuss,MaxAllowedNumberOfDislikes")] BigStory bigStory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bigStory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bigStory);
        }

        // GET: BigStoriesAdminTemp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BigStory bigStory = db.BigStories.Find(id);
            if (bigStory == null)
            {
                return HttpNotFound();
            }
            return View(bigStory);
        }

        // POST: BigStoriesAdminTemp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BigStory bigStory = db.BigStories.Find(id);
            db.BigStories.Remove(bigStory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
