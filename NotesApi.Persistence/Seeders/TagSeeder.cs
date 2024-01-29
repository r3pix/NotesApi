using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotesApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Persistence.Seeders
{
    public class TagSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public TagSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static async Task Seed(NotesApiDbContext context)
        {
            await context.Tags.AddRangeAsync(new Tag[]
                    {
                        new Tag { Name = "Phone", CreateDate = DateTime.UtcNow, CreateEmail = "system@com.pl", LMDate = DateTime.UtcNow, LMEmail = "system@com.pl", Type = Domain.ValueObject.TagTypes.Phone },
                        new Tag { Name = "Email", CreateDate = DateTime.UtcNow, CreateEmail = "system@com.pl", LMDate = DateTime.UtcNow, LMEmail = "system@com.pl", Type = Domain.ValueObject.TagTypes.Email },
                        new Tag { Name = "None", CreateDate = DateTime.UtcNow, CreateEmail = "system@com.pl", LMDate = DateTime.UtcNow, LMEmail = "system@com.pl", Type = Domain.ValueObject.TagTypes.None }
                    });
            await context.SaveChangesAsyncWithoutUser();
        }

        public async Task Seed()
        {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<NotesApiDbContext>();

                if (! await _dbContext.Tags.AnyAsync())
                {
                    await Seed(_dbContext);
                }
            }
        }
    }
}
