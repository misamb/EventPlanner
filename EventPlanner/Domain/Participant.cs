using System;

public class Participant : BaseEntity
{
	public Event? Event { get; set; } 

	public int EventId { get; set; }
	public virtual int ParticipantCount { get; set; }

	public virtual string? AdditionalInfo { get; set; }


}
