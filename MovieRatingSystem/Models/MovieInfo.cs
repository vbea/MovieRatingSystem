using MovieModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vbes.WebControls.Mvc;

namespace MovieRatingSystem.Models
{
    public class MovieInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Movie Movie { get; set; }
        public string Categories { get; set; }
        public List<Comments> Comments { get; set; }
    }
}