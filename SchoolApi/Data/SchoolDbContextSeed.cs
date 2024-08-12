using SchoolApi.Models;

namespace SchoolApi.Data;

public class SchoolDbContextSeed
{
    public static void SeedAsync(SchoolDbContext schoolDbContext)
    {
        if (!schoolDbContext.Students.Any())
        {
            schoolDbContext.Students.AddRange(
                new Student { FirstName = "Ivan", LastName = "Ivan", DateOfBirth = DateTime.Parse("2000/01/01"), Email = "ivan@email.com", PhoneNumber = "+123456789" },
                new Student { FirstName = "Peter", LastName = "Porker", DateOfBirth = DateTime.Parse("1989/11/21"), Email = "porky@email.com", PhoneNumber = "+123456789" }
            );

            schoolDbContext.SaveChanges();
        }

        if (!schoolDbContext.Courses.Any())
        {
            schoolDbContext.Courses.AddRange(
                new Course { Code = "MATH101", Title = "Mathematics 101", Credits = 3 },
                new Course { Code = "BIO101", Title = "Biology 101", Credits = 5 }
            );

            schoolDbContext.SaveChanges();
        }

        if (!schoolDbContext.Applications.Any())
        {
            schoolDbContext.Applications.AddRange(
                new Application { StudentId = 1, CourseId = 1, ApplicationDate = DateTime.Now },
                new Application { StudentId = 1, CourseId = 2, ApplicationDate = DateTime.Now },
                new Application { StudentId = 2, CourseId = 1, ApplicationDate = DateTime.Now }
            );

            schoolDbContext.SaveChanges();
        }

    }

}