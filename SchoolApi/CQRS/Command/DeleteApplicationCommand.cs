using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;

public class DeleteApplicationCommand : IRequest<Application>
{
    public DeleteApplicationCommand(int Id) => this.Id = Id;

    public int Id { get; set; }
}