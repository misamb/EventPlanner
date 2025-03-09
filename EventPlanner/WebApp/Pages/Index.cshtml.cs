using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    
    private readonly DAL.AppDbContext _context;

    public IndexModel(DAL.AppDbContext context)
    {
        _context = context;
    }

    public IList<Event> FutureEvents { get;set; } = new List<Event>();
    
    public IList<Event> PastEvents { get;set; } = new List<Event>();

    public async Task OnGetAsync()
    {
        var events = await _context.GetAllEventsWithParticipants();
        
        foreach (var e in events)
        {
            if (Helpers.IsInFuture(e.EventStartTime))
            {
                FutureEvents.Add(e);
            }
            else
            {
                PastEvents.Add(e);
            }
        }
    }

    public static int GetParticipantCount(Event e)
    {
        var result = 0;

        if (e.BusinessParticipants != null)
        {
            result += e.BusinessParticipants.Sum(bp => bp.ParticipantCount);
        }

        if (e.PersonParticipants != null)
        {
            result += e.PersonParticipants.Sum(pp => pp.ParticipantCount);
        }

        return result;
    }
    
    
    
}