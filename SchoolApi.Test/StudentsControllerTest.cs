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
public class StudentsControllerTest
{
    private readonly StudentsController _controller;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IValidator<Student>> _validator;
    public StudentsControllerTest()
    {
        _mediator = new Mock<IMediator>();
        _validator = new Mock<IValidator<Student>>();
        _controller = new StudentsController(_mediator.Object, _validator.Object);
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetStudentsShouldReturnAllStudents()
    {
        var students = new List<Student>
        {
            new(),
            new()
        };
        _mediator
        .Setup(m => m.Send(It.IsAny<GetAllStudentsQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(students);

        var actionResult = await _controller.GetStudents();
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(students);
    }

    [Test]
    public async Task GetStudentShouldReturnStudentById()
    {
        var student = new Student { Id = 1, FirstName = "Ivan", LastName = "Ivan" };
        var students = new List<Student>
        {
            student,
            new(){ Id=2, FirstName="Agon", LastName="Agon"}
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(student);

        var actionResult = await _controller.GetStudent(1);
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(student);
    }

    [Test]
    public async Task GetStudentShouldReturnNotFound()
    {
        var student = new Student { Id = 1, FirstName = "Ivan", LastName = "Ivan" };
        var students = new List<Student>
        {
            student,
            new(){ Id=2, FirstName="Agon", LastName="Agon"}
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Student);

        var actionResult = await _controller.GetStudent(3);

        actionResult.Result.ShouldBeOfType<NotFoundResult>();

    }

    [Test]
    public async Task PutStudentShouldUpdateStudent()
    {
        var student = new Student { Id = 1, FirstName = "Ivan", LastName = "Ivan" };
        var students = new List<Student>
        {
            student,
            new(){ Id=2, FirstName="Agon", LastName="Agon"}
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<UpdateStudentCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() =>
        {
            student.FirstName = "Ovan";
            student.LastName = "Ovan";
            student.Email = "a@mail.com";
        })
        .ReturnsAsync(student);

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PutStudent(1, student);
        var result = actionResult.Result as OkObjectResult;

        result.Value.ShouldBe(student);
        student.FirstName.ShouldBe("Ovan");
        student.LastName.ShouldBe("Ovan");
        student.Email.ShouldBe("a@mail.com");
    }

    [Test]
    public async Task PutStudentShouldReturnBadRequest()
    {
        var student = new Student { Id = 1, FirstName = "Ivan", LastName = "Ivan" };
        var student2 = new Student { Id = 2, FirstName = "Agon", LastName = "Agon" };
        var students = new List<Student>
        {
            student,
            student2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Student);

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PutStudent(2, student);
        actionResult.Result.ShouldBeOfType<BadRequestResult>();

        actionResult = await _controller.PutStudent(1, student2);
        actionResult.Result.ShouldBeOfType<BadRequestResult>();
    }

    [Test]
    public async Task DeleteStudentShouldReturnBadRequest()
    {
        var student = new Student { Id = 1, FirstName = "Ivan", LastName = "Ivan" };
        var student2 = new Student { Id = 2, FirstName = "Agon", LastName = "Agon" };
        var students = new List<Student>
        {
            student,
            student2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<DeleteStudentCommand>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(null as Student);

        var actionResult = await _controller.DeleteStudent(3);
        actionResult.ShouldBeOfType<BadRequestResult>();
    }

    [Test]
    public async Task DeleteStudentShouldRemoveStudent()
    {
        var student = new Student { Id = 1, FirstName = "Ivan", LastName = "Ivan" };
        var student2 = new Student { Id = 2, FirstName = "Agon", LastName = "Agon" };
        var students = new List<Student>
        {
            student,
            student2
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<DeleteStudentCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() => students.Remove(student))
        .ReturnsAsync(student);

        var actionResult = await _controller.DeleteStudent(3);
        actionResult.ShouldBeOfType<NoContentResult>();
        students.ShouldNotContain(student);
    }

    [Test]
    public async Task PostStudentShouldReturnNewStudent()
    {
        var student = new Student { Id = 1, FirstName = "Ivan", LastName = "Ivan" };
        var student2 = new Student { Id = 2, FirstName = "Agon", LastName = "Agon" };
        var students = new List<Student>
        {
            student
        };

        _mediator
        .Setup(m => m.Send(It.IsAny<CreateStudentCommand>(), It.IsAny<CancellationToken>()))
        .Callback(() => students.Add(student2))
        .ReturnsAsync(student2);

        _validator
        .Setup(v => v.ValidateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var actionResult = await _controller.PostStudent(student2);
        var result = actionResult.Result as CreatedAtActionResult;
        students.ShouldContain(student2);
        result.Value.ShouldBe(student2);
    }
}