using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace ElectronicBookingSystem.Domain.Entities
    {
        public abstract class BaseEntity
        {
            public DateTime CreateDate { get; set; }
            public DateTime LMDate { get; set; }
            public string? CreateEmail { get; set; }
            public string? LMEmail { get; set; }
        }

        public abstract class BaseEntity<TKey> : BaseEntity where TKey : struct
        {
            public TKey Id { get; set; }

            public bool IsActive { get; set; }

            public BaseEntity()
            {
                IsActive = true;
            }
        }
    }

}
