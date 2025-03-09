using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Moq;
using WebApp.Domain;
using WebApp.Pages_BsnParticipant;

namespace Tests.UnitTests;

public class BusinessParticipantEditPageTest
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
            BusinessName = "Test Business",
            RegistryCode = "12345678"
        };

        var expextedEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };

        var expectedBusinessParticipantId = 1;
        var expectedBusinessParticipant = new BusinessParticipant()
        {
            Id = expectedBusinessParticipantId,
            EventId = expextedEvent.Id,
            Event = expextedEvent,
            BusinessId = expectedBusiness.Id,
            Business = expectedBusiness,
            ParticipantCount = 2,
            PaymentTypeId = 1
        };
        
        mockAppDbContext.Setup(
            db => db.GetBusinessParticipantWithBusinessAndEventById(expectedBusinessParticipantId))
            .Returns(Task.FromResult(expectedBusinessParticipant));
        
        var pageModel = new EditModel(mockAppDbContext.Object);
        
        //Act
        await pageModel.OnGetAsync(expectedBusinessParticipantId);
        
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
        

        var expectedBusinessParticipantId = 1;
        var expectedBusinessParticipant = new BusinessParticipant()
        {
            Id = expectedBusinessParticipantId,
            EventId = 1,
            Event = null,
            BusinessId = 1,
            Business = null,
            ParticipantCount = 2,
            PaymentTypeId = 1
        };
        
        mockAppDbContext.Setup(
                db => db.GetBusinessParticipantWithBusinessAndEventById(expectedBusinessParticipantId))
            .Returns(Task.FromResult(expectedBusinessParticipant));
        
        var pageModel = new EditModel(mockAppDbContext.Object);
        
        //Act
        var result = await pageModel.OnGetAsync(expectedBusinessParticipantId);
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
        
    }
    
    [Fact]
    public async Task OnPostAsync_ReturnsARedirectToPageResult_WhenModelStateIsValid()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        var expectedBusiness = new Business()
        {
            Id = 1,
            BusinessName = "Test Business",
            RegistryCode = "12345678"
        };
        
        var expectedBusinessParticipantId = 1;
        var expectedBusinessParticipant = new BusinessParticipant()
        {
            Id = expectedBusinessParticipantId,
            EventId = 1,
            BusinessId = expectedBusiness.Id,
            ParticipantCount = 2,
            PaymentTypeId = 1
        };
        
        mockAppDbContext.Setup(db => db.EditBusiness(expectedBusiness)).Returns(Task.FromResult(expectedBusiness));
        mockAppDbContext.Setup(db => db.EditBusinessParticipant(expectedBusinessParticipant))
            .Returns(Task.FromResult(expectedBusinessParticipant));
        var httpContext = new DefaultHttpContext();
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var pageContext = new PageContext(actionContext)
        {
            ViewData = viewData
        };
        var pageModel = new EditModel(mockAppDbContext.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        // Act
        // A new ModelStateDictionary is valid by default.
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }
    
    [Fact]
    public async Task OnPostAsync_ReturnsAPageResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        var httpContext = new DefaultHttpContext();
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var pageContext = new PageContext(actionContext)
        {
            ViewData = viewData
        };
        var pageModel = new EditModel(mockAppDbContext.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };
        pageModel.ModelState.AddModelError("TestError", "This field is required.");

        // Act
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_UpdatesBusinessAndBusinessParticipant()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);
        
        var savedBusiness = new Business()
        {
            Id = 1,
            BusinessName = "Test Business",
            RegistryCode = "12345678"
        };

        var savedBusinessParticipant = new BusinessParticipant()
        {
            Id = 1,
            EventId = 1,
            BusinessId = 1,
            ParticipantCount = 2,
            PaymentTypeId = 1
        };
        
        await appDbContext.Businesses.AddAsync(savedBusiness);
        await appDbContext.BusinessParticipants.AddAsync(savedBusinessParticipant);
        await appDbContext.SaveChangesAsync();
        
        var httpContext = new DefaultHttpContext();
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var pageContext = new PageContext(actionContext)
        {
            ViewData = viewData
        };
        var pageModel = new EditModel(appDbContext)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        pageModel.Business = savedBusiness;
        pageModel.BusinessParticipant = savedBusinessParticipant;
        
        //Act
        var updatedBusinessName = "New Business Name";
        var updatedParticipantCount = 51;
        
        pageModel.Business.BusinessName = updatedBusinessName;
        pageModel.BusinessParticipant.ParticipantCount = updatedParticipantCount;
        
        await pageModel.OnPostAsync();
        
        //Assert
        var updatedBusiness = appDbContext.Businesses.FirstOrDefault(b => b.Id == savedBusiness.Id);
        var updatedBusinessParticipant = appDbContext.BusinessParticipants.FirstOrDefault(bp => bp.Id == savedBusinessParticipant.Id);
        
        Assert.Equal(updatedBusinessName, updatedBusiness!.BusinessName);
        Assert.Equal(updatedParticipantCount, updatedBusinessParticipant!.ParticipantCount);
        
        
        

    }
}