using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<Student>>
{
    private readonly SchoolDbContext _context;
    public GetAllStudentsQueryHandler(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken) =>
        await _context.Students.ToListAsync();
}
