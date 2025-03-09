using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Domain;
using WebApp.Pages_Business;

namespace Tests.UnitTests;

public class BusinessDeletePageTests
{
    [Fact]
    public async Task OnGetAsync_PopulatesThePageModel_WithBusiness()
    {
        
        //Assert
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        var expectedBusinessId = 1;
        
        var expectedBusiness = new Business()
        {
            Id = expectedBusinessId,
            BusinessName = "Test Business",
            RegistryCode = "12345678"
        };
        
        mockAppDbContext.Setup(
            db => db.GetBusinessById(expectedBusinessId)).Returns(Task.FromResult(expectedBusiness));
        
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        //Act
        await pageModel.OnGetAsync(expectedBusinessId);
        
        //Assert
        
        var actualBusiness = Assert.IsAssignableFrom<Business>(pageModel.Business);
        Assert.Equal(expectedBusiness, actualBusiness);
    }
    
    
    
    
    [Fact]
    public async Task OnPostAsync_ReturnsARedirectToPageResult()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        pageModel.Business = new Business();
        var recId = 1;

        // Act
        var result = await pageModel.OnPostAsync(recId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_DeletesBusiness()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);

        var testBusiness = new Business()
        {
            Id = 1,
            BusinessName = "Test Business",
            RegistryCode = "12345678"
        };
        
        var participation = new BusinessParticipant()
        {
            Id = 1,
            BusinessId = 1,
            EventId = 1,
            ParticipantCount = 1,
        };
        
        await appDbContext.BusinessParticipants.AddAsync(participation);
        await appDbContext.Businesses.AddAsync(testBusiness);
        await appDbContext.SaveChangesAsync();
        
        var pageModel = new DeleteModel(appDbContext);
        pageModel.Business = testBusiness;
        
        //Act
        var result = await pageModel.OnPostAsync(testBusiness.Id);
        
        //Assert
        Assert.Empty(appDbContext.Businesses);
        //Check cascade
        Assert.Empty(appDbContext.BusinessParticipants);

    }
}