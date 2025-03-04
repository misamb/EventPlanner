using System;

public class Event : BaseEntity
{
	public string EventName { get; set; } = default!;

	public string EventLocation { get; set; } = default!;

	public DateTime EventStartTime { get; set; }

	public ICollection<Participant>? Participants { get; set; }
}
