using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieModel;

namespace MovieDAL
{
    public class UserDAL
    {
        MovieRatingModel db;
        public UserDAL()
        {
            db = new MovieRatingModel();
        }

        public Users Login(string username, string password)
        {
            Users user = db.Users.Where(u => u.UserName == username).Where(u => u.Password==password).FirstOrDefault();
            return user;
        }
    }
}
