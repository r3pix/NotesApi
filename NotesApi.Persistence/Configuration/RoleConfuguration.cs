using NotesApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Persistence.Configuration
{
    public class RoleConfiguration : BaseEntityConfiguration<int, Role>
    {
        public RoleConfiguration() : base("Role")
        {
        }
    }
}
