using NotesApi.Domain.Entities.ElectronicBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Domain.Entities
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }

    }
}
