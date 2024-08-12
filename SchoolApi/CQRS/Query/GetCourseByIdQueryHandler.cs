using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, Course>
{
    private readonly SchoolDbContext _context;
    public GetCourseByIdQueryHandler(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<Course> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken) =>
        await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
}
