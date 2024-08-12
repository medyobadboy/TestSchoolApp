using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetAllApplicationsQueryHandler : IRequestHandler<GetAllApplicationsQuery, IEnumerable<Application>>
{
    private readonly SchoolDbContext _context;
    public GetAllApplicationsQueryHandler(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Application>> Handle(GetAllApplicationsQuery request, CancellationToken cancellationToken) =>
        await _context.Applications.Include(a=>a.Course).Include(a=>a.Student).ToListAsync();
}
