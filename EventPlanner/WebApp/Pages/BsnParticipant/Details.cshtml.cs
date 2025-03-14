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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public BusinessParticipant BusinessParticipant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessparticipant = await _context.BusinessParticipants.FirstOrDefaultAsync(m => m.Id == id);

            if (businessparticipant is not null)
            {
                BusinessParticipant = businessparticipant;

                return Page();
            }

            return NotFound();
        }
    }
}
