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
public class CoursesControllerTest
{
    private readonly CoursesController _controller;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IValidator<Course>> _validator;
    public CoursesControllerTest()
    {
        _mediator = new Mock<IMediator>();
        _validator = new Mock<IValidator<Course>>();
        _controller = new CoursesController(_mediator.Object, _validator.Object);
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetCoursesShouldReturnAllCourses()
    {
        var courses = new List<Course>
        {
            new(),
            new()
        };
        _mediator
        .Setup(m => m.Send(It.IsAny<GetAllCoursesQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(courses);

        var actionResult = await _controller.GetCourses();
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(courses);
    }

    [Test]
    public async Task GetCourseShouldReturnCourseById()
    {
        var course = new Course { Id = 1, Code = "MATH101", Title = "Mathematics 101", Credits = 3 };
        var courses = new List<Course>
        {
            course,
            new(){ Id=2, Code="SCIE101", Title="Science 101", Credits=5 }
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetCourseByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(course);

        var actionResult = await _controller.GetCourse(1);
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(course);
    }

    [Test]
    public async Task GetCourseShouldReturnNotFound()
    {
        var course = new Course { Id = 1, Code = "MATH101", Title = "Mathematics 101", Credits = 3 };
        var courses = new List<Course>
        {
            course,
            new(){ Id=2, Code="SCIE101", Title="Science 101", Credits=5 }
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetCourseByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Course);

        var actionResult = await _controller.GetCourse(3);

        actionResult.Result.ShouldBeOfType<NotFoundResult>();

    }

    [Test]
    public async Task PutCourseShouldUpdateCourse()
    {
        var course = new Course { Id = 1, Code = "MATH101", Title = "Mathematics 101", Credits = 3 };
        var courses = new List<Course>
        {
            course,
            new(){ Id=2, Code="SCIE101", Title="Science 101", Credits=5 }
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<UpdateCourseCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() =>
        {
            course.Code = "MATH102";
            course.Title = "Mathematics 102";
            course.Credits = 2;
        })
        .ReturnsAsync(course);

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PutCourse(1, course);
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(course);
        course.Code.ShouldBe("MATH102");
        course.Title.ShouldBe("Mathematics 102");
        course.Credits.ShouldBe(2);
    }

    [Test]
    public async Task PutCourseShouldReturnBadRequest()
    {
        var course = new Course { Id = 1, Code = "MATH101", Title = "Mathematics 101", Credits = 3 };
        var course2 = new Course { Id = 2, Code = "SCIE101", Title = "Science 101", Credits = 5 };
        var courses = new List<Course>
        {
            course,
            course2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetCourseByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Course);

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PutCourse(2, course);
        actionResult.Result.ShouldBeOfType<BadRequestResult>();

        actionResult = await _controller.PutCourse(1, course2);
        actionResult.Result.ShouldBeOfType<BadRequestResult>();
    }

    [Test]
    public async Task DeleteCourseShouldReturnBadRequest()
    {
        var course = new Course { Id = 1, Code = "MATH101", Title = "Mathematics 101", Credits = 3 };
        var course2 = new Course { Id = 2, Code = "SCIE101", Title = "Science 101", Credits = 5 };
        var courses = new List<Course>
        {
            course,
            course2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<DeleteCourseCommand>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Course);

        var actionResult = await _controller.DeleteCourse(3);
        actionResult.ShouldBeOfType<BadRequestResult>();
    }

    [Test]
    public async Task DeleteCourseShouldRemoveCourse()
    {
        var course = new Course { Id = 1, Code = "MATH101", Title = "Mathematics 101", Credits = 3 };
        var course2 = new Course { Id = 2, Code = "SCIE101", Title = "Science 101", Credits = 5 };
        var courses = new List<Course>
        {
            course,
            course2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<DeleteCourseCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() => courses.Remove(course))
        .ReturnsAsync(course);

        var actionResult = await _controller.DeleteCourse(3);
        actionResult.ShouldBeOfType<NoContentResult>();
        courses.ShouldNotContain(course);
    }

    [Test]
    public async Task PostCourseShouldReturnNewCourse()
    {
        var course = new Course { Id = 1, Code = "MATH101", Title = "Mathematics 101", Credits = 3 };
        var course2 = new Course { Id = 2, Code = "SCIE101", Title = "Science 101", Credits = 5 };
        var courses = new List<Course>
        {
            course,
            course2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<CreateCourseCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() => courses.Add(course2))
        .ReturnsAsync(course2);

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PostCourse(course2);
        var result = actionResult.Result as CreatedAtActionResult;
        courses.ShouldContain(course2);
        result.Value.ShouldBe(course2);
    }
}