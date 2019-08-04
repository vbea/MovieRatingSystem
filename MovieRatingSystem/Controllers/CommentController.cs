using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieModel;
using MovieRatingSystem.Models;
using MovieBLL;
using Vbes.WebControls.Mvc;

namespace MovieRatingSystem.Controllers
{
    public class CommentController : Controller
    {
        private MovieRatingModel db = new MovieRatingModel();
        
        public ActionResult Index(int? page)
        {
            if (UserBLL.isAdmin(this))
            {
                var rating = (from r in db.Rating
                              join m in db.Movie on r.Movie equals m.ID
                              join u in db.Users on r.Users equals u.ID
                              orderby r.Dated descending
                              select new Comments
                              {
                                  ID = r.ID,
                                  Movie = m.Name,
                                  UserName = u.UserName,
                                  Nickname = u.Nickname,
                                  Comment = r.Comment,
                                  Score = r.Score,
                                  Praise = r.Praise,
                                  Dated = r.Dated
                              });
                return View(rating.ToPagedList(page??1, 10));
            }
            else
                return RedirectToAction("Login", "Home");
        }

        public ActionResult Subject(int? id, int? page)
        {
            if (!UserBLL.isLogin(this))
                return RedirectToAction("Login", "Home");
            if (id != null)
            {
                Movie movie = db.Movie.Find(id);
                if (movie != null)
                {
                    CommentModel model = new CommentModel();
                    model.ID = movie.ID;
                    model.Name = movie.Name;
                    model.Comments = getComments(movie.ID, page ?? 1, 15);
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Subject(int? id, FormCollection form)
        {
            if (id != null)
            {
                Users user = UserBLL.getLoginUser(this);
                if (user != null)
                {
                    string score = form["Score"];
                    Rating rat = new Rating();
                    rat.Movie = id ?? 0;
                    rat.Score = (int)(double.Parse(score) * 2);
                    rat.Comment = form["Comment"];
                    rat.Praise = 0;
                    rat.Users = user.ID;
                    rat.Dated = DateTime.Now;
                    db.Rating.Add(rat);
                    if (db.SaveChanges() > 0)
                    {
                        //计算平均分
                        List<Rating> list = db.Rating.Where(r => r.Movie == id).ToList();
                        double score1 = 0;
                        if (list.Count > 0)
                        {
                            foreach (Rating r in list)
                            {
                                score1 += r.Score;
                            }
                            score1 = score1 / list.Count;
                            setAverige(id, score1);
                        }
                    }
                    return RedirectToAction("Details", "Movie", new { id = id });
                }
            }
            return View();
        }

        public string Praise(int? id)
        {
            if (UserBLL.isLogin(this) && id != null)
            {
                Rating rat = db.Rating.Find(id);
                if (rat != null)
                {
                    rat.Praise += 1;
                    db.Entry(rat).State = EntityState.Modified;
                    db.SaveChanges();
                    return rat.Praise.ToString();
                }
            }
            return "-1";
        }


        public ActionResult Delete(int id)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            Rating rating = db.Rating.Find(id);
            db.Rating.Remove(rating);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void setAverige(int? id, double averige)
        {
            if (id != null)
            {
                Movie movie = db.Movie.Find(id);
                movie.Averige = Convert.ToDecimal(averige);
                db.Entry<Movie>(movie).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public PagedList<Comments> getComments(int movie, int page, int size)
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
                          });
            return rating.ToPagedList(page, size);
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
