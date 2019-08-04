using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieModel;
using Vbes.WebControls.Mvc;
using MovieBLL;

namespace MovieRatingSystem.Controllers.admin
{
    public class CategoryController : Controller
    {
        private MovieRatingModel db = new MovieRatingModel();
        
        public ActionResult Index(int? page)
        {
            if (UserBLL.isAdmin(this))
                return View(db.Category.OrderBy(c=>c.ID).ToPagedList(page??1,10));
            else
                return RedirectToAction("Login", "Home");
        }
        
        public ActionResult Create()
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            return View();
        }
        
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,TypeName,Valid")] Category category)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        
        public ActionResult Edit(int? id)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            if (id != null)
            {
                Category category = db.Category.Find(id);
                if (category != null)
                {
                    return View(category);
                }
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,TypeName,Valid")] Category category)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
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
