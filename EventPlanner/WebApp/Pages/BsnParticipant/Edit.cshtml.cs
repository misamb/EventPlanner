using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using WebApp.Domain;

namespace WebApp.Pages_BsnParticipant
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
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

            var businessparticipant =  await _context.BusinessParticipants.FirstOrDefaultAsync(m => m.Id == id);
            if (businessparticipant == null)
            {
                return NotFound();
            }
            BusinessParticipant = businessparticipant;
           ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "BusinessName");
           ViewData["EventId"] = new SelectList(_context.Events, "Id", "EventLocation");
           ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "TypeName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BusinessParticipant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessParticipantExists(BusinessParticipant.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BusinessParticipantExists(int id)
        {
            return _context.BusinessParticipants.Any(e => e.Id == id);
        }
    }
}
