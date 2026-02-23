using System;
using Task.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Task.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<TaskItem> Tasks { get; set; }
}
