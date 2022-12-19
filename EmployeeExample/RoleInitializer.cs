using EmployeeExample.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeExample
{
    public static class RoleInitializer
    {
        public static async Task Initialize(IServiceProvider servicerProvider, IConfiguration configuration, CancellationToken cancellationToken = default)
        {
            // Ensure the database is created before trying to add roles
            var _dbContext = servicerProvider.GetRequiredService<ApplicationDbContext>();

            await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

            // Get the list of default roles to add from our configuration
            var defaultRoles = configuration.GetSection("DefaultRoles")?.GetChildren();

            // Get the role manager to check if the needed roles exist, and if they don't
            // create them.
            var _roleManager = servicerProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach(var role in defaultRoles)
            {
                if(!await _roleManager.RoleExistsAsync(role.Value))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.Value));
                }
            }
        }
    }
}
