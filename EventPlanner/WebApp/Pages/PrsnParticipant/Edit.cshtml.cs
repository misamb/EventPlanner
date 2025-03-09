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

namespace WebApp.Pages_PrsnParticipant
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }
        
        public Event? Event { get; set; }

        [BindProperty]
        public PersonParticipant? PersonParticipant { get; set; }
        
        [BindProperty]
        public Person? Person { get; set; }
        
        public SelectList PaymentTypeSelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? personParticipantId)
        {
            if (personParticipantId == null)
            {
                return NotFound();
            }

            PersonParticipant =  await _context.GetPersonParticipantWithPersonAndEventById(personParticipantId.Value);
            
            if (PersonParticipant == null)
            {
                return NotFound();
            }

            Person = PersonParticipant.Person;
            Event = PersonParticipant.Event;
            
            if (Person == null || Event == null)
            {
                return NotFound();
            }
            
            PaymentTypeSelectList = new SelectList(_context.PaymentTypes, nameof(PaymentType.Id),
                nameof(PaymentType.TypeName));
            
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

            try
            {
                await _context.EditPersonParticipant(PersonParticipant!);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonParticipantExists(PersonParticipant!.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            try
            {
                await _context.EditPerson(Person!);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(Person!.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Index");
        }

        private bool PersonParticipantExists(int id)
        {
            return _context.PersonParticipants.Any(e => e.Id == id);
        }
        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
