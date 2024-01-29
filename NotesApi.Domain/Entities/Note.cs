using NotesApi.Domain.Entities.ElectronicBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Domain.Entities
{
    public class Note : BaseEntity<int>
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<Tag> Tags { get; set; }
    }
}
