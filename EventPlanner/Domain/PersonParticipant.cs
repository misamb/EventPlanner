using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class PersonParticipant : Participant
{
    public Person? Person { get; set; }
    
    public int PersonId {get; set;}

    public override int ParticipantCount { get; set; } = 1;
    
    [MaxLength(1500)]
    public override string? AdditionalInfo { get; set; }
}