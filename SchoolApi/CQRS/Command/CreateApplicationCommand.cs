using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class CreateApplicationCommand : IRequest<Application>
{
    public CreateApplicationCommand(Application application)
    {
        this.StudentId = application.StudentId;
        this.CourseId = application.CourseId;
        this.ApplicationDate = application.ApplicationDate;
    }

    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime ApplicationDate { get; set; }
}