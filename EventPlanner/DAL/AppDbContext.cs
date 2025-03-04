using Microsoft.EntityFrameworkCore;
using WebApp.Domain;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Event> Events { get; set; } = default!;
    public DbSet<Person> Persons { get; set; } = default!;
    public DbSet<Business> Businesses  { get; set; } = default!;
    public DbSet<PersonParticipant> PersonParticipants { get; set; } = default!;
    public DbSet<BusinessParticipant> BusinessParticipants { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}