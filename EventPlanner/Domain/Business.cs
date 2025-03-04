using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class Business : BaseEntity
{
    [MaxLength(128)]
    public string BusinessName { get; set; } = default!;
    
    
    [RegularExpression("(^[1-9]{1}[0-9]{7}$)", 
        ErrorMessage = "Ebakorrektne registrikood!")]
    public string RegistryCode { get; set; } = default!;
    
    public ICollection<BusinessParticipant>? Participations { get; set; }
}