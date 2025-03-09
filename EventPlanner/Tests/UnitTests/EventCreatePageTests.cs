using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Pages_Events;

namespace Tests.UnitTests;

public class EventCreatePageTests
{
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
        var pageModel = new CreateModel(mockAppDbContext.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };
        
        pageModel.Event = new Event()
        {
            EventName = "TestEvent",
            EventStartTime = DateTime.Now.AddMinutes(-5),
            EventLocation = "TestLocation",
        };
        

        // Act
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(pageModel.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_ReturnsARedirectToPageResult_WhenModelStateIsValid()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        
        var expectedEvent = new Event()
        {
            Id = 1,
            EventName = "TestEvent",
            EventStartTime = DateTime.Now.AddMinutes(10),
            EventLocation = "TestLocation",
        };


        mockAppDbContext.Setup(db => db.CreateEvent(expectedEvent)).Returns(Task.FromResult(expectedEvent.Id));

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
        var pageModel = new CreateModel(mockAppDbContext.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        pageModel.Event = expectedEvent;

        //Act
        await pageModel.OnPostAsync();

        //Assert
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }
    
    [Fact]
    public async Task OnPostAsync_CreatesEvent()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);
        
        var testEvent = new Event()
        {
            Id = 1,
            EventName = "TestEvent",
            EventStartTime = DateTime.Now.AddMinutes(10),
            EventLocation = "TestLocation",
        };
        
        await appDbContext.Events.AddAsync(testEvent);
        await appDbContext.SaveChangesAsync();
        
        var pageModel = new CreateModel(appDbContext);
        pageModel.Event = testEvent;
        
        //Act
        var result = await pageModel.OnPostAsync();
        
        //Assert
        var createdEvent = appDbContext.Events.Find(testEvent.Id);
        Assert.Equal(testEvent.Id, createdEvent!.Id);
        Assert.Equal(testEvent.EventName, createdEvent.EventName);
        Assert.Equal(testEvent.EventLocation, createdEvent.EventLocation);
        Assert.Equal(testEvent.EventStartTime, createdEvent.EventStartTime);

    }
}