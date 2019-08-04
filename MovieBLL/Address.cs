using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieBLL
{
    public class Address
    {
        public Address()
        {
            clear();
        }
        public Address(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                string[] addr = s.Split('-');
                switch (addr.Length)
                {
                    case 1:
                        Prov = addr[0];
                        break;
                    case 2:
                        Prov = addr[0];
                        City = addr[1];
                        break;
                    case 3:
                        Prov = addr[0];
                        City = addr[1];
                        Dist = addr[2];
                        break;
                }
            }
        }

        public Address(string p, string c, string d)
        {
            if (!string.IsNullOrEmpty(p))
            {
                Prov = p;
                if (!string.IsNullOrEmpty(c))
                {
                    City = c;
                    if (!string.IsNullOrEmpty(d))
                        Dist = d;
                }
            }
        }
        /// <summary>
        /// 省
        /// </summary>
        public string Prov { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 县区
        /// </summary>
        public string Dist { get; set; }

        public void clear()
        {
            Prov = City = Dist = "";
        }

        public string GetAddress()
        {
            if (string.IsNullOrEmpty(Prov))
                return "";
            else
            {
                string result = Prov;
                if (!string.IsNullOrEmpty(City))
                {
                    result += "-" + City;
                    if (!string.IsNullOrEmpty(Dist))
                        result += "-" + Dist;
                }
                return result.Trim('-');
            }
        }
    }
}
