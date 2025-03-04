using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using WebApp.Domain;

namespace WebApp.Pages_BisParticipant
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessparticipant = await _context.BusinessParticipants.FindAsync(id);
            if (businessparticipant != null)
            {
                BusinessParticipant = businessparticipant;
                _context.BusinessParticipants.Remove(BusinessParticipant);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
