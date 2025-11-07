using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.ClassConfige
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> entity)
        {
            // 🔹 المفتاح الأساسي
            entity.HasKey(e => e.Id);

            // 🔹 عنوان المهمة (إجباري + أقصى طول)
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            // 🔹 الوصف (اختياري + أقصى طول)
            entity.Property(e => e.Description)
                .HasMaxLength(500);

            // 🔹 تاريخ الإنشاء (قيمة افتراضية)
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
