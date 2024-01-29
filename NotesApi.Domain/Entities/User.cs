using NotesApi.Domain.Entities.ElectronicBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Domain.Entities
{
    public class User : BaseEntity<int>
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual List<Note> Notes { get; set; }
    }
    
}
