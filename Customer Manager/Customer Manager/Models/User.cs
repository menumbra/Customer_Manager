using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Manager.Models;

public class User
{
    //public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; } // TODO: Use hashing in production
    public required string Role { get; set; } // "user" or "admin"
}
