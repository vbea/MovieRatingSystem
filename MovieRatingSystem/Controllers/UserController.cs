using MovieModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieBLL;
using System.Drawing.Drawing2D;
using Vbes.WebControls.Mvc;

namespace MovieRatingSystem.Controllers
{
    public class UserController : Controller
    {
        MovieRatingModel db = new MovieRatingModel();

        public ActionResult Index(int? page)
        {
            if (UserBLL.isAdmin(this))
                return View(db.Users.OrderBy(u => u.ID).ToPagedList(page ?? 1, 10));
            else
                return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult Portrait(string backUrl)
        {
            string path = Request.PhysicalApplicationPath + "portrait";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            HttpPostedFileBase file = Request.Files["myfile"];
            string type = Path.GetExtension(file.FileName).ToLower();
            if ((type.Equals(".jpg") || type.Equals(".png") || type.Equals(".jpeg") || type.Equals(".gif")) && file.ContentLength <= 3145728)
            {
                Image img = Image.FromStream(file.InputStream);
                int max = img.Width < img.Height ? img.Width : img.Height;
                int maxPix = 200;//设定最大像素
                string src = "temp" + DateTime.Now.ToString("yyyyMMddHHmmss") + type;
                /*if (max > maxPix)注释此代码，可将大图片变小，小图片变大
                {*/
                    int width, height;
                    width = img.Width;
                    height = img.Height;
                    float scale = ((float)max / (float)maxPix);
                    if (width < height)
                    {
                        width = maxPix;
                        height = (int)(height / scale);
                    }
                    else
                    {
                        height = maxPix;
                        width = (int)(width / scale);
                    }
                    Image newImg = new Bitmap(img, width, height);
                    newImg.Save(path + "\\" + src, img.RawFormat);
                /*}
                else
                    file.SaveAs(path + "\\" + src);*/
                ViewData.Add("imgresouce", "/portrait/" + src);
                Session["tempPath"] = path;
                Session["tempSrc"] = src;
                Session["backUrl"] = backUrl;
                return View();
            }
            else
                return Content(MoviesBLL.AlertAndRedirect(backUrl, "请上传小于3MB的图片文件!"));
        }

        [HttpPost]
        public ActionResult Header()
        {
            Users user = UserBLL.getLoginUser(this);
            if (user == null)
                return RedirectToAction("Login", "Users");
            user = db.Users.Find(user.ID);
            string path = Session["tempPath"].ToString();
            string src = Session["tempSrc"].ToString();
            int x = Convert.ToInt32(Request["x"]);
            int y = Convert.ToInt32(Request["y"]);
            int w = Convert.ToInt32(Request["w"]);
            int h = Convert.ToInt32(Request["h"]);
            string file = user.UserName.ToLower() + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(src);
            if (SaveImage(path + "\\" + src, path + "\\" + file, w, h, x, y))
            {
                string prefix = "/portrait/";
                string old = user.Header;
                user.Header = prefix + file;
                db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0)
                    UserBLL.Login(this, user);
                //删除临时图片
                FileInfo s = new FileInfo(path + "\\" + src);
                if (s.Exists)
                    s.Delete();
                //删除旧的头像
                if (!string.IsNullOrEmpty(old))
                {
                    FileInfo s1 = new FileInfo(path + "\\" + old.Replace(prefix, ""));
                    if (s1.Exists)
                        s1.Delete();
                }
                ViewData.Add("headimage", user.Header);
            }
            Session.Remove("tempPath");
            Session.Remove("tempSrc");
            string backu = Session["backUrl"].ToString();
            Session.Remove("backUrl");
            if (string.IsNullOrEmpty(backu))
                backu = "/";
            return Redirect(backu);
        }

        public ActionResult Setting()
        {
            Users user = UserBLL.getLoginUser(this);
            if (user == null)
                return RedirectToAction("Login", "Home");
            Address add = new Address(user.Address);
            ViewBag.Prov = add.Prov;
            ViewBag.City = add.City;
            ViewBag.Dist = add.Dist;
            return View(user);
        }

        public ActionResult Delete(int id)
        {
            if (!UserBLL.isAdmin(this))
                return RedirectToAction("Login", "Home");
            Users user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Setting(Users user)
        {
            if (user == null)
                return RedirectToAction("Login", "Home");
            Address add = new Address(Request["prov"], Request["city"], Request["dist"]);
            user.Address = add.GetAddress();
            ViewBag.Prov = add.Prov;
            ViewBag.City = add.City;
            ViewBag.Dist = add.Dist;
            db.Entry<Users>(user).State = System.Data.Entity.EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                UserBLL.Login(this, user);
                ViewBag.Status = "OK";
            }
            else
                ViewBag.Status = "NO";
            return View(user);
        }

        public ActionResult ChangePassword()
        {
            if (!UserBLL.isLogin(this))
                return RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            Users user = UserBLL.getLoginUser(this);
            if (user != null)
            {
                string old = form["oldpass"];
                string pass1 = form["password"];
                string pass2 = form["password2"];
                if (pass1.Equals(pass2))
                {
                    if (UserBLL.MD5Hash(old).Equals(user.Password))
                    {
                        Users newUser = db.Users.Find(user.ID);
                        newUser.Password = UserBLL.MD5Hash(pass2);
                        db.Entry<Users>(newUser).State = System.Data.Entity.EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            UserBLL.Login(this, newUser);
                            return Content(MoviesBLL.AlertAndRedirect("/Index", "修改成功"));
                        }
                        else
                            ViewBag.Message = "修改失败";
                    }
                    else
                        ViewBag.Message = "原密码输入不正确";
                }
                else
                    ViewBag.Message = "两次输入的密码不一致";
            }
            return View();
        }

        /// <summary>
        /// 剪裁图像
        /// </summary>
        /// <param name="Img"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        private bool SaveImage(string img, string path, int width, int height, int x, int y)
        {
            try
            {
                using (var OriginalImage = new Bitmap(img))
                {
                    using (var bmp = new Bitmap(width, height, OriginalImage.PixelFormat))
                    {
                        bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                        using (Graphics Graphic = Graphics.FromImage(bmp))
                        {
                            Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);//裁剪图片
                            bmp.Save(path, OriginalImage.RawFormat);
                            return true;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                return false;
            }
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