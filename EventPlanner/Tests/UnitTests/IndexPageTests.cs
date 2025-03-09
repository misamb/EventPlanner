using DAL;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Domain;
using WebApp.Pages;

namespace Tests.UnitTests;

public class IndexPageTests
{
    [Fact]
    public async Task OnGetAsync_PopulatesThePageModel_WithEvents()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("InMemoryDb");
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);

        var expectedFutureEvents = new List<Event>()
        {
            new Event()
            {
                Id = 1,
                EventName = "Future Event 1",
                EventLocation = "London",
                EventStartTime = DateTime.Now.AddDays(1)
            },
            new Event()
            {
                Id = 2,
                EventName = "Future Event 2",
                EventLocation = "Tallinn",
                EventStartTime = DateTime.Now.AddDays(2)
            }
        };

        var expectedPastEvents = new List<Event>()
        {
            new Event()
            {
                Id = 3,
                EventName = "Past Event 1",
                EventLocation = "London",
                EventStartTime = DateTime.Now.AddDays(-1)
            },
            new Event()
            {
                Id = 4,
                EventName = "Past Event 2",
                EventLocation = "Tallinn",
                EventStartTime = DateTime.Now.AddDays(-2)
            }
        };
        
        mockAppDbContext.Setup(
            db => db.GetAllEventsWithParticipants()).Returns(Task.FromResult(
            new List<Event>(expectedFutureEvents.Concat(expectedPastEvents))));
        
        var pageModel = new IndexModel(mockAppDbContext.Object);
        
        // Act
        await pageModel.OnGetAsync();
        
        // Assert
        var actualFutureEvents = Assert.IsAssignableFrom<List<Event>>(pageModel.FutureEvents);
        Assert.Equal(expectedFutureEvents, actualFutureEvents);
        var actualPastEvents = Assert.IsAssignableFrom<List<Event>>(pageModel.PastEvents);
        Assert.Equal(expectedPastEvents, actualPastEvents);
    }

    [Fact]
    public Task TestMethod_GetParticipantCount()
    {
        //Arrange 
        var personParticipants = new List<PersonParticipant>()
        {
            new PersonParticipant()
            {
                Id = 1,
                EventId = 1,
                PersonId = 1,
                ParticipantCount = 1
            },
            new PersonParticipant()
            {
                Id = 2,
                EventId = 1,
                PersonId = 2,
                ParticipantCount = 1
            }
        };

        var businessParticipants = new List<BusinessParticipant>()
        {
            new BusinessParticipant()
            {
                Id = 1,
                EventId = 1,
                BusinessId = 1,
                ParticipantCount = 21
            },
            new BusinessParticipant()
            {
                Id = 2,
                EventId = 1,
                BusinessId = 2,
                ParticipantCount = 5
            }
        };

        var testEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event 1",
            EventLocation = "London",
            EventStartTime = DateTime.Now.AddDays(1),
            PersonParticipants = personParticipants,
            BusinessParticipants = businessParticipants
        };
        
        //Act
        var participantCount = IndexModel.GetParticipantCount(testEvent);
        
        //Assert
        Assert.Equal(28, participantCount);
        return Task.CompletedTask;
    }
}