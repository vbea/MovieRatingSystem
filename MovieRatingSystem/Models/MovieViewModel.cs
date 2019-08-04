using MovieModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRatingSystem.Models
{
    public class MovieViewModel
    {
        public List<Poster> Posters { get; set; }
        public List<Movie> Movies { get; set; }
    }
}