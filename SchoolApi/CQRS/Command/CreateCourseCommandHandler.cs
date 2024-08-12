using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course>
{
    private readonly SchoolDbContext _dbContext;

    public CreateCourseCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Code = request.Code,
            Title = request.Title,
            Credits = request.Credits,
        };

        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
        return course;
    }
}