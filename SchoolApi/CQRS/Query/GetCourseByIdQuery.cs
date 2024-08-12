using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetCourseByIdQuery : IRequest<Course>
{
    public GetCourseByIdQuery(int Id) => this.Id = Id;
    public int Id { get; set; }
}