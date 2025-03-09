using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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
    
    public virtual async Task<PersonParticipant?> GetPersonParticipantWithPersonAndEventById(int personParticipantId)
    {
        return await PersonParticipants.Include(pp => pp.Person)
            .Include(pp => pp.Event)
            .FirstOrDefaultAsync(pp => pp.Id == personParticipantId);
    }

    public virtual async Task<BusinessParticipant?> GetBusinessParticipantWithBusinessAndEventById(int businessParticipantId)
    {
        return await BusinessParticipants.Include(bp => bp.Business)
            .Include(bp => bp.Event)
            .FirstOrDefaultAsync(bp => bp.Id == businessParticipantId);
    }

    public virtual async Task<List<Person>> GetPersonsNotAtEventById(int eventId)
    {
        var personsAtEventIds = PersonParticipants.Where(pp => pp.EventId == eventId)
            .Select(p => p.PersonId);
        
        return await Persons.Where(p => !personsAtEventIds.Contains(p.Id)).ToListAsync();
    }

    public virtual async Task<List<Business>> GetBusinessesNotAtEventById(int eventId)
    {
        var businessesAtEventIds = BusinessParticipants.Where(pb => pb.EventId == eventId)
            .Select(p => p.BusinessId);
        
        return await Businesses.Where(p => !businessesAtEventIds.Contains(p.Id)).ToListAsync();
    }
    
    public virtual async Task<Event?> GetEventWithAllParticipantsById(int eventId)
    {
        return await Events
            .Include(e => e.PersonParticipants)
            .ThenInclude(pp => pp.Person)
            .Include(e => e.BusinessParticipants)
            .ThenInclude(bp => bp.Business)
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }
    
    public virtual async Task<Event?> GetEventById(int eventId)
    {
        return await Events.FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public virtual async Task<Person?> GetPersonById(int personId)
    {
        return await Persons.FirstOrDefaultAsync(p => p.Id == personId);
    }

    public virtual async Task<Business?> GetBusinessById(int businessId)
    {
        return await Businesses.FirstOrDefaultAsync(b => b.Id == businessId);
    }

    public virtual async Task<int> CreateEvent(Event newEvent)
    {
        await Events.AddAsync(newEvent);
        await SaveChangesAsync();
        return newEvent.Id;
    }

    public virtual async Task<int> CreatePerson(Person newPerson)
    {
        await Persons.AddAsync(newPerson);
        await SaveChangesAsync();
        return newPerson.Id;
    }

    public virtual async Task<int> CreateBusiness(Business newBusiness)
    {
        await Businesses.AddAsync(newBusiness);
        await SaveChangesAsync();
        return newBusiness.Id;
    }

    public virtual async Task<int> CreatePersonParticipant(PersonParticipant newPersonParticipant)
    {
        await PersonParticipants.AddAsync(newPersonParticipant);
        await SaveChangesAsync();
        return newPersonParticipant.Id;
    }

    public virtual async Task<int> CreateBusinessParticipant(BusinessParticipant newBusinessParticipant)
    {
        await BusinessParticipants.AddAsync(newBusinessParticipant);
        await SaveChangesAsync();
        return newBusinessParticipant.Id;
    }

    public virtual async Task EditEvent(Event e)
    {
        Attach(e).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public virtual async Task EditPersonParticipant(PersonParticipant personParticipant)
    {
        Attach(personParticipant).State = EntityState.Modified;
        await SaveChangesAsync();
    }
    
    public virtual async Task EditPerson(Person person)
    {
        Attach(person).State = EntityState.Modified;
        await SaveChangesAsync();
    }
    
    public virtual async Task EditBusinessParticipant(BusinessParticipant businessParticipant)
    {
        Attach(businessParticipant).State = EntityState.Modified;
        await SaveChangesAsync();
    }
    
    public virtual async Task EditBusiness(Business business)
    {
        Attach(business).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public virtual async Task DeleteEvent(int eventId)
    {
        var e = await Events.FindAsync(eventId);
        if (e != null)
        {
            Events.Remove(e);
            await SaveChangesAsync();
        }
    }

    public virtual async Task DeletePerson(int personId)
    {
        var person = await Persons.FindAsync(personId);
        if (person != null)
        {
            Persons.Remove(person);
            await SaveChangesAsync();
        }
    }

    public virtual async Task DeleteBusiness(int businessId)
    {
        var business = await Businesses.FindAsync(businessId);
        if (business != null)
        {
            Businesses.Remove(business);
            await SaveChangesAsync();
        }
    }

    public virtual async Task DeleteBusinessParticipant(int bsnParticipantId)
    {
        var businessparticipant = await BusinessParticipants.FindAsync(bsnParticipantId);
        if (businessparticipant != null)
        {
            BusinessParticipants.Remove(businessparticipant);
            await SaveChangesAsync();
        }
    }

    public virtual async Task DeletePersonParticipant(int prsnParticipantId)
    {
        var personparticipant = await PersonParticipants.FindAsync(prsnParticipantId);
        if (personparticipant != null)
        {
            
            PersonParticipants.Remove(personparticipant);
            await SaveChangesAsync();
        }
    }

    
}