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
using WebApp.Pages_BsnParticipant;

namespace Tests.UnitTests;

public class BusinessParticipantCreatePageTest
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
    public async Task OnPostAsync_CreatesOnlyBusinessParticipant_WhenIsSavedIsTrue()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);
        
        var newBusinessFormData = new Business()
        {
            BusinessName = "Test Business",
            RegistryCode = "12345678"
        };

        var savedBusinessFormData = new BusinessParticipant()
        {
            EventId = 1,
            BusinessId = 99, //Saved business id
            ParticipantCount = 3,
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
        
        pageModel.Business = newBusinessFormData;
        pageModel.BusinessParticipant = savedBusinessFormData;
        pageModel.IsSavedBusiness = true;
        
        
        //Act
        await pageModel.OnPostAsync();
        
        //Assert
        Assert.Empty(appDbContext.Businesses); // didnt add business

        //Check if savedbusinessforms fields changed 
        var createdBusinessParticipant = appDbContext.BusinessParticipants.First();
        
        Assert.Equal(createdBusinessParticipant.BusinessId, savedBusinessFormData.BusinessId);
        Assert.Equal(createdBusinessParticipant.EventId, savedBusinessFormData.EventId);
        Assert.Equal(createdBusinessParticipant.ParticipantCount, savedBusinessFormData.ParticipantCount);
        Assert.Equal(createdBusinessParticipant.PaymentTypeId, savedBusinessFormData.PaymentTypeId);
        Assert.Equal(createdBusinessParticipant.AdditionalInfo, savedBusinessFormData.AdditionalInfo);


    }
    
    [Fact]
    public async Task OnPostAsync_CreatesBusinessParticipantAndBusiness_WhenIsSavedIsFalse()
    {
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var appDbContext = new AppDbContext(optionsBuilder.Options);
        
        var newBusinessFormData = new Business()
        {
            BusinessName = "Test Business",
            RegistryCode = "12345678"
        };

        var savedBusinessFormData = new BusinessParticipant()
        {
            EventId = 1,
            BusinessId = 99, //Saved business id
            ParticipantCount = 3,
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
        
        pageModel.Business = newBusinessFormData;
        pageModel.BusinessParticipant = savedBusinessFormData;
        pageModel.IsSavedBusiness = false;
        
        
        //Act
        await pageModel.OnPostAsync();
        
        //Assert
        Assert.NotEmpty(appDbContext.Businesses); // Added new business
        var createdBusiness = appDbContext.Businesses.First();
        
        Assert.Equal(createdBusiness.BusinessName, newBusinessFormData.BusinessName);
        Assert.Equal(createdBusiness.RegistryCode, newBusinessFormData.RegistryCode);

        //Check created BusinessParticipant 
        var createdBusinessParticipant = appDbContext.BusinessParticipants.First();
        
        
        Assert.Equal(createdBusinessParticipant.BusinessId, createdBusiness.Id);
        Assert.Equal(createdBusinessParticipant.EventId, savedBusinessFormData.EventId);
        Assert.Equal(createdBusinessParticipant.ParticipantCount, savedBusinessFormData.ParticipantCount);
        Assert.Equal(createdBusinessParticipant.PaymentTypeId, savedBusinessFormData.PaymentTypeId);
        Assert.Equal(createdBusinessParticipant.AdditionalInfo, savedBusinessFormData.AdditionalInfo);


    }
    
}