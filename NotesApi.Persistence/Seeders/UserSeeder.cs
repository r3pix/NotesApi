using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotesApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Persistence.Seeders
{
    public class UserSeeder
    {
        private readonly IServiceProvider _service;

        public UserSeeder(IServiceProvider service)
        {
            _service = service;
        }

        public static async Task Seed(NotesApiDbContext context)
        {
            await context.Users.AddRangeAsync(
                       new User
                       {
                           Email = "user@com.pl",
                           PasswordHash = "AQAAAAEAACcQAAAAEBaEMPUzQ5i1WkTaZ0VMolEIb0TO8Nq2dKV7shMJOMAYYLPyCRRb31ulzwF0lr4rAA==",
                           RoleId = (await context.Roles.FirstOrDefaultAsync(x => x.Name == "User")).Id,
                           CreateDate = DateTime.UtcNow,
                           LMDate = DateTime.UtcNow,
                           IsActive = true,
                           CreateEmail = "system@com.pl",
                           LMEmail = "system@com.pl"
                       });
            await context.SaveChangesAsyncWithoutUser();
        }

        public static async Task SeedRoles(NotesApiDbContext context)
        {
            await context.Roles.AddRangeAsync(new Role[]
                    {
                        new Role { Name = "User", CreateDate = DateTime.UtcNow, LMDate = DateTime.UtcNow, CreateEmail = "system", LMEmail = "system" }
                    });
            await context.SaveChangesAsyncWithoutUser();
        }

        public async Task Seed()
        {
            using (var scope = _service.CreateAsyncScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<NotesApiDbContext>();

                if (!await _dbContext.Roles.AnyAsync())
                {
                    await SeedRoles(_dbContext);
                }

                if (!await _dbContext.Users.AnyAsync())
                {
                    await Seed(_dbContext);
                }
            }
        }
    }
}
