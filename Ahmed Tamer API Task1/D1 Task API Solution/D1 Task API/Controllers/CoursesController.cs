using D1_Task_API.Data.DbContexts;
using D1_Task_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace D1_Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var courses = await _context.Courses.ToListAsync();
            if (!courses.Any())
                return NotFound(); // 404
            return Ok(courses); // 200
        }

        // GET: api/Courses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(); // 404
            return Ok(course); // 200
        }

        // GET: api/Courses/byName/{name}
        [HttpGet("byName/{name}")]
        public async Task<IActionResult> CourseByName(string name)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Crs_name == name);

            if (course == null)
                return NotFound(); // 404

            return Ok(course); // 200
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course course)
        {
            if (course == null)
                return BadRequest(); // 400

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return StatusCode(201); // 201
        }

        // PUT: api/Courses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Course course)
        {
            if (id != course.Id)
                return BadRequest(); // 400

            var existing = await _context.Courses.FindAsync(id);
            if (existing == null)
                return NotFound(); // 404

            existing.Crs_name = course.Crs_name;
            existing.Crs_desc = course.Crs_desc;
            existing.Duration = course.Duration;

            await _context.SaveChangesAsync();
            return NoContent(); // 204
        }

        // DELETE: api/Courses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(); // 404

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            var remaining = await _context.Courses.ToListAsync();
            return Ok(remaining); // 200 + list
        }
    }
}
