using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolApi.Controllers;
using SchoolApi.CQRS.Command;
using SchoolApi.CQRS.Query;
using SchoolApi.Models;
using Shouldly;

namespace SchoolApi.Test;

[TestFixture]
public class ApplicationsControllerTest
{
    private readonly ApplicationsController _controller;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IValidator<Application>> _validator;
    private List<Student> students;
    private List<Course> courses;
    public ApplicationsControllerTest()
    {
        _mediator = new Mock<IMediator>();
        _validator = new Mock<IValidator<Application>>();
        _controller = new ApplicationsController(_mediator.Object, _validator.Object);
    }

    [SetUp]
    public void Setup()
    {
        students =
        [
            new(){ Id=1, FirstName="Ivan", LastName="Ivan"},
            new(){ Id=2, FirstName="Agon", LastName="Agon"}
        ];

        courses =
        [
            new (){ Id=1, Code="MATH101", Title="Mathematics 101", Credits=3 },
            new (){ Id=2, Code="SCIE101", Title="Science 101", Credits=5 }
        ];
    }

    [Test]
    public async Task GetApplicationsShouldReturnAllApplications()
    {
        var applications = new List<Application>
        {
            new(),
            new()
        };
        _mediator
        .Setup(m => m.Send(It.IsAny<GetAllApplicationsQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(applications);

        var actionResult = await _controller.GetApplications();
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(applications);
    }

    [Test]
    public async Task GetApplicationShouldReturnApplicationById()
    {
        var application = new Application { Id = 1, StudentId = 1, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var application2 = new Application { Id = 2, StudentId = 2, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var applications = new List<Application>
        {
            application,
            application2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetApplicationByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(application);

        var actionResult = await _controller.GetApplication(1);
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(application);
    }

    [Test]
    public async Task GetApplicationShouldReturnNotFound()
    {
        var application = new Application { Id = 1, StudentId = 1, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var application2 = new Application { Id = 2, StudentId = 2, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var applications = new List<Application>
        {
            application,
            application2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetApplicationByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Application);

        var actionResult = await _controller.GetApplication(3);

        actionResult.Result.ShouldBeOfType<NotFoundResult>();

    }

    [Test]
    public async Task PutApplicationShouldUpdateApplication()
    {
        var application = new Application { Id = 1, StudentId = 1, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var application2 = new Application { Id = 2, StudentId = 2, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var applications = new List<Application>
        {
            application,
            application2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<UpdateApplicationCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() =>
        {
            application.StudentId = 2;
            application.CourseId = 2;
            application.ApplicationDate = new DateTime(123456789);
        })
        .ReturnsAsync(application);

        _mediator
        .Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(students.First());

        _mediator
        .Setup(m => m.Send(It.IsAny<GetCourseByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(courses.First());

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Application>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PutApplication(1, application);
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(application);
        application.StudentId.ShouldBe(2);
        application.CourseId.ShouldBe(2);
        application.ApplicationDate.ShouldBe(new DateTime(123456789));
    }

    [Test]
    public async Task PutApplicationShouldReturnBadRequest()
    {
        var application = new Application { Id = 1, StudentId = 1, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var application2 = new Application { Id = 2, StudentId = 2, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var applications = new List<Application>
        {
            application,
            application2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetApplicationByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Application);

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Application>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PutApplication(2, application);
        actionResult.Result.ShouldBeOfType<BadRequestResult>();

        actionResult = await _controller.PutApplication(1, application2);
        actionResult.Result.ShouldBeOfType<BadRequestResult>();
    }

    [Test]
    public async Task DeleteApplicationShouldReturnBadRequest()
    {
        var application = new Application { Id = 1, StudentId = 1, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var application2 = new Application { Id = 2, StudentId = 2, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var applications = new List<Application>
        {
            application,
            application2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<DeleteApplicationCommand>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Application);

        var actionResult = await _controller.DeleteApplication(3);
        actionResult.ShouldBeOfType<BadRequestResult>();
    }

    [Test]
    public async Task DeleteApplicationShouldRemoveApplication()
    {
        var application = new Application { Id = 1, StudentId = 1, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var application2 = new Application { Id = 2, StudentId = 2, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var applications = new List<Application>
        {
            application,
            application2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<DeleteApplicationCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() => applications.Remove(application))
        .ReturnsAsync(application);

        var actionResult = await _controller.DeleteApplication(3);
        actionResult.ShouldBeOfType<NoContentResult>();
        applications.ShouldNotContain(application);
    }

    [Test]
    public async Task PostApplicationShouldReturnNewApplication()
    {
        var application = new Application { Id = 1, StudentId = 1, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var application2 = new Application { Id = 2, StudentId = 2, CourseId = 1, ApplicationDate = new DateTime(5432154321) };
        var applications = new List<Application>
        {
            application
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<CreateApplicationCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() => applications.Add(application2))
        .ReturnsAsync(application2);

        _mediator
        .Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(students.First());

        _mediator
        .Setup(m => m.Send(It.IsAny<GetCourseByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(courses.First());

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Application>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PostApplication(application2);
        var result = actionResult.Result as CreatedAtActionResult;
        result.Value.ShouldBe(application2);
        applications.ShouldContain(application2);
    }
}