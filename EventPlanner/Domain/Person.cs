using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Domain;

public class Person : BaseEntity
{
	[MaxLength(128)]
	public string FirstName { get; set; } = default!;
	
	[MaxLength(128)]
	public string LastName { get; set; } = default!;
	
	[MaxLength(11)]
	public string PersonalCode { get; set; } = default!;
	
	public ICollection<PersonParticipant>? Participations { get; set; } = default!;
}
