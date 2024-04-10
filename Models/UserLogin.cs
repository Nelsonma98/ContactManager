using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Models
{
    public class UserLogin
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}