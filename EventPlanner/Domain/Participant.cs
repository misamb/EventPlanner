using System;
using WebApp.Domain;

public class Participant : BaseEntity
{
	public Event? Event { get; set; } 

	public int EventId { get; set; }
	
	public PaymentType? PaymentType { get; set; }
	
	public int PaymentTypeId { get; set; }
	
	public int ParticipantCount { get; set; }


}
