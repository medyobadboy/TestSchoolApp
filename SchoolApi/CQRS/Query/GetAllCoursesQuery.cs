using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetAllCoursesQuery : IRequest<IEnumerable<Course>>
{
}