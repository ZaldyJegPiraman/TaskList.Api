using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using TaskList.Api.Models;

namespace TaskList.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();


}
