using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Persistence.Configuration
{
    public class TagConfiguration : BaseEntityConfiguration<int, Tag>
    {
        public TagConfiguration() : base("Tag")
        {
        }

        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasMany(x => x.Notes).WithMany(x => x.Tags);
            base.Configure(builder);
        }
    }
}
