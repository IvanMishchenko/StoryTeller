using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using StoryTeller.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoryTeller.Controllers
{
    public class PhotoController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public PhotoController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public FileContentResult UserProfilePhotoByAuthorization()
        {
            if (User.Identity.IsAuthenticated)
            {
                var StoryTellerName = manager.FindById(User.Identity.GetUserId()).StoryTellerName;
                var user = db.Users.Where(x => x.StoryTellerName == StoryTellerName).FirstOrDefault();

                return new FileContentResult(user.UserPhoto, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Content/Images/question-mark.jpg");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/jpg");

            }
        }

        public FileContentResult UserProfilePhotoById(string id)
        {
                var StoryTellerName = manager.FindById(User.Identity.GetUserId()).StoryTellerName;
                var userImage = db.Users.Where(x => x.StoryTellerName == id).FirstOrDefault();

                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
        }

        public FileContentResult PostPhotById(string id)
        {
            var post = db.Posts.Where(x => x.Id.ToString() == id).FirstOrDefault();

            return new FileContentResult(post.StoryPhoto, "image/jpeg");
        }

        public FileContentResult BigStoryPhotById(string id)
        {
            var bigStory = db.Posts.Where(x => x.Id.ToString() == id).FirstOrDefault();
            return new FileContentResult(bigStory.StoryPhoto, "image/jpeg");
        }



    }
}