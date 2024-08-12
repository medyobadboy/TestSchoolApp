using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Student>
{
    private readonly SchoolDbContext _dbContext;

    public CreateStudentCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Student> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();
        return student;
    }
}