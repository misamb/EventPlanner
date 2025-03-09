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
        
        public SelectList PaymentTypeSelectList { get; set; } = default!;
        
        public Event? Event { get; set; }

        [BindProperty]
        public BusinessParticipant? BusinessParticipant { get; set; }
        
        [BindProperty]
        public Business? Business { get; set; }

        public async Task<IActionResult> OnGetAsync(int? businessParticipantId)
        {
            if (businessParticipantId == null)
            {
                return NotFound();
            }

            BusinessParticipant =  await _context.GetBusinessParticipantWithBusinessAndEventById(businessParticipantId.Value);
            
            if (BusinessParticipant == null)
            {
                return NotFound();
            }

            Business = BusinessParticipant.Business;
            
            Event = BusinessParticipant.Event;
            
            if (Business == null || Event == null)
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
                await _context.EditBusinessParticipant(BusinessParticipant);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessParticipantExists(BusinessParticipant!.Id))
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
                await _context.EditBusiness(Business);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(Business!.Id))
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

        private bool BusinessParticipantExists(int id)
        {
            return _context.BusinessParticipants.Any(e => e.Id == id);
        }
        
        private bool BusinessExists(int id)
        {
            return _context.Businesses.Any(e => e.Id == id);
        }
    }
}
