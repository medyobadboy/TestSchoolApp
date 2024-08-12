using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Course>
{
    private readonly SchoolDbContext _dbContext;

    public UpdateCourseCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Course> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = _dbContext.Courses.FirstOrDefault(p => p.Id == request.Id);

        if (course is null)
            return default;

        course.Code = request.Code;
        course.Title = request.Title;
        course.Credits = request.Credits;

        await _dbContext.SaveChangesAsync();
        return course;
    }
}