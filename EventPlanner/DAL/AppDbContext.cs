using System.Collections;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Business> Businesses  { get; set; }
    public DbSet<PersonParticipant> PersonParticipants { get; set; }
    public DbSet<BusinessParticipant> BusinessParticipants { get; set; }
    
    public DbSet<PaymentType> PaymentTypes { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public async Task<Event?> GetEventById(int eventId)
    {
        return await Events.FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<List<Person>> GetPersonsNotAtEventById(int eventId)
    {
        var personsAtEventIds = PersonParticipants.Where(pp => pp.EventId == eventId)
            .Select(p => p.PersonId);
        
        return await Persons.Where(p => !personsAtEventIds.Contains(p.Id)).ToListAsync();
    }
    
    public async Task<Event?> GetEventWithAllParticipantsById(int eventId)
    {
        return await Events
            .Include(e => e.PersonParticipants)
            .ThenInclude(pp => pp.Person)
            .Include(e => e.BusinessParticipants)
            .ThenInclude(bp => bp.Business)
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task EditPersonParticipant(PersonParticipant personParticipant)
    {
        Attach(personParticipant).State = EntityState.Modified;
        await SaveChangesAsync();
    }
    
    public async Task EditPerson(Person person)
    {
        Attach(person).State = EntityState.Modified;
        await SaveChangesAsync();
    }
    
    public async Task EditBusinessParticipant(BusinessParticipant businessParticipant)
    {
        Attach(businessParticipant).State = EntityState.Modified;
        await SaveChangesAsync();
    }
    
    public async Task EditBusiness(Business business)
    {
        Attach(business).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task<PersonParticipant?> GetPersonParticipantWithPersonAndEventById(int personParticipantId)
    {
        return await PersonParticipants.Include(pp => pp.Person)
            .Include(pp => pp.Event)
            .FirstOrDefaultAsync(pp => pp.Id == personParticipantId);
    }

    public async Task<BusinessParticipant?> GetBusinessParticipantWithBusinessAndEventById(int businessParticipantId)
    {
        return await BusinessParticipants.Include(bp => bp.Business)
            .Include(bp => bp.Event)
            .FirstOrDefaultAsync(bp => bp.Id == businessParticipantId);
    }
}