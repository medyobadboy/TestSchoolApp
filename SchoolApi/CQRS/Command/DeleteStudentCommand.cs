using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;

public class DeleteStudentCommand : IRequest<Student>
{
    public DeleteStudentCommand(int Id) => this.Id = Id;

    public int Id { get; set; }
}