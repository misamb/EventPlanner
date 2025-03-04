using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class PersonParticipant : Participant
{
    public Person? Person { get; set; }
    
    public int PersonId {get; set;}
    
    [MaxLength(1500)]
    public  string? AdditionalInfo { get; set; }
}