using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieModel;
using MovieBLL;

namespace MovieRatingSystem.Controllers
{
    public class UserInfoController : Controller
    {
        private MovieRatingModel db = new MovieRatingModel();

        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                Users users = db.Users.Find(id);
                if (users != null)
                {
                    Users user = UserBLL.getLoginUser(this);
                    users.Email = UserBLL.showKeys(user != null ? user.Roles == 1 || users.UserName.Equals(user.UserName) ? 1 : 0 : 0, users.Email);
                    return View(users);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details(string id)
        {
            if (id != null)
            {
                Users users = db.Users.Where(u => u.UserName == id).FirstOrDefault();
                if (users != null)
                {
                    Users user = UserBLL.getLoginUser(this);
                    users.Email = UserBLL.showKeys(user != null ? user.Roles == 1 || users.UserName.Equals(user.UserName) ? 1 : 0 : 0, users.Email);
                    return View("Index",users);
                }
            }
            return RedirectToAction("Index", "Home");
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
