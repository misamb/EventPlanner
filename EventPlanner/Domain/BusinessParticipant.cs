using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class BusinessParticipant : Participant
{
    public Business? Business { get; set; }
    
    public int BusinessId { get; set; }
    
    [MaxLength(5000)]
    public string? AdditionalInfo { get; set; }
}