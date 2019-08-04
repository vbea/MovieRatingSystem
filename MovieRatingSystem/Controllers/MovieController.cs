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
using System.IO;
using System.Data.Entity.Validation;
using MovieRatingSystem.Models;

namespace MovieRatingSystem.Controllers
{
    public class MovieController : Controller
    {
        private MovieRatingModel db = new MovieRatingModel();
        public ActionResult Index(int? page)
        {
            if (UserBLL.isAdmin(this))
                return View(db.Movie.OrderBy(m => m.ID).ToPagedList(page ?? 1, 10));
            else
                return RedirectToAction("Login", "Home");
        }

        public ActionResult Category()
        {
            ViewBag.Shows = (from d in db.Movie where d.Valid select d.Show.Substring(0, 4)).Distinct().ToList();
            ViewBag.Areas = (from d in db.Movie where d.Valid select d.Area).Distinct().ToList();
            ViewBag.Lang = (from d in db.Movie where d.Valid select d.Languages).Distinct().ToList();
            return View(db.Category.Where(c => c.Valid == true));
        }

        public ActionResult Ranking()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                Movie movie = db.Movie.Find(id);
                if (movie != null)
                {
                    MovieInfo model = new MovieInfo();
                    model.Movie = movie;
                    model.Name = movie.Name;
                    model.ID = movie.ID;
                    string[] cs = movie.Types.Split(',');
                    List<String> cate = (from c in db.Category where cs.Contains(c.ID.ToString()) select c.TypeName).ToList();
                    model.Categories = string.Join("/", cate);
                    model.Comments = getComments(movie.ID); 
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            ViewBag.Categorys = db.Category.Where(u => u.Valid == true);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            bool status = true;
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            string directory = DateTime.Now.ToString("yyyy-MM-dd");
            string dated = DateTime.Now.ToString("HHmmss");
            string path = Request.PhysicalApplicationPath + "upload\\" + directory;
            //读取封面文件
            HttpPostedFileBase cover = Request.Files["covers"];
            if (cover != null && cover.ContentLength > 0)
            {
                string type = Path.GetExtension(cover.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && cover.ContentLength <= 3145728)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    cover.SaveAs(path + "\\cover_" + dated + type);
                    movie.Cover = "/upload/" + directory + "/cover_" + dated + type;
                    //读取剧照1文件
                    HttpPostedFileBase stills0 = Request.Files["still0"];
                    if (stills0 != null && stills0.ContentLength > 0)
                    {
                        type = Path.GetExtension(stills0.FileName).ToLower();
                        if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && cover.ContentLength <= 3145728)
                        {
                            stills0.SaveAs(path + "\\stills_0_" + dated + type);
                            movie.Stills0 = "/upload/" + directory + "/stills_0_" + dated + type;
                        }
                        else
                        {
                            ViewBag.Message = "剧照1文件错误：请上传小于3MB的图片文件";
                            status = false;
                        }
                    }
                    //读取剧照2文件
                    HttpPostedFileBase stills1 = Request.Files["still1"];
                    if (stills1 != null && stills1.ContentLength > 0)
                    {
                        type = Path.GetExtension(stills1.FileName).ToLower();
                        if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && stills1.ContentLength <= 3145728)
                        {
                            stills1.SaveAs(path + "\\stills_1_" + dated + type);
                            movie.Stills1 = "/upload/" + directory + "/stills_1_" + dated + type;
                        }
                        else
                        {
                            ViewBag.Message = "剧照2文件错误：请上传小于3MB的图片文件";
                            status = false;
                        }
                    }
                    //读取剧照3文件
                    HttpPostedFileBase stills2 = Request.Files["still2"];
                    if (stills2 != null && stills2.ContentLength > 0)
                    {
                        type = Path.GetExtension(stills2.FileName).ToLower();
                        if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && stills2.ContentLength <= 3145728)
                        {
                            stills2.SaveAs(path + "\\stills_2_" + dated + type);
                            movie.Stills2 = "/upload/" + directory + "/stills_2_" + dated + type;
                        }
                        else
                        {
                            ViewBag.Message = "剧照3文件错误：请上传小于3MB的图片文件";
                            status = false;
                        }
                    }
                    //读取剧照4文件
                    HttpPostedFileBase stills3 = Request.Files["still3"];
                    if (stills3 != null && stills3.ContentLength > 0)
                    {
                        type = Path.GetExtension(stills3.FileName).ToLower();
                        if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && stills3.ContentLength <= 3145728)
                        {
                            stills3.SaveAs(path + "\\stills_3_" + dated + type);
                            movie.Stills3 = "/upload/" + directory + "/stills_3_" + dated + type;
                        }
                        else
                        {
                            ViewBag.Message = "剧照4文件错误：请上传小于3MB的图片文件";
                            status = false;
                        }
                    }
                    //读取剧照5文件
                    HttpPostedFileBase stills4 = Request.Files["still4"];
                    if (stills4 != null && stills4.ContentLength > 0)
                    {
                        type = Path.GetExtension(stills4.FileName).ToLower();
                        if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && stills4.ContentLength <= 3145728)
                        {
                            stills4.SaveAs(path + "\\stills_4_" + dated + type);
                            movie.Stills4 = "/upload/" + directory + "/stills_4_" + dated + type;
                        }
                        else
                        {
                            ViewBag.Message = "剧照5文件错误：请上传小于3MB的图片文件";
                            status = false;
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "封面文件错误：请上传小于3MB的图片文件";
                    status = false;
                }
            }
            if (status)
            {
                if (ModelState.IsValid)
                {
                    movie.CreateBy = UserBLL.getLoginUser(this).UserName;
                    movie.CreateOn = DateTime.Now;
                    movie.Averige = 0;
                    db.Movie.Add(movie);
                    if (db.SaveChanges() > 0)
                        return RedirectToAction("Index");
                    else
                        ViewBag.Message = "保存失败";
                }
            }
            ViewBag.Categorys = db.Category.Where(u => u.Valid == true);
            return View(movie);
        }

        public ActionResult Edit(int? id)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            if (id != null)
            {
                Movie movie = db.Movie.Find(id);
                if (movie != null)
                {
                    ViewBag.Categorys = db.Category.Where(u => u.Valid == true);
                    return View(movie);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            bool status = true;
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            string directory = DateTime.Now.ToString("yyyy-MM-dd");
            string dated = DateTime.Now.ToString("HHmmss");
            string path = Request.PhysicalApplicationPath + "upload\\" + directory;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //读取封面文件
            HttpPostedFileBase cover = Request.Files["covers"];
            if (cover != null && cover.ContentLength > 0)
            {
                string type = Path.GetExtension(cover.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && cover.ContentLength <= 3145728)
                {
                    cover.SaveAs(path + "\\cover_" + dated + type);
                    movie.Cover = "/upload/" + directory + "/cover_" + dated + type;
                }
                else
                {
                    ViewBag.Message = "封面文件错误：请上传小于3MB的图片文件";
                    status = false;
                }
            }
            //读取剧照1文件
            HttpPostedFileBase stills0 = Request.Files["still0"];
            if (stills0 != null && stills0.ContentLength > 0)
            {
                string type = Path.GetExtension(stills0.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && cover.ContentLength <= 3145728)
                {
                    stills0.SaveAs(path + "\\stills_0_" + dated + type);
                    movie.Stills0 = "/upload/" + directory + "/stills_0_" + dated + type;
                }
                else
                {
                    ViewBag.Message = "剧照1文件错误：请上传小于3MB的图片文件";
                    status = false;
                }
            }
            //读取剧照2文件
            HttpPostedFileBase stills1 = Request.Files["still1"];
            if (stills1 != null && stills1.ContentLength > 0)
            {
                string type = Path.GetExtension(stills1.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && stills1.ContentLength <= 3145728)
                {
                    stills1.SaveAs(path + "\\stills_1_" + dated + type);
                    movie.Stills1 = "/upload/" + directory + "/stills_1_" + dated + type;
                }
                else
                {
                    ViewBag.Message = "剧照2文件错误：请上传小于3MB的图片文件";
                    status = false;
                }
            }
            //读取剧照3文件
            HttpPostedFileBase stills2 = Request.Files["still2"];
            if (stills2 != null && stills2.ContentLength > 0)
            {
                string type = Path.GetExtension(stills2.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && stills2.ContentLength <= 3145728)
                {
                    stills2.SaveAs(path + "\\stills_2_" + dated + type);
                    movie.Stills2 = "/upload/" + directory + "/stills_2_" + dated + type;
                }
                else
                {
                    ViewBag.Message = "剧照3文件错误：请上传小于3MB的图片文件";
                    status = false;
                }
            }
            //读取剧照4文件
            HttpPostedFileBase stills3 = Request.Files["still3"];
            if (stills3 != null && stills3.ContentLength > 0)
            {
                string type = Path.GetExtension(stills3.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && stills3.ContentLength <= 3145728)
                {
                    stills3.SaveAs(path + "\\stills_3_" + dated + type);
                    movie.Stills3 = "/upload/" + directory + "/stills_3_" + dated + type;
                }
                else
                {
                    ViewBag.Message = "剧照4文件错误：请上传小于3MB的图片文件";
                    status = false;
                }
            }
            //读取剧照5文件
            HttpPostedFileBase stills4 = Request.Files["still4"];
            if (stills4 != null && stills4.ContentLength > 0)
            {
                string type = Path.GetExtension(stills4.FileName).ToLower();
                if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && stills4.ContentLength <= 3145728)
                {
                    stills4.SaveAs(path + "\\stills_4_" + dated + type);
                    movie.Stills4 = "/upload/" + directory + "/stills_4_" + dated + type;
                }
                else
                {
                    ViewBag.Message = "剧照5文件错误：请上传小于3MB的图片文件";
                    status = false;
                }
            }
            if (status)
            {
                if (ModelState.IsValid)
                {
                    movie.CreateBy = UserBLL.getLoginUser(this).UserName;
                    movie.CreateOn = DateTime.Now;
                    db.Entry(movie).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                        return RedirectToAction("Index");
                    else
                        ViewBag.Message = "修改失败";
                }
            }
            ViewBag.Categorys = db.Category.Where(u => u.Valid == true);
            return View(movie);
        }

        public ActionResult Delete(int id)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            Movie movie = db.Movie.Find(id);
            db.Movie.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Comments> getComments(int movie)
        {
            var rating = (from r in db.Rating
                          join m in db.Movie on r.Movie equals m.ID
                          join u in db.Users on r.Users equals u.ID
                          where r.Movie == movie
                          orderby r.Dated descending
                          select new Comments
                          {
                              ID = r.ID,
                              Movie = m.Name,
                              UserName = u.UserName,
                              Nickname = u.Nickname,
                              Comment = r.Comment,
                              Score = (double)r.Score / 2,
                              Praise = r.Praise,
                              Dated = r.Dated,
                              Header = u.Header
                          }).Take(10);
            return rating.ToList();
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
