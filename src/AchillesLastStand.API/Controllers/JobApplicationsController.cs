using AchillesLastStand.Application.Interfaces;
using AchillesLastStand.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AchillesLastStand.API.Controllers
{
    // API LAYER - CONTROLLER (Presentation Layer)
    // This controller handles HTTP requests and returns HTTP responses.
    // Following RESTful API principles and best practices.
    // Uses Dependency Injection to get IJobApplicationRepository.
    [ApiController]
    [Route("api/[controller]")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationRepository _repository;

        // Constructor Injection - ASP.NET Core DI container injects the repository
        public JobApplicationsController(IJobApplicationRepository repository)
        {
            _repository = repository;
        }

        // GET: api/jobapplications
        // Retrieves all job applications
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<JobApplication>>> GetAll()
        {
            var applications = await _repository.GetAllAsync();
            return Ok(applications);
        }

        // GET: api/jobapplications/search?company=Microsoft&role=Developer
        // Search/filter job applications by company and/or role
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<JobApplication>>> Search(
            [FromQuery] string? company = null,
            [FromQuery] string? role = null)
        {
            var applications = await _repository.SearchAsync(company, role);
            return Ok(applications);
        }


        // GET: api/jobapplications/5
        // Retrieves a single job application by ID
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobApplication>> GetById(int id)
        {
            var application = await _repository.GetByIdAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return Ok(application);
        }

        // POST: api/jobapplications
        // Creates a new job application
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JobApplication>> Create([FromBody] JobApplication jobApplication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _repository.CreateAsync(jobApplication);

            // Returns 201 Created with Location header pointing to the new resource
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/jobapplications/5
        // Updates an existing job application
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] JobApplication jobApplication)
        {
            if (id != jobApplication.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(jobApplication);

            // 204 No Content is standard for successful PUT
            return NoContent();
        }

        // DELETE: api/jobapplications/5
        // Deletes a job application
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}