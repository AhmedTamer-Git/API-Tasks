using D1_Task_API.Data.DbContexts;
using D1_Task_API.DTOs;
using D1_Task_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace D1_Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var departments = await _context.Departments.ToListAsync();
            if (!departments.Any())
                return NotFound();
            return Ok(departments);
        }

        // GET: api/Departments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound();
            return Ok(department);
        }

        // GET: api/Departments/{id}/info - Method 1: Custom Serialization
        [HttpGet("{id}/info")]
        public async Task<IActionResult> GetDepartmentInfoSerialization(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.DeptID == id);

            if (department == null)
                return NotFound();

            var response = new
            {
                department.Name,
                CourseCount = department.Courses?.Count ?? 0
            };

            return Ok(response);
        }

        // GET: api/Departments/{id}/details - Method 2: Using DTO
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDepartmentInfoDTO(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.DeptID == id);

            if (department == null)
                return NotFound();

            var dto = new DepartmentDetailsDTO
            {
                Name = department.Name,
                CourseCount = department.Courses?.Count ?? 0,
                Courses = department.Courses.Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Crs_name = c.Crs_name,
                    Crs_desc = c.Crs_desc,
                    Duration = c.Duration
                }).ToList()
            };

            return Ok(dto);
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Department department)
        {
            if (department == null || string.IsNullOrEmpty(department.Name))
                return BadRequest();

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // PUT: api/Departments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Department department)
        {
            if (id != department.DeptID)
                return BadRequest();

            var existing = await _context.Departments.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = department.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Departments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound();

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

}
