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
            var courses = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .Select(c => new
                {
                    c.Id,
                    c.Crs_name,
                    c.Crs_desc,
                    c.Duration,
                    c.DeptID,
                    Department = new { c.Department.DeptID, c.Department.Name }
                })
                .ToListAsync();

            if (!courses.Any())
                return NotFound();
            return Ok(courses);
        }

        // GET: api/Courses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Crs_name,
                    c.Crs_desc,
                    c.Duration,
                    c.DeptID,
                    Department = new { c.Department.DeptID, c.Department.Name }
                })
                .FirstOrDefaultAsync();

            if (course == null)
                return NotFound();
            return Ok(course);
        }

        // GET: api/Courses/byName/{name}
        [HttpGet("byName/{name}")]
        public async Task<IActionResult> CourseByName(string name)
        {
            var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .Where(c => c.Crs_name == name)
                .Select(c => new
                {
                    c.Id,
                    c.Crs_name,
                    c.Crs_desc,
                    c.Duration,
                    c.DeptID,
                    Department = new { c.Department.DeptID, c.Department.Name }
                })
                .FirstOrDefaultAsync();

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course course)
        {
            if (course == null || string.IsNullOrEmpty(course.Crs_name))
                return BadRequest();

            // Verify department exists
            var department = await _context.Departments.FindAsync(course.DeptID);
            if (department == null)
                return BadRequest("Department not found");

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // PUT: api/Courses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Course course)
        {
            if (id != course.Id)
                return BadRequest();

            var existing = await _context.Courses.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Verify department exists if changed
            if (existing.DeptID != course.DeptID)
            {
                var department = await _context.Departments.FindAsync(course.DeptID);
                if (department == null)
                    return BadRequest("Department not found");
            }

            existing.Crs_name = course.Crs_name;
            existing.Crs_desc = course.Crs_desc;
            existing.Duration = course.Duration;
            existing.DeptID = course.DeptID;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Courses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            var remaining = await _context.Courses.ToListAsync();
            return Ok(remaining);
        }
    }
}