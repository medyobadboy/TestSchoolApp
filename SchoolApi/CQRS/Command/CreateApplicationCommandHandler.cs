using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Application>
{
    private readonly SchoolDbContext _dbContext;

    public CreateApplicationCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Application> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = new Application
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            ApplicationDate = request.ApplicationDate,
        };

        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();
        return application;
    }
}