using System.ComponentModel.DataAnnotations.Schema;

namespace D1_Task_API.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string? Crs_name { get; set; }
        public string? Crs_desc { get; set; }
        public int Duration { get; set; }

        [ForeignKey("Department")]
        public int DeptID { get; set; }
        public Department? Department { get; set; }
    }
}
