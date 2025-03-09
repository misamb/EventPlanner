using DAL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Domain;
using WebApp.Pages_Events;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace Tests.UnitTests;

public class EventEditPageTests
{
    [Fact]
    public async Task OnGetAsync_PopulatesThePageModel_WithEvent()
    {
        
        //Assert
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        
        var expectedEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };

        mockAppDbContext.Setup(
            db => db.GetEventById(expectedEvent.Id))
            .Returns(Task.FromResult(expectedEvent));
        
        var pageModel = new EditModel(mockAppDbContext.Object);
        
        //Act
        await pageModel.OnGetAsync(expectedEvent.Id);
        
        //Assert
        var actualEvent = Assert.IsAssignableFrom<Event>(pageModel.Event);
        Assert.Equal(expectedEvent, actualEvent);
    }

    [Fact]
    public async Task OnPostAsync_ReturnsARedirectToPageResult_WhenModelStateIsValid()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);

        var testEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(1),
            EventLocation = "Test Location",
        };
        
        mockAppDbContext.Setup(
                db => db.GetEventById(testEvent.Id))
            .Returns(Task.FromResult(testEvent));
        
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

        pageModel.Event = testEvent;

        // Act
        // A new ModelStateDictionary is valid by default.
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }
    
    [Fact]
    public async Task OnPostEditEventAsync_ReturnsAPageResult_WhenModelStateIsInvalid()
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
    public async Task OnPostAsync_ReturnsAPageResult_WhenTimeInPast()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);

        var testEvent = new Event()
        {
            Id = 1,
            EventName = "Test Event",
            EventStartTime = DateTime.Now.AddMonths(-1),
            EventLocation = "Test Location",
        };

        mockAppDbContext.Setup(
                db => db.GetEventById(testEvent.Id))
            .Returns(Task.FromResult(testEvent));

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

        pageModel.Event = testEvent;

        // Act
        // A new ModelStateDictionary is valid by default.
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(pageModel.ErrorMessage);
    }
}