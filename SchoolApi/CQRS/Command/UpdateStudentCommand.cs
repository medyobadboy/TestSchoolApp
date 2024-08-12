using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class UpdateStudentCommand : IRequest<Student>
{
    public UpdateStudentCommand(Student student)
    {
        this.Id = student.Id;
        this.FirstName = student.FirstName;
        this.LastName = student.LastName;
        this.DateOfBirth = student.DateOfBirth;
        this.Email = student.Email;
        this.PhoneNumber = student.PhoneNumber;
    }

    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}