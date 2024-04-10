using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Models
{
    public class Contact
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Phone { get; set; }
        public required Guid Owner { get; set; }
    }
}