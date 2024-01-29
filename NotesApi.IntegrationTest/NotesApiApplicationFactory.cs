using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NotesApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.IntegrationTest
{
    public class NotesApiApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _tempDbName;

        public NotesApiApplicationFactory(string tempDbName)
        {
            _tempDbName = tempDbName;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<NotesApiDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<NotesApiDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_tempDbName);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<NotesApiDbContext>();
             

                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
