using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UniversityStudentTracker.API.Contexts;

public class StudentAuth(DbContextOptions<StudentAuth> options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        const string studentRoleId = "04ab77f0-e145-4fbf-b641-989df24e5574";
        const string adminRoleId = "04ab77f0-e145-4fbf-b641-989df24e5576";

        var roles = new List<IdentityRole>
        {
            new()
            {
                Id = studentRoleId,
                ConcurrencyStamp = studentRoleId,
                Name = "StudentMetrics",
                NormalizedName = "StudentMetrics".ToUpper()
            },
            new()
            {
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId,
                Name = "Admin",
                NormalizedName = "Admin".ToUpper()
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}