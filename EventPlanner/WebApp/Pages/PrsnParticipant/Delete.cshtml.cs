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
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PersonParticipant PersonParticipant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonParticipant = await _context.GetPersonParticipantWithPersonAndEventById(id.Value);

            if (PersonParticipant is not null)
            {

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

            var personparticipant = await _context.PersonParticipants.FindAsync(id);
            if (personparticipant != null)
            {
                PersonParticipant = personparticipant;
                _context.PersonParticipants.Remove(PersonParticipant);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}
