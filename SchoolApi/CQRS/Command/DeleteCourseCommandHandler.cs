using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Course>
{
    private readonly SchoolDbContext _dbContext;

    public DeleteCourseCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Course> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = _dbContext.Courses.FirstOrDefault(p => p.Id == request.Id);

        if (course is null)
            return default;
       
        _dbContext.Remove(course);
        await _dbContext.SaveChangesAsync();
        return course;
    }
}