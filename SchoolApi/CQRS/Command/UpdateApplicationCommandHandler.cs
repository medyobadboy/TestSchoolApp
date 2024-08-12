using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplicationCommand, Application>
{
    private readonly SchoolDbContext _dbContext;

    public UpdateApplicationCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Application> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = _dbContext.Applications.FirstOrDefault(p => p.Id == request.Id);

        if (application is null)
            return default;

        application.StudentId = request.StudentId;
        application.CourseId = request.CourseId;
        application.ApplicationDate = request.ApplicationDate;
        
        await _dbContext.SaveChangesAsync();
        return application;
    }
}