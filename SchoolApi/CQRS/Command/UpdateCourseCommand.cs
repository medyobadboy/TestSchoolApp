using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class UpdateCourseCommand : IRequest<Course>
{
    public UpdateCourseCommand(Course course)
    {
        this.Id = course.Id;
        this.Code = course.Code;
        this.Title = course.Title;
        this.Credits = course.Credits;
    }
    
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Title { get; set; }
    public int? Credits { get; set; }
}