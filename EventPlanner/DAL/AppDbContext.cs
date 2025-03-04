﻿using Microsoft.EntityFrameworkCore;
using WebApp.Domain;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Business> Businesses  { get; set; }
    public DbSet<PersonParticipant> PersonParticipants { get; set; }
    public DbSet<BusinessParticipant> BusinessParticipants { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}