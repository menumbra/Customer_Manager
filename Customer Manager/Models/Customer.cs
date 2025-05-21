using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Manager.Models;

public class Customer
{
    public string? Id { get; set; }          // Unique ID for the record
    public string? Month { get; set; }       // Month name like "May"
    public string? Date { get; set; }        // Day like "21"
    public string? Editor { get; set; }      // Username of logged-in user
    public string? Name { get; set; }        // Customer name
    public string? Email { get; set; }       // Customer email
}
