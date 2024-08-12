using FluentValidation;

namespace SchoolApi.Models.Validators;

public class ApplicationValidator : AbstractValidator<Application>
{
    public ApplicationValidator()
    {
        RuleFor(s=> s.StudentId).NotEmpty();
        RuleFor(s=> s.CourseId).NotEmpty();
        RuleFor(s=> s.ApplicationDate).NotEmpty();
    }
}