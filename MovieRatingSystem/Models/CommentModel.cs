using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vbes.WebControls.Mvc;

namespace MovieRatingSystem.Models
{
    public class CommentModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public PagedList<Comments> Comments { get; set; }
    }
}