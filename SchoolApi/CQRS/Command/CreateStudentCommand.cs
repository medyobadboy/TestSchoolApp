using MediatR;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class CreateStudentCommand : IRequest<Student>
{
    public CreateStudentCommand(Student student)
    {
        this.FirstName = student.FirstName;
        this.LastName = student.LastName;
        this.DateOfBirth = student.DateOfBirth;
        this.Email = student.Email;
        this.PhoneNumber = student.PhoneNumber;
    }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set;}
    public string? PhoneNumber { get; set; }
}