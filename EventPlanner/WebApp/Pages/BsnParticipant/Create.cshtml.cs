using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using WebApp.Domain;

namespace WebApp.Pages_BisParticipant
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "BusinessName");
        ViewData["EventId"] = new SelectList(_context.Events, "Id", "EventLocation");
            return Page();
        }

        [BindProperty]
        public BusinessParticipant BusinessParticipant { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BusinessParticipants.Add(BusinessParticipant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
