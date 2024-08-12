using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, IEnumerable<Course>>
{
    private readonly SchoolDbContext _context;
    public GetAllCoursesQueryHandler(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken) =>
        await _context.Courses.ToListAsync();
}
