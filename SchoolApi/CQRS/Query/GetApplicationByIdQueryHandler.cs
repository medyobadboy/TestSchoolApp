using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetApplicationByIdQueryHandler : IRequestHandler<GetApplicationByIdQuery, Application>
{
    private readonly SchoolDbContext _context;
    public GetApplicationByIdQueryHandler(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<Application> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken) =>
        await _context.Applications.Include(a=>a.Course).Include(a=>a.Student).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
}
