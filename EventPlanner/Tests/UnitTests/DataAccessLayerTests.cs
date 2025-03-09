using DAL;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain;

namespace Tests.UnitTests;

public class DataAccessLayerTests
{
    [Fact]
    public async Task TestCRUDOnEvent()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            //Arrange
            var testEventId = 1;
            var testEvent = new Event()
            {
                Id = testEventId,
                EventName = "Test Event",
                AdditionalInfo = "Test Additional Info",
                EventLocation = "Test Event Location",
                EventStartTime = DateTime.Now.AddMonths(1)
            };
            
            //Act - Create
            var idFromDb = await db.CreateEvent(testEvent);
            var eventFromDb = await db.Events.FirstAsync(e => e.Id == testEventId);
            
            //Assert
            Assert.Equal(testEventId, idFromDb);
            Assert.Equal(testEvent.EventName, eventFromDb.EventName);
            Assert.Equal(testEvent.EventLocation, eventFromDb.EventLocation);
            Assert.Equal(testEvent.EventStartTime, eventFromDb.EventStartTime);
            Assert.Equal(testEvent.AdditionalInfo, eventFromDb.AdditionalInfo);
            
            //Act - Read
            var eventFromMethod = await db.GetEventById(testEventId);
            
            //Assert
            Assert.Equal(testEvent.EventName, eventFromMethod!.EventName);
            Assert.Equal(testEvent.EventLocation, eventFromMethod.EventLocation);
            Assert.Equal(testEvent.EventStartTime, eventFromMethod.EventStartTime);
            Assert.Equal(testEvent.AdditionalInfo, eventFromMethod.AdditionalInfo);
            
            //Arrange - Update
            var updatedInfo = "Test Updated Info";
            var updatedLocation = "Test Updated Location";
            eventFromDb.AdditionalInfo = updatedInfo;
            eventFromDb.EventLocation = updatedLocation;
            
            //Act
            await db.EditEvent(eventFromDb);
            var updatedEvent = await db.Events.FirstAsync(e => e.Id == testEventId);
            
            //Assert
            Assert.Equal(updatedEvent.AdditionalInfo, updatedInfo);
            Assert.Equal(updatedEvent.EventLocation, updatedLocation);
            
            //Act - Delete
            await db.DeleteEvent(testEventId);
            
            //Assert
            Assert.Empty(db.Events);

        }
    }

    [Fact]
    public async Task TestCRUDOnPerson()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            //Arrange
            var testPersonId = 1;
            var testPerson = new Person()
            {
                Id = testPersonId,
                FirstName = "Test First",
                LastName = "Test Last",
                PersonalCode = "34501234215"
            };
            
            //Act - Create
            var idFromDb = await db.CreatePerson(testPerson);
            var personFromDb = await db.Persons.FirstAsync(e => e.Id == testPersonId);
            
            //Assert
            Assert.Equal(testPersonId, idFromDb);
            Assert.Equal(testPerson.FirstName, personFromDb.FirstName);
            Assert.Equal(testPerson.LastName, personFromDb.LastName);
            Assert.Equal(testPerson.PersonalCode, personFromDb.PersonalCode);
            
            //Act - Read
            var personFromMethod = await db.GetPersonById(testPersonId);
            
            //Assert
            Assert.Equal(testPerson.FirstName, personFromMethod!.FirstName);
            Assert.Equal(testPerson.LastName, personFromMethod.LastName);
            Assert.Equal(testPerson.PersonalCode, personFromMethod.PersonalCode);
            
            //Arrange - Update
            var updatedPersonalCode = "49403136526";
            personFromDb.PersonalCode = updatedPersonalCode;
            
            //Act
            await db.EditPerson(personFromDb);
            var updatedPerson = await db.Persons.FirstAsync(e => e.Id == testPersonId);
            
            //Assert
            Assert.Equal(updatedPerson.PersonalCode, updatedPersonalCode);
            
            //Act - Delete
            await db.DeletePerson(testPersonId);
            
            //Assert
            Assert.Empty(db.Persons);

        }
        
    }
    
    [Fact]
    public async Task TestCRUDOnBusiness()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            //Arrange
            var testBusinessId = 1;
            var testBusiness = new Business()
            {
                Id = testBusinessId,
                BusinessName = "Test Business",
                RegistryCode = "10421629"
            };
            
            //Act - Create
            var idFromDb = await db.CreateBusiness(testBusiness);
            var businessFromDb = await db.Businesses.FirstAsync(e => e.Id == testBusinessId);
            
            //Assert
            Assert.Equal(testBusinessId, idFromDb);
            Assert.Equal(testBusiness.BusinessName, businessFromDb.BusinessName);
            Assert.Equal(testBusiness.RegistryCode, businessFromDb.RegistryCode);
            
            //Act - Read
            var businessFromMethod = await db.GetBusinessById(testBusinessId);
            
            //Assert
            Assert.Equal(testBusiness.BusinessName, businessFromMethod!.BusinessName);
            Assert.Equal(testBusiness.RegistryCode, businessFromMethod.RegistryCode);
            
            //Arrange - Update
            var updatedRegistryCode = "12004434";
            businessFromDb.RegistryCode = updatedRegistryCode;
            
            //Act
            await db.EditBusiness(businessFromDb);
            var updatedBusiness = await db.Businesses.FirstAsync(e => e.Id == testBusinessId);
            
            //Assert
            Assert.Equal(updatedBusiness.RegistryCode, updatedRegistryCode);
            
            //Act - Delete
            await db.DeleteBusiness(testBusinessId);
            
            //Assert
            Assert.Empty(db.Businesses);

        }
        
    }
    
    [Fact]
    //Does not test FK or Cascade
    public async Task TestCRUDOnPersonParticipant()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            //Arrange
            var testEventId = 1;
            var testEvent = new Event()
            {
                Id = testEventId,
                EventName = "Test Event",
                AdditionalInfo = "Test Additional Info",
                EventLocation = "Test Event Location",
                EventStartTime = DateTime.Now.AddMonths(1)
            };
            
            var testPersonId = 2;
            var testPerson = new Person()
            {
                Id = testPersonId,
                FirstName = "Test First",
                LastName = "Test Last",
                PersonalCode = "34501234215"
            };
            
            var testPersonParticipantId = 3;
            var testPersonParticipant = new PersonParticipant()
            {
                Id = testPersonParticipantId,
                EventId = testEventId,
                PersonId = testPersonId,
                PaymentTypeId = 999,
                ParticipantCount = 1,
                AdditionalInfo = "Test Additional Info"
            };

            await db.CreateEvent(testEvent);
            await db.CreatePerson(testPerson);
            
            //Act - Create
            var idFromDb = await db.CreatePersonParticipant(testPersonParticipant);
            var personParticipantFromDb = await db.PersonParticipants.FirstAsync(e => e.Id == testPersonParticipantId);
            
            //Assert
            Assert.Equal(testPersonParticipantId, idFromDb);
            Assert.Equal(testPersonParticipant.PersonId, personParticipantFromDb.PersonId);
            Assert.Equal(testPersonParticipant.EventId, personParticipantFromDb.EventId);
            Assert.Equal(testPersonParticipant.ParticipantCount, personParticipantFromDb.ParticipantCount);
            Assert.Equal(testPersonParticipant.AdditionalInfo, personParticipantFromDb.AdditionalInfo);
            Assert.Equal(testPersonParticipant.PaymentTypeId, personParticipantFromDb.PaymentTypeId);
            
            //Act - Read
            var personParticipantFromMethod = await db.GetPersonParticipantWithPersonAndEventById(testPersonParticipantId);
            
            //Assert
            
            //PersonParticipant
            Assert.Equal(testPersonParticipant.PersonId, personParticipantFromMethod!.PersonId);
            Assert.Equal(testPersonParticipant.EventId, personParticipantFromMethod.EventId);
            Assert.Equal(testPersonParticipant.ParticipantCount, personParticipantFromMethod.ParticipantCount);
            Assert.Equal(testPersonParticipant.AdditionalInfo, personParticipantFromMethod.AdditionalInfo);
            Assert.Equal(testPersonParticipant.PaymentTypeId, personParticipantFromMethod.PaymentTypeId);
            
            //Event
            Assert.Equal(personParticipantFromMethod!.Event!.EventName, testEvent.EventName);
            Assert.Equal(personParticipantFromMethod.Event.EventLocation, testEvent.EventLocation);
            Assert.Equal(personParticipantFromMethod.Event.AdditionalInfo, testEvent.AdditionalInfo);
            Assert.Equal(personParticipantFromMethod.Event.EventStartTime, testEvent.EventStartTime);
            
            //Person
            Assert.Equal(personParticipantFromMethod.Person!.FirstName, testPerson.FirstName);
            Assert.Equal(personParticipantFromMethod.Person!.LastName, testPerson.LastName);
            Assert.Equal(personParticipantFromMethod.Person.PersonalCode, testPerson.PersonalCode);
            
            //Arrange - Update
            var updatedAdditionalInfo = "Test new Additional Info";
            personParticipantFromDb.AdditionalInfo = updatedAdditionalInfo;
            
            //Act
            await db.EditPersonParticipant(personParticipantFromDb);
            var updatedPersonParticipant = await db.PersonParticipants.FirstAsync(e => e.Id == testPersonParticipantId);
            
            //Assert
            Assert.Equal(updatedPersonParticipant.AdditionalInfo, updatedAdditionalInfo);
            
            //Act - Delete
            await db.DeletePersonParticipant(testPersonParticipantId);
            
            //Assert
            Assert.Empty(db.PersonParticipants);
        }
        
    }
    
    [Fact]
    //Does not test FK or Cascade
    public async Task TestCRUDOnBusinessParticipant()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            //Arrange
            var testEventId = 1;
            var testEvent = new Event()
            {
                Id = testEventId,
                EventName = "Test Event",
                AdditionalInfo = "Test Additional Info",
                EventLocation = "Test Event Location",
                EventStartTime = DateTime.Now.AddMonths(1)
            };
            
            var testBusinessId = 2;
            var testBusiness = new Business()
            {
                Id = testBusinessId,
                BusinessName = "Test Business",
                RegistryCode = "10421629"
            };
            
            var testBusinessParticipantId = 3;
            var testBusinessParticipant = new BusinessParticipant()
            {
                Id = testBusinessParticipantId,
                EventId = testEventId,
                BusinessId = testBusinessId,
                PaymentTypeId = 999,
                ParticipantCount = 56,
                AdditionalInfo = "Test Additional Info"
            };

            await db.CreateEvent(testEvent);
            await db.CreateBusiness(testBusiness);
            
            //Act - Create
            var idFromDb = await db.CreateBusinessParticipant(testBusinessParticipant);
            var businessParticipantFromDb = await db.BusinessParticipants.FirstAsync(e => e.Id == testBusinessParticipantId);
            
            //Assert
            Assert.Equal(testBusinessParticipantId, idFromDb);
            Assert.Equal(testBusinessParticipant.BusinessId, businessParticipantFromDb.BusinessId);
            Assert.Equal(testBusinessParticipant.EventId, businessParticipantFromDb.EventId);
            Assert.Equal(testBusinessParticipant.ParticipantCount, businessParticipantFromDb.ParticipantCount);
            Assert.Equal(testBusinessParticipant.AdditionalInfo, businessParticipantFromDb.AdditionalInfo);
            Assert.Equal(testBusinessParticipant.PaymentTypeId, businessParticipantFromDb.PaymentTypeId);
            
            //Act - Read
            var businessParticipantFromMethod = await db.GetBusinessParticipantWithBusinessAndEventById(testBusinessParticipantId);
            
            //Assert
            
            //PersonParticipant
            Assert.Equal(testBusinessParticipant.BusinessId, businessParticipantFromMethod!.BusinessId);
            Assert.Equal(testBusinessParticipant.EventId, businessParticipantFromMethod.EventId);
            Assert.Equal(testBusinessParticipant.ParticipantCount, businessParticipantFromMethod.ParticipantCount);
            Assert.Equal(testBusinessParticipant.AdditionalInfo, businessParticipantFromMethod.AdditionalInfo);
            Assert.Equal(testBusinessParticipant.PaymentTypeId, businessParticipantFromMethod.PaymentTypeId);
            
            //Event
            Assert.Equal(businessParticipantFromMethod!.Event!.EventName, testEvent.EventName);
            Assert.Equal(businessParticipantFromMethod.Event.EventLocation, testEvent.EventLocation);
            Assert.Equal(businessParticipantFromMethod.Event.AdditionalInfo, testEvent.AdditionalInfo);
            Assert.Equal(businessParticipantFromMethod.Event.EventStartTime, testEvent.EventStartTime);
            
            //Business
            Assert.Equal(businessParticipantFromMethod.Business!.BusinessName, testBusiness.BusinessName);
            Assert.Equal(businessParticipantFromMethod.Business!.RegistryCode, testBusiness.RegistryCode);
            
            //Arrange - Update
            var updatedAdditionalInfo = "Test new Additional Info";
            var updatedParticipationCount = 20;
            businessParticipantFromDb.AdditionalInfo = updatedAdditionalInfo;
            businessParticipantFromDb.ParticipantCount = updatedParticipationCount;
            
            //Act
            await db.EditBusinessParticipant(businessParticipantFromDb);
            var updatedBusinessParticipant = await db.BusinessParticipants.FirstAsync(e => e.Id == testBusinessParticipantId);
            
            //Assert
            Assert.Equal(updatedBusinessParticipant.AdditionalInfo, updatedAdditionalInfo);
            Assert.Equal(updatedBusinessParticipant.ParticipantCount, updatedParticipationCount);
            
            //Act - Delete
            await db.DeleteBusinessParticipant(testBusinessParticipantId);
            
            //Assert
            Assert.Empty(db.BusinessParticipants);
        }
        
    }

    [Fact]
    public async Task TestMethod_GetEventWithAllParticipantsById()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            //Arrange
            var personAtEventId = 1;
            var personNotAtEventId = 2;
            db.Persons.AddRange(new List<Person>()
            {
                new Person()
                {
                    Id = personAtEventId,
                    FirstName = "John",
                    LastName = "Doe",
                    PersonalCode = "49403136515",
                },
                new Person()
                {
                    Id = personNotAtEventId,
                    FirstName = "Jane",
                    LastName = "Doe",
                    PersonalCode = "49403136516",
                }
            });
            var businessAtEventId = 1;
            var businessNotAtEventId = 2;
            db.Businesses.AddRange(new List<Business>()
            {
                new Business()
                {
                    Id = businessAtEventId,
                    BusinessName = "Good Business",
                    RegistryCode = "12345678"
                },
                new Business()
                {
                    Id = businessNotAtEventId,
                    BusinessName = "Seed Business",
                    RegistryCode = "12345578"
                }
            });
            var eventId = 1;
            db.Events.Add(new Event()
            {
                Id = eventId,
                EventName = "Good Event",
                EventLocation = "London",
                EventStartTime = DateTime.Today.AddMonths(1)
            });
            db.PersonParticipants.Add(new PersonParticipant()
            {
                Id = 1,
                PersonId = personAtEventId,
                EventId = eventId,
                ParticipantCount = 1,
                PaymentTypeId = 999,
                AdditionalInfo = "Test Additional Info"
            });
            db.BusinessParticipants.Add(new BusinessParticipant()
            {
                Id = 1,
                BusinessId = businessAtEventId,
                EventId = eventId,
                ParticipantCount = 10,
                PaymentTypeId = 999,
                AdditionalInfo = "Test Additional Info"
            });
            
            await db.SaveChangesAsync();
            
            //Act
            var eventFromDb = await db.GetEventWithAllParticipantsById(eventId);
            var personsAtEvent = new List<int>(eventFromDb!.PersonParticipants.Select(pp => pp.Person!.Id));
            var businessesAtEvent = new List<int>(eventFromDb.BusinessParticipants.Select(bp => bp.Business!.Id));
            
            //Assert
            Assert.Contains(personAtEventId, personsAtEvent);
            Assert.DoesNotContain(personNotAtEventId, personsAtEvent);
            Assert.Contains(businessAtEventId, businessesAtEvent);
            Assert.DoesNotContain(businessNotAtEventId, businessesAtEvent);
            
        }
    }

    [Fact]
    public async Task TestMethod_GetPersonsNotAtEvent()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            //Arrange
            var personAtEvent = Utilities.seedPersons;
            db.Persons.AddRange(personAtEvent);

            var expectedPersonsNotAtEvent = new List<Person>()
            {
                new Person()
                {
                    Id = 3,
                    FirstName = "Not",
                    LastName = "Doe",
                    PersonalCode = "49403136515",
                },
                new Person()
                {
                    Id = 4,
                    FirstName = "Jane",
                    LastName = "Not",
                    PersonalCode = "49403136516",
                }
            };
            db.Persons.AddRange(expectedPersonsNotAtEvent);
            
            var eventId = 1;
            db.Events.Add(new Event()
            {
                Id = eventId,
                EventName = "Test Event",
                EventLocation = "London",
            });
            
            db.PersonParticipants.AddRange(
                new PersonParticipant()
            {
                Id = 1,
                PersonId = 1,
                EventId = eventId,
                PaymentTypeId = 999
            }, 
                new PersonParticipant()
                {   
                    Id = 2, 
                    PersonId = 2,
                    EventId = eventId,
                    PaymentTypeId = 999
                });
            
            await db.SaveChangesAsync();
            
            //Act
            var actualPersonsNotAtEvent = await db.GetPersonsNotAtEventById(eventId);
            
            //Assert
            Assert.Equal(actualPersonsNotAtEvent.OrderBy(p => p.Id).Select(p => p.Id),
                expectedPersonsNotAtEvent.OrderBy(p => p.Id).Select(p => p.Id));
        }
        
    }
    
    [Fact]
    public async Task TestMethod_GetBusinessesNotAtEvent()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            //Arrange
            var businessesAtEvent = Utilities.seedBusinesses;
            db.Businesses.AddRange(businessesAtEvent);

            var expectedBusinessesNotAtEvent = new List<Business>()
            {
                new Business()
                {
                    Id = 3,
                    BusinessName = "Good Business",
                    RegistryCode = "12345678"
                },
                new Business()
                {
                    Id = 4,
                    BusinessName = "Seed Business",
                    RegistryCode = "01234567"
                }
            };
            db.Businesses.AddRange(expectedBusinessesNotAtEvent);
            
            var eventId = 1;
            db.Events.Add(new Event()
            {
                Id = eventId,
                EventName = "Test Event",
                EventLocation = "London",
            });
            
            db.BusinessParticipants.AddRange(
                new BusinessParticipant()
                {
                    Id = 1,
                    BusinessId = 1,
                    EventId = eventId,
                    PaymentTypeId = 999
                }, 
                new BusinessParticipant()
                {   
                    Id = 2, 
                    BusinessId = 2,
                    EventId = eventId,
                    PaymentTypeId = 999
                });
            
            await db.SaveChangesAsync();
            
            //Act
            var actualBusinessesNotAtEvent = await db.GetBusinessesNotAtEventById(eventId);
            
            //Assert
            Assert.Equal(actualBusinessesNotAtEvent.OrderBy(p => p.Id).Select(p => p.Id),
                expectedBusinessesNotAtEvent.OrderBy(p => p.Id).Select(p => p.Id));
        }
        
    }
}