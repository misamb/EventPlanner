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
        public PersonParticipant? PersonParticipant { get; set; }

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
            if (id == null || PersonParticipant is null)
            {
                return NotFound();
            }

            await _context.DeletePersonParticipant(id.Value);

            return RedirectToPage("../Index");
        }
    }
}
