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
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<Course> _validator;

        public CoursesController(IMediator mediator, IValidator<Course> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var courses = await _mediator.Send(new GetAllCoursesQuery());
            return Ok(courses);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(id));

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> PutCourse(int id, Course course)
        {
            var result = await _validator.ValidateAsync(course);

            if (result.IsValid && id == course.Id)
            {
                var updated_course = await _mediator.Send(new UpdateCourseCommand(course));
                return Ok(updated_course);
            }
            else
            {
                return BadRequest();
            }
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            var result = await _validator.ValidateAsync(course);

            if (result.IsValid)
            {
                var new_course = await _mediator.Send(new CreateCourseCommand(course));
                return CreatedAtAction("GetCourse", new { id = new_course.Id }, new_course);
            }
            else
            {
                return BadRequest();
            }

        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _mediator.Send(new DeleteCourseCommand(id));
            if (result is null)
                return BadRequest();

            return NoContent();
        }
    }
}
