namespace WebApp.Domain;

public class PaymentType : BaseEntity
{
    public string TypeName { get; set; } = default!;
    
    public ICollection<BusinessParticipant>? BusinessParticipants { get; set; }
    
    public ICollection<PersonParticipant>? PersonParticipants { get; set; }
}