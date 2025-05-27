using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Manager.Models;

public class Customer
{
    public required string Id { get; set; }          // Unique ID for the record
    public required string Month { get; set; }       // Month name like "May"
    public required string Date { get; set; }        // Day like "21"
    public required string Editor { get; set; }      // Username of logged-in user
    public required string Name { get; set; }        // Customer name
    public required string Email { get; set; }       // Customer email
    public string SME { get; set; } = "";
    public string SV { get; set; } = "";

    public string SmeSv => $"{SME} / {SV}";
    public string MonthDate => $"{Month} {Date}";

}
