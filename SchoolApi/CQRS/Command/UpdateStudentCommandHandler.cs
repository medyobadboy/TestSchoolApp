using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Student>
{
    private readonly SchoolDbContext _dbContext;

    public UpdateStudentCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Student> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = _dbContext.Students.FirstOrDefault(p => p.Id == request.Id);

        if (student is null)
            return default;

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.DateOfBirth = request.DateOfBirth;
        student.Email = request.Email;
        student.PhoneNumber = request.PhoneNumber;

        await _dbContext.SaveChangesAsync();
        return student;
    }
}