namespace WebApp.Domain;

public class Business : BaseEntity
{
    public string BusinessName { get; set; } = default!;
    
    public string RegistryCode { get; set; } = default!;
    
    public ICollection<BusinessParticipant>? Participations { get; set; }
}