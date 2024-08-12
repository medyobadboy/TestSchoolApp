using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.CQRS.Command;
using SchoolApi.CQRS.Query;
using SchoolApi.Models;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<Student> _validator;

        public StudentsController(IMediator mediator, IValidator<Student> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _mediator.Send(new GetAllStudentsQuery());
            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _mediator.Send(new GetStudentByIdQuery(id));

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> PutStudent(int id, Student student)
        {
            var result = await _validator.ValidateAsync(student);

            if (result.IsValid && id == student.Id)
            {
                var updated_student = await _mediator.Send(new UpdateStudentCommand(student));
                return Ok(updated_student);
            }
            else
            {
                return BadRequest();
            }
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            var result = await _validator.ValidateAsync(student);

            if (result.IsValid)
            {
                var new_student = await _mediator.Send(new CreateStudentCommand(student));

                return CreatedAtAction("GetStudent", new { id = new_student.Id }, new_student);
            }
            else
            {
                return BadRequest();
            }

        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var result = await _mediator.Send(new DeleteStudentCommand(id));
            if (result is null)
                return BadRequest();
            return NoContent();
        }
    }
}
