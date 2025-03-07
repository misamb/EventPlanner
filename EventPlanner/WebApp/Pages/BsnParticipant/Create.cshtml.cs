using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using WebApp.Domain;

namespace WebApp.Pages_BsnParticipant
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
        
        public SelectList? BusinessSelectList { get; set; }
        
        public async Task<IActionResult> OnGet(int eventId)
        {
            Event = await _context.GetEventById(eventId);

            PaymentTypeSelectList = new SelectList(_context.PaymentTypes, nameof(PaymentType.Id),
                nameof(PaymentType.TypeName));
            
            BusinessSelectList = new SelectList(_context.Businesses, nameof(Business.Id),
                nameof(Business.BusinessName));

            return Page();
        }

        [BindProperty]
        public BusinessParticipant BusinessParticipant { get; set; } = default!;
        
        [BindProperty]
        public Business Business { get; set; } = default!;
        
        [BindProperty]
        public bool IsSavedBusiness {get; set;}
        
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (IsSavedBusiness)
            {
                ModelState.Remove("Business.BusinessName");
                ModelState.Remove("Business.RegistryCode");
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (!IsSavedBusiness)
            {
                _context.Businesses.Add(Business);
                await _context.SaveChangesAsync();
                BusinessParticipant.BusinessId = Business.Id;
            }

            _context.BusinessParticipants.Add(BusinessParticipant);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
