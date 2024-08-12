using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;

public class DeleteCourseCommand : IRequest<Course>
{
    public DeleteCourseCommand(int Id) => this.Id = Id;
    public int Id { get; set; }
}