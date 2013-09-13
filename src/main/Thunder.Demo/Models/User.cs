using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.Demo.Models
{
    public class User
    {
        [StringLength(50)]
        public string Name { get; set; }

        public string Password { get; set; }

        public string Document { get; set; }

        public decimal Currency { get; set; }

        public DateTime Date { get; set; }
    }
}