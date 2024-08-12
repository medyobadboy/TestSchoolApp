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
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<Application> _validator;

        public ApplicationsController(IMediator mediator, IValidator<Application> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            var applications = await _mediator.Send(new GetAllApplicationsQuery());
            return Ok(applications);
        }

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var application = await _mediator.Send(new GetApplicationByIdQuery(id));

            if (application == null)
            {
                return NotFound();
            }

            return Ok(application);
        }

        // PUT: api/Applications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Application>> PutApplication(int id, Application application)
        {
            var student = await _mediator.Send(new GetStudentByIdQuery(application.StudentId));
            var course = await _mediator.Send(new GetCourseByIdQuery(application.CourseId));
            var result = await _validator.ValidateAsync(application);

            if (result.IsValid && student != null && course != null && id == application.Id)
            {
                application.Student = student;
                application.Course = course;
                var updated_application = await _mediator.Send(new UpdateApplicationCommand(application));
                return Ok(updated_application);
            }
            else
            {
                return BadRequest();
            }
        }

        // POST: api/Applications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Application>> PostApplication(Application application)
        {
            var student = await _mediator.Send(new GetStudentByIdQuery(application.StudentId));
            var course = await _mediator.Send(new GetCourseByIdQuery(application.CourseId));
            var result = await _validator.ValidateAsync(application);

            if (result.IsValid && student != null && course != null)
            {
                application.Student = student;
                application.Course = course;
                var new_application = await _mediator.Send(new CreateApplicationCommand(application));

                return CreatedAtAction("GetApplication", new { id = new_application.Id }, new_application);
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var result = await _mediator.Send(new DeleteApplicationCommand(id));
            if (result is null)
                return BadRequest();

            return NoContent();
        }
    }
}
