using NotesApi.Domain.Entities.ElectronicBookingSystem.Domain.Entities;
using NotesApi.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Domain.Entities
{
    public class Tag : BaseEntity<int>
    {
        public string Name { get; set; }
        public TagTypes Type { get; set; }

        public virtual List<Note> Notes{ get; set; }
    }
}
