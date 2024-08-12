using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand, Application>
{
    private readonly SchoolDbContext _dbContext;

    public DeleteApplicationCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Application> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = _dbContext.Applications.FirstOrDefault(p => p.Id == request.Id);

        if (application is null)
            return default;
       
        _dbContext.Remove(application);
        await _dbContext.SaveChangesAsync();
        return application;
    }
}