using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Domain;
using WebApp.Pages_PrsnParticipant;

namespace Tests.UnitTests;

public class PersonParticipantDeletePage
{
    [Fact]
    public async Task OnGetAsync_PopulatesThePageModel_WithPersonParticipant()
    {
        
        //Assert
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        
        var expectedPerson = new Person()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            PersonalCode = "34501234215"
        };

        var expextedEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };
        
        var expectedPersonParticipant = new PersonParticipant()
        {
            Id = 1,
            EventId = expextedEvent.Id,
            Event = expextedEvent,
            PersonId = expectedPerson.Id,
            Person = expectedPerson,
            ParticipantCount = 1,
            PaymentTypeId = 1
        };
        
        mockAppDbContext.Setup(
                db => db.GetPersonParticipantWithPersonAndEventById(expectedPersonParticipant.Id))
            .Returns(Task.FromResult(expectedPersonParticipant));
        
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        //Act
        await pageModel.OnGetAsync(expectedPersonParticipant.Id);
        
        //Assert
        
        var actualPersonParticipant = Assert.IsAssignableFrom<PersonParticipant>(pageModel.PersonParticipant);
        var actualPerson = Assert.IsAssignableFrom<Person>(pageModel.Person);
        var actualEvent = Assert.IsAssignableFrom<Event>(pageModel.Event);
        Assert.Equal(expectedPersonParticipant, actualPersonParticipant);
        Assert.Equal(expectedPerson, actualPerson);
        Assert.Equal(expextedEvent, actualEvent);
    }
    
    [Fact]
    public async Task OnGetAsync_ReturnsNotFound_WithFaultyPersonParticipant()
    {
        
        //Assert
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        
        var expectedPersonParticipant = new PersonParticipant()
        {
            Id = 1,
            EventId = 1,
            Event = null,
            PersonId = 1,
            Person = null,
            ParticipantCount = 2
            
        };
        
        mockAppDbContext.Setup(
                db => db.GetPersonParticipantWithPersonAndEventById(expectedPersonParticipant.Id))
            .Returns(Task.FromResult(expectedPersonParticipant));
        
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        //Act
        var result = await pageModel.OnGetAsync(expectedPersonParticipant.Id);
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
        
    }
    
    [Fact]
    public async Task OnPostAsync_ReturnsARedirectToPageResult()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        pageModel.PersonParticipant = new PersonParticipant();
        var recId = 1;

        // Act
        var result = await pageModel.OnPostAsync(recId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }
    
    [Fact]
    public async Task OnPostAsync_DeletesPersonParticipant()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);

        var testPerson = new Person()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            PersonalCode = "34501234215"
        };

        var testEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };

        var participation = new PersonParticipant()
        {
            Id = 1,
            PersonId = 1,
            EventId = 1,
            ParticipantCount = 1,
        };
        
        await appDbContext.PersonParticipants.AddAsync(participation);
        await appDbContext.Persons.AddAsync(testPerson);
        await appDbContext.Events.AddAsync(testEvent);
        await appDbContext.SaveChangesAsync();
        
        var pageModel = new DeleteModel(appDbContext);
        
        pageModel.PersonParticipant = participation;
        
        //Act
        var result = await pageModel.OnPostAsync(participation.Id);
        
        //Assert
        Assert.Empty(appDbContext.PersonParticipants);
        //Check cascade
        Assert.NotEmpty(appDbContext.Events);
        Assert.NotEmpty(appDbContext.Persons);

    }
}