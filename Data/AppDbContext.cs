using Microsoft.EntityFrameworkCore;
using minimal_api_crud.Models;

namespace minimal_api_crud.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
}
