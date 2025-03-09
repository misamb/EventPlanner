using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Domain;
using WebApp.Pages_Events;

namespace Tests.UnitTests;

public class EventDeletePageTests
{
    [Fact]
    public async Task OnGetAsync_PopulatesThePageModel_WithEvent()
    {
        
        //Assert
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);

        var expextedEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };
        
        
        mockAppDbContext.Setup(
                db => db.GetEventById(expextedEvent.Id))
            .Returns(Task.FromResult(expextedEvent));
        
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        //Act
        await pageModel.OnGetAsync(expextedEvent.Id);
        
        //Assert
        
        var actualEvent = Assert.IsAssignableFrom<Event>(pageModel.Event);
        Assert.Equal(expextedEvent, actualEvent);
    }
    
    [Fact]
    public async Task OnPostAsync_ReturnsARedirectToPageResult()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        pageModel.Event = new Event();
        var recId = 1;

        // Act
        var result = await pageModel.OnPostAsync(recId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }
    
    [Fact]
    public async Task OnPostAsync_DeletesEvent()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);
        
        var testEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };
        
        var personParticipant = new PersonParticipant()
        {
            Id = 1,
            PersonId = 1,
            EventId = 1,
            ParticipantCount = 1,
        };

        var businessParticipant = new BusinessParticipant()
        {
            Id = 1,
            BusinessId = 1,
            EventId = 1,
            ParticipantCount = 20,
        };
        
        await appDbContext.Events.AddAsync(testEvent);
        await appDbContext.PersonParticipants.AddAsync(personParticipant);
        await appDbContext.BusinessParticipants.AddAsync(businessParticipant);
        await appDbContext.SaveChangesAsync();
        
        var pageModel = new DeleteModel(appDbContext);
        pageModel.Event = testEvent;
        
        //Act
        await pageModel.OnPostAsync(testEvent.Id);
        
        //Assert
        Assert.Empty(appDbContext.Events);
        //Check cascade
        Assert.Empty(appDbContext.PersonParticipants);
        Assert.Empty(appDbContext.BusinessParticipants);

    }
}