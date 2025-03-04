using System;
using WebApp.Domain;

public class Event : BaseEntity
{
	public string EventName { get; set; } = default!;

	public string EventLocation { get; set; } = default!;

	public DateTime EventStartTime { get; set; }

	public ICollection<PersonParticipant>? PersonParticipants { get; set; }
	
	public ICollection<BusinessParticipant>? BusinessParticipants { get; set; }
}
