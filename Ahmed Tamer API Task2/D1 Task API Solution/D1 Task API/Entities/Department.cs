using System.ComponentModel.DataAnnotations;

namespace D1_Task_API.Entities
{
    public class Department
    {
        [Key]
        public int DeptID { get; set; }
        public string? Name { get; set; }
        public ICollection<Course>? Courses { get; set; } = new List<Course>();
    }
}
