using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutOfLensWebsite.Models
{
    public class HomeModel
    {
        public Employee Employee { get; set; }
        public Login LoginData { get; set; }
    }

    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
