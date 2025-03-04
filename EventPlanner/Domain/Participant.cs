using System;

public class Participant : BaseEntity
{
	public Event? Event { get; set; } 

	public int EventId { get; set; }
	
	public int ParticipantCount { get; set; }


}
