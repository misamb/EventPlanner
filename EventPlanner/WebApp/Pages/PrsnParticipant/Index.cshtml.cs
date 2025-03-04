using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using WebApp.Domain;

namespace WebApp.Pages_PrsnParticipant
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<PersonParticipant> PersonParticipant { get;set; } = default!;

        public async Task OnGetAsync()
        {
            PersonParticipant = await _context.PersonParticipants
                .Include(p => p.Event)
                .Include(p => p.Person).ToListAsync();
        }
    }
}
