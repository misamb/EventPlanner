using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Domain;

public class Event : BaseEntity
{
	[MaxLength(128)]
	public string EventName { get; set; } = default!;
	
	[MaxLength(128)]
	public string EventLocation { get; set; } = default!;

	public DateTime EventStartTime { get; set; }
	
	[MaxLength(1000)]
	public string? AdditionalInfo { get; set; }

	public ICollection<PersonParticipant>? PersonParticipants { get; set; }
	
	public ICollection<BusinessParticipant>? BusinessParticipants { get; set; }
}
