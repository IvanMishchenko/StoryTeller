using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using StoryTeller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

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

        // GET: Story
        public ActionResult Index(string id)
        {
            PostsUser postsUser = new PostsUser()
            {
                Posts = db.Posts.Where(x => x.User.StoryTellerName == id).Include(x => x.User),
                userToSubsribe = db.Users.FirstOrDefault(x => x.StoryTellerName == id)
            };

            return View(postsUser);
        }

        // GET: Story/Details/5
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

        // GET: Story/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Story/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Text,Created,Subtitle")] Post post)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                post.User = currentUser;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = post.User.StoryTellerName });
            }

            return View(post);
        }

        // GET: Story/Edit/5
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

        // POST: Story/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Text,Created")] Post post)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
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

        // GET: Story/Delete/5
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

        // POST: Story/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);

            string StoryTellerName = post.User.StoryTellerName;

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
