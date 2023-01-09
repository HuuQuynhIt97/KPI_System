using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO.auth
{
    public class UserForLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserID { get; set; }
    }
}
