using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotesApi.Domain.Entities;
using NotesApi.Domain.Entities.ElectronicBookingSystem.Domain.Entities;
using NotesApi.Infrastacture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Persistence
{
    public class NotesApiDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CorrectModificationFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsyncWithoutUser()
        {
            return await base.SaveChangesAsync();
        }

        private void CorrectModificationFields()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if(entry.Entity is BaseEntity)
                {
                    if (entry.State == EntityState.Modified)
                    {
                        ((BaseEntity)entry.Entity).LMDate = DateTime.UtcNow;
                        ((BaseEntity)entry.Entity).LMEmail = _currentUserService?.Email;
                    }
                    else if (entry.State == EntityState.Added)
                    {
                        ((BaseEntity)entry.Entity).LMDate = DateTime.UtcNow;
                        ((BaseEntity)entry.Entity).LMEmail = _currentUserService?.Email;
                        ((BaseEntity)entry.Entity).CreateDate = DateTime.UtcNow;
                        ((BaseEntity)entry.Entity).CreateEmail = _currentUserService?.Email;
                    }
                }
               
            }
        }

        public NotesApiDbContext(DbContextOptions<NotesApiDbContext> options) : base(options)
        {

        }

        public NotesApiDbContext(DbContextOptions<NotesApiDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotesApiDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
