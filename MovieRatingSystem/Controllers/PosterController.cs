using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieModel;
using System.IO;
using MovieBLL;
using Vbes.WebControls.Mvc;

namespace MovieRatingSystem.Controllers
{
    public class PosterController : Controller
    {
        private MovieRatingModel db = new MovieRatingModel();

        public ActionResult Index(int? page)
        {
            if (UserBLL.isAdmin(this))
                return View(db.Poster.OrderBy(p => p.ID).ToPagedList(page ?? 1, 10));
            else
                return RedirectToAction("Login", "Home");
        }

        public ActionResult Details(int? id)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            if (id != null)
            {
                Poster poster = db.Poster.Find(id);
                if (poster != null)
                    return View(poster);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            if (UserBLL.isAdmin(this))
                return View();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(Poster poster)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            string path = Request.PhysicalApplicationPath + "posters";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            HttpPostedFileBase file = Request.Files["picture"];
            if (file != null)
            {
                string type = Path.GetExtension(file.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && file.ContentLength <= 3145728)
                {
                    string name = DateTime.Now.ToString("yyyyMMddHHmmss") + type;
                    file.SaveAs(path + "\\" + name);
                    poster.Picture = "/posters/" + name;
                    poster.CreateBy = UserBLL.getLoginUser(this).UserName;
                    poster.CreateOn = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Poster.Add(poster);
                        if (db.SaveChanges() > 0)
                            return RedirectToAction("Index");
                        else
                            ViewBag.Message = "创建失败";
                    }
                }
                else
                    ViewBag.Message = "请上传小于3MB的图片文件";
            }
            return View(poster);
        }

        public ActionResult Edit(int? id)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            if (id != null)
            {
                Poster poster = db.Poster.Find(id);
                if (poster != null)
                    return View(poster);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Poster poster)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            string path = Request.PhysicalApplicationPath + "posters";
            HttpPostedFileBase file = Request.Files["pictures"];
            if (file != null && file.ContentLength != 0)
            {
                string type = Path.GetExtension(file.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && file.ContentLength <= 3145728)
                {
                    string name = DateTime.Now.ToString("yyyyMMddHHmmss") + type;
                    file.SaveAs(path + "\\" + name);
                    poster.Picture = "/posters/" + name;
                }
                else
                {
                    ViewBag.Message = "请上传小于3MB的图片文件";
                    return View(poster);
                }
            }
            if (ModelState.IsValid)
            {
                poster.CreateBy = UserBLL.getLoginUser(this).UserName;
                poster.CreateOn = DateTime.Now;
                db.Entry(poster).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                    return RedirectToAction("Index");
                else
                    ViewBag.Message = "更新失败";
            }
            return View(poster);
        }
        
        public ActionResult Delete(int id)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            Poster poster = db.Poster.Find(id);
            db.Poster.Remove(poster);
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
