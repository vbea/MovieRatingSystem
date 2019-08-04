using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MovieModel;

namespace MovieBLL
{
    public class UserBLL
    {
        public const string Session_LoginUser = "LoginUser";
        public const int Admin = 1;
        public const int Normal = 0;
        public static string MD5Hash(string str)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        //public static void Login(HttpContextBase context, Users user)
        //{
        //    context.Session[Session_LoginUser] = user;
        //}

        //public static bool isLogin(HttpContextBase context)
        //{
        //    return context.Session[Session_LoginUser] != null;
        //}

        //public static bool isAdmin(HttpContextBase context)
        //{
        //    Users u = getLoginUser(context);
        //    if (u != null)
        //        return u.Roles == Admin;
        //    return false;
        //}

        //public static Users getLoginUser(HttpContextBase context)
        //{
        //    if (isLogin(context))
        //        return context.Session[Session_LoginUser] as Users;
        //    return null;
        //}

        public static void Login(Controller context, Users user)
        {
            context.Session[Session_LoginUser] = user;
        }

        public static bool isLogin(Controller context)
        {
            return context.Session[Session_LoginUser] != null;
        }

        public static bool isAdmin(Controller context)
        {
            Users u = getLoginUser(context);
            if (u != null)
                return u.Roles == Admin;
            return false;
        }

        public static Users getLoginUser(Controller context)
        {
            if (isLogin(context))
                return context.Session[Session_LoginUser] as Users;
            return null;
        }

        public static string showKeys(int role, string key)
        {
            if (role == Admin)
                return key;
            else
            {
                string show = "";
                if (key.Length >= 20)
                {
                    show += key.Substring(0, 5);
                    show += showStar(key.Length - 10);
                    show += key.Substring(key.Length - 5);
                }
                else if (key.Length <= 20 && key.Length > 7)
                {
                    show += key.Substring(0, 4);
                    show += showStar(key.Length - 8);
                    show += key.Substring(key.Length - 4);
                }
                else if (key.Length <= 7 && key.Length > 4)
                {
                    show += key.Substring(0, 2);
                    show += showStar(key.Length - 4);
                    show += key.Substring(key.Length - 2);
                }
                else// if (key.Length <= 4)
                {
                    show += key.Substring(0, 1);
                    show += showStar(key.Length - 1);
                }
                return show;
            }
        }

        private static string showStar(int i)
        {
            int j = 0;
            string s = "";
            while (j < i)
            {
                s += "*";
                j++;
            }
            return s;
        }
    }
}
