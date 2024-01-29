using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Persistence.Configuration
{
    public class UserConfiguration : BaseEntityConfiguration<int, User>
    {
        public UserConfiguration() : base("User")
        {
        }

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);
            builder.HasMany(x => x.Notes).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            base.Configure(builder);
        }
    }
}