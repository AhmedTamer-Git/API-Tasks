using D1_Task_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace D1_Task_API.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
    }
}
