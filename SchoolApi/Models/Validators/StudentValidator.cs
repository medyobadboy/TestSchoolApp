using FluentValidation;

namespace SchoolApi.Models.Validators;

public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(s=> s.FirstName).NotEmpty()
                                .Length(2,30)
                                .Matches(@"^[A-Za-z ]+$");
        RuleFor(s=> s.LastName).NotEmpty()
                               .Length(2,30)
                               .Matches(@"^[A-Za-z ]+$");
        RuleFor(s=> s.DateOfBirth).NotEmpty();
        RuleFor(s=> s.Email).EmailAddress().When(s => !string.IsNullOrEmpty(s.Email));
    }
}