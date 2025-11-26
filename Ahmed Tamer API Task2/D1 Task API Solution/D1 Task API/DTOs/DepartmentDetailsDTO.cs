namespace D1_Task_API.DTOs
{
   public class DepartmentDetailsDTO
{
    public string Name { get; set; }
    public int CourseCount { get; set; }
    public List<CourseDTO> Courses { get; set; }
}
}
