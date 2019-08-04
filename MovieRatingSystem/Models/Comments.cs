using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRatingSystem.Models
{
    public class Comments
    {
        public int ID { get; set; }
        public string Movie { get; set; }
        public double Score { get; set; }
        public string Comment { get; set; }
        public int Praise { get; set; }
        public System.DateTime Dated { get; set; }
        public string UserName { get; set; }
        public string Nickname { get; set; }
        public string Header { get; set; }
    }
}