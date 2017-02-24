using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using StoryTeller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System;
using System.Web;
using System.IO;

namespace StoryTeller.Controllers
{
    [Authorize]
    public class StoryController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public StoryController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult Index(string id)
        {
            PostsUser postsUser = new PostsUser()
            {
                Posts = db.Posts.Where(x => x.User.StoryTellerName == id).Include(x => x.User),
                userToSubsribe = db.Users.FirstOrDefault(x => x.StoryTellerName == id)
            };

            return View(postsUser);
        }

        public ActionResult Details(int? id)
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
            return View(post);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Text,Subtitle", Exclude = "PostPhoto")] Post post)
        {
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["PostPhoto"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                post.User = currentUser;
                post.Created = DateTime.Now;
                post.PostPhoto = imageData;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = post.User.StoryTellerName });
            }

            return View(post);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (post.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Text", Exclude = "PostPhoto")] Post post)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());

            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["PostPhoto"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            if (ModelState.IsValid)
            {
                post.Created = DateTime.Now;
                post.PostPhoto = imageData;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = currentUser.StoryTellerName });
            }

            if (post.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return View(post);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (post.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);

            string StoryTellerName = post.User.StoryTellerName;

            db.Comments.RemoveRange(db.Comments.Where(x => x.Post.Id == id));
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = StoryTellerName });
        }

        public ActionResult Unsubscribe(string userToSubsribeId)
        {

            if (userToSubsribeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = manager.FindById(User.Identity.GetUserId());
            if(user != null)
            {
                db.Users.FirstOrDefault(x => x.StoryTellerName == userToSubsribeId).Followers.Remove(user);
                db.Users.FirstOrDefault(x => x.StoryTellerName == user.StoryTellerName).Following.Remove(db.Users.FirstOrDefault(x => x.StoryTellerName == userToSubsribeId));
                db.SaveChanges();
                return PartialView("~/Views/Story/Partial/_SubscribeButton.cshtml");
            }
            return null;

        }

        public ActionResult Subscribe(string userToSubsribeId)
        {
            if (userToSubsribeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = manager.FindById(User.Identity.GetUserId());

            if(user != null)
            {
                db.Users.FirstOrDefault(x => x.StoryTellerName == userToSubsribeId).Followers.Add(user);
                db.Users.FirstOrDefault(x => x.StoryTellerName == user.StoryTellerName).Following.Add(db.Users.FirstOrDefault(x => x.StoryTellerName == userToSubsribeId));
                db.SaveChanges();
                return PartialView("~/Views/Story/Partial/_UnsubscribeButton.cshtml");
            }

            return null;
        }

        public ActionResult LeaveComment(string commentText, string postId)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (commentText == string.Empty || postId == null || postId == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = new Comment();
            comment.Created = DateTime.Now;
            comment.User = currentUser;
            comment.Text = commentText;
            comment.Post = db.Posts.FirstOrDefault(x => x.Id.ToString() == postId);

            db.Comments.Add(comment);
            db.SaveChanges();

            return PartialView("~/Views/Story/Partial/_CommentsSection.cshtml", db.Comments.Where(x => x.Post.Id.ToString() == postId).OrderByDescending(x => x.Created));
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
