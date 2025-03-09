using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Domain;
using WebApp.Pages_Persons;

namespace Tests;

public class PersonDeletePageTests
{
    [Fact]
    public async Task OnGetAsync_PopulatesThePageModel_WithPerson()
    {
        
        //Assert
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        var expectedPersonId = 1;
        var expectedPerson = new Person()
        {
            Id = expectedPersonId,
            FirstName = "John",
            LastName = "Doe",
            PersonalCode = "34501234215"
        };
        mockAppDbContext.Setup(
            db => db.GetPersonById(expectedPersonId)).Returns(Task.FromResult(expectedPerson));
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        
        //Act
        await pageModel.OnGetAsync(expectedPersonId);
        
        //Assert
        
        var actualPerson = Assert.IsAssignableFrom<Person>(pageModel.Person);
        Assert.Equal(expectedPerson, actualPerson);
    }
    
    
    [Fact]
    public async Task OnPostAsync_ReturnsARedirectToPageResult()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var mockAppDbContext = new Mock<AppDbContext>(optionsBuilder.Options);
        var pageModel = new DeleteModel(mockAppDbContext.Object);
        pageModel.Person = new Person();
        var recId = 1;

        // Act
        var result = await pageModel.OnPostAsync(recId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_DeletesPerson()
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

        var participation = new PersonParticipant()
        {
            Id = 1,
            PersonId = 1,
            EventId = 1,
            ParticipantCount = 1,
        };
        
        await appDbContext.PersonParticipants.AddAsync(participation);
        await appDbContext.Persons.AddAsync(testPerson);
        await appDbContext.SaveChangesAsync();
        
        var pageModel = new DeleteModel(appDbContext);
        pageModel.Person = testPerson;
        
        //Act
        var result = await pageModel.OnPostAsync(testPerson.Id);
        
        //Assert
        Assert.Empty(appDbContext.Persons);
        //Check cascade
        Assert.Empty(appDbContext.PersonParticipants);

    }
    
    
}