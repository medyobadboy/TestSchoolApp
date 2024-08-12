using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class UpdateApplicationCommand : IRequest<Application>
{
    public UpdateApplicationCommand(Application application)
    {
        this.Id = application.Id;
        this.StudentId = application.StudentId;
        this.CourseId = application.CourseId;
        this.ApplicationDate = application.ApplicationDate;
    }

    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime ApplicationDate { get; set; }
}