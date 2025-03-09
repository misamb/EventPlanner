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

public class PersonParticipantCreatePageTests
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
        pageModel.ModelState.AddModelError("TestError", "This field is required.");

        // Act
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_CreatesOnlyPersonParticipant_WhenIsSavedIsTrue()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);
        
        var newPersonFormData = new Person()
        {
            FirstName = "John",
            LastName = "Doe",
            PersonalCode = "34501234215"
        };

        var savedPersonFormData = new PersonParticipant()
        {
            EventId = 1,
            PersonId = 99, //Saved person id
            PaymentTypeId = 1,
            AdditionalInfo = "Test Additional Info",
        };
        
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
        var pageModel = new CreateModel(appDbContext)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };
        
        pageModel.Person = newPersonFormData;
        pageModel.PersonParticipant = savedPersonFormData;
        pageModel.IsSavedPerson = true;
        
        
        //Act
        await pageModel.OnPostAsync();
        
        //Assert
        Assert.Empty(appDbContext.Persons); // didnt add person

        //Check if savedbusinessforms fields changed 
        var createdPersonParticipant = appDbContext.PersonParticipants.First();
        
        Assert.Equal(createdPersonParticipant.PersonId, savedPersonFormData.PersonId);
        Assert.Equal(createdPersonParticipant.EventId, savedPersonFormData.EventId);
        Assert.Equal(createdPersonParticipant.ParticipantCount, savedPersonFormData.ParticipantCount);
        Assert.Equal(createdPersonParticipant.PaymentTypeId, savedPersonFormData.PaymentTypeId);
        Assert.Equal(createdPersonParticipant.AdditionalInfo, savedPersonFormData.AdditionalInfo);


    }
    
    [Fact]
    public async Task OnPostAsync_CreatesPersonParticipantAndPerson_WhenIsSavedIsFalse()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);
        
        var newPersonFormData = new Person()
        {
            FirstName = "John",
            LastName = "Doe",
            PersonalCode = "34501234215"
        };

        var savedPersonFormData = new PersonParticipant()
        {
            EventId = 1,
            PersonId = 99, //Saved person id
            PaymentTypeId = 1,
            AdditionalInfo = "Test Additional Info",
            ParticipantCount = 1
        };
        
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
        var pageModel = new CreateModel(appDbContext)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };
        
        pageModel.Person = newPersonFormData;
        pageModel.PersonParticipant = savedPersonFormData;
        pageModel.IsSavedPerson = false;
        
        
        //Act
        await pageModel.OnPostAsync();
        
        //Assert
        Assert.NotEmpty(appDbContext.Persons); // Added new Person
        var createdPerson = appDbContext.Persons.First();
        
        Assert.Equal(createdPerson.FirstName, newPersonFormData.FirstName);
        Assert.Equal(createdPerson.LastName, newPersonFormData.LastName);
        Assert.Equal(createdPerson.PersonalCode, newPersonFormData.PersonalCode);

        //Check created PersonParticipant 
        var createdPersonParticipant = appDbContext.PersonParticipants.First();
        
        
        Assert.Equal(createdPersonParticipant.PersonId, createdPerson.Id);
        Assert.Equal(createdPersonParticipant.EventId, savedPersonFormData.EventId);
        Assert.Equal(createdPersonParticipant.ParticipantCount, savedPersonFormData.ParticipantCount);
        Assert.Equal(createdPersonParticipant.PaymentTypeId, savedPersonFormData.PaymentTypeId);
        Assert.Equal(createdPersonParticipant.AdditionalInfo, savedPersonFormData.AdditionalInfo);


    }
}