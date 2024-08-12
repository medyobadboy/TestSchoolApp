using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Student>
{
    private readonly SchoolDbContext _context;
    public GetStudentByIdQueryHandler(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<Student> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken) =>
        await _context.Students.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
}
