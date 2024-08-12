using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;

namespace SchoolApi.Data;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Application> Applications { get; set; } = null!;
}