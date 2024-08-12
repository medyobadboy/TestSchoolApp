using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class CreateCourseCommand : IRequest<Course>
{
    public CreateCourseCommand(Course course)
    {
        this.Code = course.Code;
        this.Title = course.Title;
        this.Credits = course.Credits;
    }

    public string? Code { get; set; }
    public string? Title { get; set; }
    public int? Credits { get; set; }
}