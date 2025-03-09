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
using WebApp.Domain;
using WebApp.Pages_PrsnParticipant;

namespace Tests.UnitTests;

public class PersonParticipantEditPageTest
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
        
        var pageModel = new EditModel(mockAppDbContext.Object);
        
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
        
        var pageModel = new EditModel(mockAppDbContext.Object);
        
        //Act
        var result = await pageModel.OnGetAsync(expectedPersonParticipant.Id);
        
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
        var expectedPerson = new Person()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            PersonalCode = "34501234215"
        };
        
        var expectedPersonParticipant = new PersonParticipant()
        {
            Id = 1,
            EventId = 1,
            PersonId = expectedPerson.Id,
            ParticipantCount = 2
        };
        
        mockAppDbContext.Setup(db => db.EditPerson(expectedPerson)).Returns(Task.FromResult(expectedPerson));
        mockAppDbContext.Setup(db => db.EditPersonParticipant(expectedPersonParticipant))
            .Returns(Task.FromResult(expectedPersonParticipant));
        
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
    public async Task OnPostAsync_UpdatesPersonAndPersonParticipant()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);
        
        var savedPerson = new Person()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            PersonalCode = "34501234215"
        };

        var savedPersonParticipant = new PersonParticipant()
        {
            Id = 1,
            EventId = 1,
            PersonId = 1,
            ParticipantCount = 1,
            PaymentTypeId = 1
        };
        
        await appDbContext.Persons.AddAsync(savedPerson);
        await appDbContext.PersonParticipants.AddAsync(savedPersonParticipant);
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

        pageModel.Person = savedPerson;
        pageModel.PersonParticipant = savedPersonParticipant;
        
        //Act
        var updatedPersonFirstName = "New Name";
        var updatedAdditionalInfo = "New Info";
        
        pageModel.Person.FirstName = updatedPersonFirstName;
        pageModel.PersonParticipant.AdditionalInfo = updatedAdditionalInfo;
        
        await pageModel.OnPostAsync();
        
        //Assert
        var updatedPerson = appDbContext.Persons.FirstOrDefault(p => p.Id == savedPerson.Id);
        var updatedPersonParticipant = appDbContext.PersonParticipants.FirstOrDefault(pp => pp.Id == savedPersonParticipant.Id);
        
        Assert.Equal(updatedPersonFirstName, updatedPerson!.FirstName);
        Assert.Equal(updatedAdditionalInfo, updatedPersonParticipant!.AdditionalInfo);
        
        
        

    }
}