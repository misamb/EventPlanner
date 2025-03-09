using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Domain;
using WebApp.Pages_BsnParticipant;

namespace Tests.UnitTests;

public class BusinessParticipantDeletePageTests
{
        [Fact]
    public async Task OnGetAsync_PopulatesThePageModel_WithBusinessParticipant()
    {
        
        //Assert
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        
        var expectedBusiness = new Business()
        {
            Id = 1,
            BusinessName = "Test Event",
            RegistryCode = "12345678"
        };

        var expextedEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };
        
        var expectedBusinessParticipant = new BusinessParticipant()
        {
            Id = 1,
            EventId = expextedEvent.Id,
            Event = expextedEvent,
            BusinessId  = expectedBusiness.Id,
            Business = expectedBusiness,
            ParticipantCount = 20,
            PaymentTypeId = 1
        };
        
        mockAppDbContext.Setup(
                db => db.GetBusinessParticipantWithBusinessAndEventById(expectedBusinessParticipant.Id))
            .Returns(Task.FromResult(expectedBusinessParticipant));
        
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        //Act
        await pageModel.OnGetAsync(expectedBusinessParticipant.Id);
        
        //Assert
        
        var actualBusinessParticipant = Assert.IsAssignableFrom<BusinessParticipant>(pageModel.BusinessParticipant);
        var actualBusiness = Assert.IsAssignableFrom<Business>(pageModel.Business);
        var actualEvent = Assert.IsAssignableFrom<Event>(pageModel.Event);
        Assert.Equal(expectedBusinessParticipant, actualBusinessParticipant);
        Assert.Equal(expectedBusiness, actualBusiness);
        Assert.Equal(expextedEvent, actualEvent);
    }
    
    [Fact]
    public async Task OnGetAsync_ReturnsNotFound_WithFaultyBusinessParticipant()
    {
        
        //Assert
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        
        var expectedBusinessParticipant = new BusinessParticipant()
        {
            Id = 1,
            EventId = 1,
            Event = null,
            BusinessId  = 1,
            Business = null,
            ParticipantCount = 2
            
        };
        
        mockAppDbContext.Setup(
                db => db.GetBusinessParticipantWithBusinessAndEventById(expectedBusinessParticipant.Id))
            .Returns(Task.FromResult(expectedBusinessParticipant));
        
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        //Act
        var result = await pageModel.OnGetAsync(expectedBusinessParticipant.Id);
        
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
        pageModel.BusinessParticipant = new BusinessParticipant();
        var recId = 1;

        // Act
        var result = await pageModel.OnPostAsync(recId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }
    
    [Fact]
    public async Task OnPostAsync_DeletesBusinessParticipant()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);

        var testBusiness = new Business()
        {
            Id = 1,
            BusinessName = "Test Event",
            RegistryCode = "12345678"
        };

        var testEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };
        
        var participation = new BusinessParticipant()
        {
            Id = 1,
            EventId = testEvent.Id,
            Event = testEvent,
            BusinessId  = testBusiness.Id,
            Business = testBusiness,
            ParticipantCount = 20,
            PaymentTypeId = 1
        };
        
        await appDbContext.BusinessParticipants.AddAsync(participation);
        await appDbContext.Businesses.AddAsync(testBusiness);
        await appDbContext.Events.AddAsync(testEvent);
        await appDbContext.SaveChangesAsync();
        
        var pageModel = new DeleteModel(appDbContext);
        
        pageModel.BusinessParticipant = participation;
        
        //Act
        var result = await pageModel.OnPostAsync(participation.Id);
        
        //Assert
        Assert.Empty(appDbContext.BusinessParticipants);
        //Check cascade
        Assert.NotEmpty(appDbContext.Events);
        Assert.NotEmpty(appDbContext.Businesses);

    }
}