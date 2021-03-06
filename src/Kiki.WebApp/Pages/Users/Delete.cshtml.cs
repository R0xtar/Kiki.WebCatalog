﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Kiki.WebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Kiki.WebApp.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Kiki.WebApp.Pages.Users
{
    [Authorize(Roles = "Admin,Kiki")]
    public class DeleteModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Utilisateur Utilisateur { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Utilisateur = (await _context.Users.SingleOrDefaultAsync(m => m.Id == id)).ToUtilisateur();

            if (Utilisateur == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (Utilisateur != null)
            {
                //_context.Utilisateur.Remove(Utilisateur);
                //await _context.SaveChangesAsync();
                await _userManager.DeleteAsync(user);
            }

            return RedirectToPage("./Index");
        }
    }
}
