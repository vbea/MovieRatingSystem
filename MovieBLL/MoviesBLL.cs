using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBLL
{
    public class MoviesBLL
    {
        private const string Alert_Redirect = "<script>alert('{0}');location.href='{1}'</script>";
        private const string Alert_Snackbar = "<script>mdui.snackbar({0});</script>";
        private const string Re_Snackbar = "<script>mdui.snackbar({1});location.href='{0}';</script>";
        public static string AlertAndRedirect(string url,string msg)
        {
            return string.Format(Alert_Redirect, msg, url);
        }

        public static string Snackbar(string message)
        {
            return Snackbar(message, null);
        }

        public static string Snackbar(string message, int? timeout)
        {

            string s = "message:'" + message + "'";
            if (timeout != null)
                s += ",timeout:" + timeout;
            return string.Format(Alert_Snackbar, s);
        }
        public static string SnackbarUrl(string url, string message)
        {
            return SnackbarUrl(url, message, null);
        }

        public static string SnackbarUrl(string url, string message, int? timeout)
        {

            string s = "message:'" + message + "'";
            if (timeout!=null)
                s += ",timeout:" + timeout;
            return string.Format(Re_Snackbar, url, s);
        }
    }
}
