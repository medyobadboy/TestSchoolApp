using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetAllApplicationsQuery : IRequest<IEnumerable<Application>>
{
}