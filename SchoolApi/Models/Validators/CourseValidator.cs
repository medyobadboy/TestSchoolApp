using FluentValidation;

namespace SchoolApi.Models.Validators;

public class CourseValidator : AbstractValidator<Course>
{
    public CourseValidator()
    {
        RuleFor(s=> s.Code).NotEmpty()
                                .Length(5,10)
                                .Matches(@"^[A-Za-z0-9]+$");
        RuleFor(s=> s.Title).NotEmpty()
                               .Length(5,50)
                               .Matches(@"^[A-Za-z0-9!@#$%^&*()-+-={};:',.<>?/\[\] ]+$");
        RuleFor(s=> s.Credits).NotEmpty().GreaterThan(0);
    }
}