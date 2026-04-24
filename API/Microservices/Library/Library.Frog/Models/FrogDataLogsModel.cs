using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.Models
{
    public class FrogDataLogsModel
    {
        public string? UsersUUID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? IDNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? UsersUUIDChange { get; set; }
        public string? NameChange { get; set; }
        public string? EmailChange { get; set; }
        public string? IDNumberChange { get; set; }
        public string? EmployeeNumberChange { get; set; }
        public string? FrogElementsUUID { get; set; }
        public string? FrogElements { get; set; }
        public DateTime DateCaptured { get; set; }
        public int? Itemid_Old { get; set; }
        public string? Uniqueid_Old { get; set; }
        public string? ItemName_Old { get; set; }
        public int? Itemid { get; set; } 
        public string? Uniqueid { get; set; } 
        public string? ItemName { get; set; }
    }
}
