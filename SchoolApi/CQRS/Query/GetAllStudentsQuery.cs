using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetAllStudentsQuery : IRequest<IEnumerable<Student>>
{
}