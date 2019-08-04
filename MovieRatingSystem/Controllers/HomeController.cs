using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieModel;
using MovieBLL;
using MovieRatingSystem.Models;

namespace MovieRatingSystem.Controllers
{
    public class HomeController : Controller
    {
        MovieRatingModel db = new MovieRatingModel();
        
        public ActionResult Index()
        {
            MovieViewModel model = new MovieViewModel();
            model.Posters = db.Poster.Where(p => p.Valid == true).ToList();
            model.Movies = db.Movie.Where(m => m.Valid == true).OrderByDescending(m => m.Averige).ToList();
            return View(model);
        }

        public ActionResult Login()
        {
            if (UserBLL.isLogin(this))
                return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string username = form["username"];
            string password = form["password"];
            Users user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
            if (user != null)
            {
                ViewBag.Username = username;
                if (user.Password.Equals(UserBLL.MD5Hash(password)))
                {
                    UserBLL.Login(this, user);
                    return RedirectToAction("Index");
                }
                else
                    ViewBag.Message = "密码输入错误";
            }
            else
                ViewBag.Message = "该用户不存在";
            return View();
        }

        public ActionResult Sign()
        {
            return View(new Users());
        }
        [HttpPost]
        public ActionResult Sign(Users user)
        {
            if (user != null)
            {
                int count = (from u in db.Users where u.UserName == user.UserName || u.Email == user.Email select u).Count();
                if (count == 0)
                {
                    if (user.UserName.ToLower().Equals("admin"))
                        user.Roles = UserBLL.Admin;
                    else
                        user.Roles = UserBLL.Normal;
                    user.Password = UserBLL.MD5Hash(user.Password);
                    user.Birthday = "1990-01-01";
                    db.Users.Add(user);
                    if (db.SaveChanges() > 0)
                        return Content(MoviesBLL.AlertAndRedirect("/Login","注册成功！"));
                    else
                        ViewBag.Message = "注册失败";
                }
                else
                    ViewBag.Message = "用户名或邮箱账号已存在";
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Remove(UserBLL.Session_LoginUser);
            return RedirectToAction("Index");
        }

        public string ThemeAll()
        {
            Session.Remove("Theme_Primary");
            Session.Remove("Theme_Accent");
            Session.Remove("Theme_Layout");
            return "设置成功";
        }

        public string ThemePrimary(string primay)
        {
            Session["Theme_Primary"] = primay;
            return "设置成功";
        }
        public string ThemeAccent(string accent)
        {
            Session["Theme_Accent"] = accent;
            return "设置成功";
        }

        public string ThemeLayout(string layout)
        {
            Session["Theme_Layout"] = layout;
            return "设置成功";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void ExecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}