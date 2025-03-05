using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using WebApp.Domain;

namespace WebApp.Pages_BsnParticipant
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<BusinessParticipant> BusinessParticipant { get;set; } = default!;

        public async Task OnGetAsync()
        {
            BusinessParticipant = await _context.BusinessParticipants
                .Include(b => b.Business)
                .Include(b => b.Event)
                .Include(b => b.PaymentType).ToListAsync();
        }
    }
}
