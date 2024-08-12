using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetApplicationByIdQuery : IRequest<Application>
{
    public GetApplicationByIdQuery(int Id) => this.Id = Id;
    public int Id { get; set; }
}