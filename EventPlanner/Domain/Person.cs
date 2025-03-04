using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Domain;

public class Person : BaseEntity
{
	[MaxLength(128)]
	public string FirstName { get; set; } = default!;
	
	[MaxLength(128)]
	public string LastName { get; set; } = default!;

	[RegularExpression("(^[1-6]{1}[0-9]{2}[0-1]{1}[0-9]{1}[0-2]{1}[0-9]{1}[0-9]{4}$)", 
		ErrorMessage = "Ebakorrektne isikukood!")]
	public string PersonalCode { get; set; } = default!;
	
	public ICollection<PersonParticipant>? Participations { get; set; } = default!;
}
