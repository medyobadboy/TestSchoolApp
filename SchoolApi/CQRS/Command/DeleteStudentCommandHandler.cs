using MediatR;
using SchoolApi.Data;
using SchoolApi.Models;

namespace SchoolApi.CQRS.Command;
public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Student>
{
    private readonly SchoolDbContext _dbContext;

    public DeleteStudentCommandHandler(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Student> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = _dbContext.Students.FirstOrDefault(p => p.Id == request.Id);

        if (student is null)
            return default;
       
        _dbContext.Remove(student);
        await _dbContext.SaveChangesAsync();
        return student;
    }
}