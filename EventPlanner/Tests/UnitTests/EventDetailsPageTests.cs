using DAL;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Domain;
using WebApp.Pages_Events;

namespace Tests.UnitTests;

public class EventDetailsPageTests
{
    [Fact]
    public async Task OnGetAsync_PopulatesThePageModel_WithEventAndParticipants()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("InMemoryDb");
        
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);

        var expectedPersons = new List<Person>()
        {
            new Person()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "34501234215"
            },

            new Person()
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                PersonalCode = "49403136515"
            }
        };

        var expectedPersonParticipants = new List<PersonParticipant>()
        {
            new PersonParticipant()
            {
                Id = 1,
                EventId = 1,
                PersonId = 1,
                Person = expectedPersons[0],
                ParticipantCount = 1
            },
            new PersonParticipant()
            {
                Id = 2,
                EventId = 1,
                PersonId = 2,
                Person = expectedPersons[1],
                ParticipantCount = 1
            }
        };

        var expectedBusinesses = new List<Business>()
        {
            new Business()
            {
                Id = 1,
                BusinessName = "Test Business 1",
                RegistryCode = "12345678"
            },
            new Business()
            {
                Id = 2,
                BusinessName = "Test Business 2",
                RegistryCode = "02345678"
            }
        };

        var expectedBusinessParticipants = new List<BusinessParticipant>()
        {
            new BusinessParticipant()
            {
                Id = 1,
                EventId = 1,
                BusinessId = 1,
                Business = expectedBusinesses[0],
                ParticipantCount = 10
            },
            new BusinessParticipant()
            {
                Id = 2,
                EventId = 1,
                BusinessId = 2,
                Business = expectedBusinesses[1],
                ParticipantCount = 20
            }
        };

        var expectedEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
            BusinessParticipants = expectedBusinessParticipants,
            PersonParticipants = expectedPersonParticipants
        };
        
        mockAppDbContext.Setup(
            db => db.GetEventWithAllParticipantsById(expectedEvent.Id)).Returns(Task.FromResult(expectedEvent));

        var pageModel = new DetailsModel(mockAppDbContext.Object);
        
        //Act
        await pageModel.OnGetAsync(expectedEvent.Id);
        
        //Assert
        var actualPersons = Assert.IsAssignableFrom<List<Person>>(pageModel.Event!.PersonParticipants.Select(pp => pp.Person).ToList());
        Assert.Equal(actualPersons, expectedPersons);
        var actualBusinesses = Assert.IsAssignableFrom<List<Business>>(pageModel.Event!.BusinessParticipants.Select(bp => bp.Business).ToList());
        Assert.Equal(actualBusinesses, expectedBusinesses);
        
    }
    
}