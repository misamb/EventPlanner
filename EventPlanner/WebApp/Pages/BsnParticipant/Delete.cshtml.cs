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
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BusinessParticipant? BusinessParticipant { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BusinessParticipant = await _context.GetBusinessParticipantWithBusinessAndEventById(id.Value);

            if (BusinessParticipant is not null)
            {
                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || BusinessParticipant is null)
            {
                return NotFound();
            }

            await _context.DeleteBusinessParticipant(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
