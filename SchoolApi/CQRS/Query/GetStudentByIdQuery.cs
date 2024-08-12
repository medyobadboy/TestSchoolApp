using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Query;

public class GetStudentByIdQuery : IRequest<Student>
{
    public GetStudentByIdQuery(int Id) => this.Id = Id;
    public int Id { get; set; }
}