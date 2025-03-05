using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using WebApp.Domain;

namespace WebApp.Pages_PrsnParticipant
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }
        
        public Event? Event { get; set; }
        
        public SelectList? PaymentTypeSelectList { get; set; }
        
        public SelectList? PersonSelectList { get; set; }
        
        public async Task<IActionResult> OnGet(int eventId)
        {
            Event = await _context.GetEventById(eventId);

            PaymentTypeSelectList = new SelectList(_context.PaymentTypes, nameof(PaymentType.Id),
                nameof(PaymentType.TypeName));
            
            PersonSelectList = new SelectList(_context.Persons, nameof(Person.Id),
                nameof(Person.FirstName));

            return Page();
        }
        
        [BindProperty]
        public PersonParticipant PersonParticipant { get; set; } = default!;
        
        
        [BindProperty]
        public Person Person { get; set; } = default!;
        
        [BindProperty]
        public bool IsNewPerson { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsNewPerson)
            {
                ModelState.Remove("Person.FirstName");
                ModelState.Remove("Person.LastName");
                ModelState.Remove("Person.PersonalCode");
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (IsNewPerson)
            {
                _context.Persons.Add(Person);
                await _context.SaveChangesAsync();
                PersonParticipant.PersonId = Person.Id;
            }
            
            _context.PersonParticipants.Add(PersonParticipant);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
