using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Domain;

namespace Tests;

public static class Utilities
{
    public static DbContextOptions<AppDbContext> TestDbContextOptions()
    {
        // Create a new service provider to create a new in-memory database.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance using an in-memory database and 
        // IServiceProvider that the context should resolve all of its 
        // services from.
        var builder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    public static List<Person> seedPersons = new List<Person>()
    {
        new Person()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            PersonalCode = "49403136515",
        },
        new Person()
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Doe",
            PersonalCode = "49403136516",
        }
    };

    public static List<Business> seedBusinesses = new List<Business>()
    {
        new Business()
        {
            Id = 1,
            BusinessName = "Good Business",
            RegistryCode = "12345678"
        },
        new Business()
        {
            Id = 2,
            BusinessName = "Seed Business",
            RegistryCode = "12345578"
        }
    };

    public static List<Event> seedEvents = new List<Event>
    {
        new Event()
        {
            Id = 1,
            EventName = "Good Event",
            EventLocation = "London",
            EventStartTime = DateTime.Today.AddMonths(1)
        },
        new Event()
        {
            Id = 2,
            EventName = "Seed Event",
            EventLocation = "Tallinn",
            EventStartTime = DateTime.Today.AddMonths(2)
        }
    };

    public static List<PaymentType> seedPaymentTypes = new List<PaymentType>()
    {
        new PaymentType()
        {
            Id = 1,
            TypeName = "Sularaha"
        },
        new PaymentType()
        {
            Id = 2,
            TypeName = "Pangamakse"
        }
    };

    public static List<PersonParticipant> seedPersonParticipants = new List<PersonParticipant>()
    {
        new PersonParticipant()
        {
            Id = 1,
            PersonId = 1,
            EventId = 1,
            ParticipantCount = 1,
            PaymentTypeId = 1,
            AdditionalInfo = "John Doe, Good Event, Sularaha"
        }
    };

    public static List<BusinessParticipant> seedBusinessParticipant = new List<BusinessParticipant>()
    {
        new BusinessParticipant()
        {
            Id = 1,
            BusinessId = 1,
            EventId = 1,
            ParticipantCount = 20,
            PaymentTypeId = 2,
            AdditionalInfo = "Good Business, Good Event, Pangamakse"
        }
    };
    
    public static void PopulateDatabase(AppDbContext db)
    {
        db.Persons.AddRange(seedPersons);
        db.Businesses.AddRange(seedBusinesses);
        db.Events.AddRange(seedEvents);
        db.PaymentTypes.AddRange(seedPaymentTypes);
        db.PersonParticipants.AddRange(seedPersonParticipants);
        db.BusinessParticipants.AddRange(seedBusinessParticipant);
        db.SaveChanges();

    }

}