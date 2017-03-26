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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace StoryTeller.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public AdminController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            new HomeController().UpdateExpiredStories();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostIndex()
        {
            return View("~/Views/Admin/Post/Index.cshtml", db.Posts.ToList());
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
            return View("~/Views/Admin/Post/Details.cshtml", post);
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
            return View("~/Views/Admin/Post/Delete.cshtml", post);
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
            return View("~/Views/Admin/BigStory/Index.cshtml", db.BigStories.ToList());
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
            return View("~/Views/Admin/BigStory/Details.cshtml", bigStory);
        }

        public ActionResult BigStoryCreate()
        {
            return View("~/Views/Admin/BigStory/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BigStoryCreate([Bind(Include = "Id,Title,Text,MaxNumberOfPosts,IsLocked,Deadline,HoursToWrite", Exclude = "StoryPhoto,PostText")] BigStory bigStory)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["StoryPhoto"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            var text = Request["PostText"];

            if (ModelState.IsValid)
            {
                bigStory.Posts = new List<PartBigStory>();
                bigStory.Posts.Add(new PartBigStory()
                {
                    User = currentUser,
                    Created = DateTime.Now,
                    Text = text
                });
                bigStory.Administrator = currentUser;
                bigStory.UnModeratedPost = null;
                bigStory.Created = DateTime.Now;
                bigStory.StoryPhoto = imageData;
                db.BigStories.Add(bigStory);
                db.SaveChanges();
                return RedirectToAction("BigStoryIndex");
            }

            return View(bigStory);
        }

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
            return View("~/Views/Admin/BigStory/Edit.cshtml", bigStory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BigStoryEdit([Bind(Include = "Id,Title,Created,IsLocked,Deadline,MaxNumberOfPosts,HoursToWrite", Exclude = "StoryPhoto")] BigStory bigStory)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());

            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["StoryPhoto"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            var oldPhoto = db.BigStories.Find(bigStory.Id).StoryPhoto;
            if (ModelState.IsValid)
            {
                if (imageData.Count() > 0)
                {
                    bigStory.StoryPhoto = imageData;
                }
                else
                {
                    bigStory.StoryPhoto = oldPhoto;
                }
                db.Set<BigStory>().AddOrUpdate(bigStory);
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
            return View("~/Views/Admin/BigStory/Delete.cshtml", bigStory);
        }

        [HttpPost, ActionName("BigStoryDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult BigStoryDeleteConfirmed(int id)
        {
            BigStory bigStory = db.BigStories.Find(id);
            db.Comments.RemoveRange(bigStory.Comments);
            db.PartsBigStory.RemoveRange(bigStory.Posts);
            db.BigStories.Remove(bigStory);
            db.SaveChanges();
            return RedirectToAction("BigStoryIndex");
        }

        public ActionResult BigStoryModerate()
        {
            var bigStories = db.BigStories.Where(x => x.UnModeratedPost != null).ToList();
            return View("~/Views/Admin/BigStory/Moderate.cshtml", bigStories);
        }

        public ActionResult BigStoryModerateDetails(int? id)
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
            return View("~/Views/Admin/BigStory/ModerateDetails.cshtml", bigStory);
        }

        public ActionResult ApproveLastPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BigStory bigStory = db.BigStories.Include("UnModeratedPost").Where(x => x.Id == id).FirstOrDefault();
            if (bigStory == null)
            {
                return HttpNotFound();
            }
            if (!bigStory.AllUsers.Any(x => x.StoryTellerName == bigStory.UnModeratedPost.User.StoryTellerName))
            {
                bigStory.AllUsers.Add(bigStory.UnModeratedPost.User);
            }
            bigStory.Posts.Add(bigStory.UnModeratedPost);
            bigStory.IsLocked = false;
            bigStory.WhenLocked = null;
            bigStory.CurrentUser = null;
            bigStory.UnModeratedPost = null;

            db.SaveChanges();

            return RedirectToAction("BigStoryModerate");
        }
        public ActionResult DenyLastPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BigStory bigStory = db.BigStories.Include("UnModeratedPost").Where(x => x.Id == id).FirstOrDefault();
            if (bigStory == null)
            {
                return HttpNotFound();
            }
            bigStory.IsLocked = false;
            bigStory.WhenLocked = null;
            bigStory.CurrentUser = null;
            bigStory.UnModeratedPost = null;

            db.SaveChanges();

            return RedirectToAction("BigStoryModerate");
        }


        public ActionResult CommentIndex()
        {
            return View("~/Views/Admin/Comment/Index.cshtml", db.Comments.ToList());
        }

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
