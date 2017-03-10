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
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostIndex()
        {
            return View("~/Views/Admin/Post/Index.cshtml",db.Posts.ToList());
        }

        public ActionResult PostDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Post/Details.cshtml",post);
        }

        public ActionResult PostDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Post/Delete.cshtml",post);
        }

        [HttpPost, ActionName("PostDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult PostDeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Comments.RemoveRange(post.Comments);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("PostIndex");
        }

        public ActionResult BigStoryIndex()
        {
            return View("~/Views/Admin/BigStory/Index.cshtml",db.BigStories.ToList());
        }

        public ActionResult BigStoryDetails(int? id)
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
            return View("~/Views/Admin/BigStory/Details.cshtml",bigStory);
        }

        // GET: BigStoriesAdminTemp/Create
        public ActionResult BigStoryCreate()
        {
            return View("~/Views/Admin/BigStory/Create.cshtml");
        }

        // POST: BigStoriesAdminTemp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BigStoryCreate([Bind(Include = "Id,Title,Text,MaxNumberOfPosts,IsLocked,Created,Deadline", Exclude = "StoryPhoto")] BigStory bigStory)
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
                return RedirectToAction("BigStoryIndex");
            }

            return View(bigStory);
        }

        // GET: BigStoriesAdminTemp/Edit/5
        public ActionResult BigStoryEdit(int? id)
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
            return View("~/Views/Admin/BigStory/Edit.cshtml",bigStory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BigStoryEdit([Bind(Include = "Id,Title,Subtitle,Text,PostPhoto,MaxAmountOffUsers,IsLocked,Created,Deadline,MaxNumberOfPosts,HoursToWrite,HoursToDiscuss,MaxAllowedNumberOfDislikes")] BigStory bigStory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bigStory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BigStoryIndex");
            }
            return View(bigStory);
        }

        public ActionResult BigStoryDelete(int? id)
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
            return View("~/Views/Admin/BigStory/Delete.cshtml",bigStory);
        }

        [HttpPost, ActionName("BigStoryDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult BigStoryDeleteConfirmed(int id)
        {
            BigStory bigStory = db.BigStories.Find(id);
            db.Comments.RemoveRange(bigStory.Comments);
            db.BigStories.Remove(bigStory);
            db.SaveChanges();
            return RedirectToAction("BigStoryIndex");
        }

        public ActionResult CommentIndex()
        {
            return View("~/Views/Admin/Comment/Index.cshtml",db.Comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult CommentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Comment/Details.cshtml", comment);
        }

        public ActionResult CommentDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Comment/Delete.cshtml", comment);
        }

        [HttpPost, ActionName("CommentDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult CommentDeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("CommentIndex");
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
