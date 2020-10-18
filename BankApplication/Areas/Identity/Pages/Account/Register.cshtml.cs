using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BankApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PESEL.Attributes;
using BankApplication.Data;
using BankApplication.Controllers;

namespace BankApplication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Adres e-mail")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasło i jego potwierdzenie są niezgodne.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Imię")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }

            [Required]
            [Pesel]
            [Display(Name = "PESEL")]
            public string PESEL { get; set; }

            [Display(Name = "Typ konta")]
            public int BankAccountTypeID { get; set; }
            public virtual BankAccountType BankAccountType { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    Profile profile = new Profile
                    {
                        Login = Input.Email,
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        PESEL = Input.PESEL,
                        BankAccounts = new List<BankAccount>()
                    };

                    BankAccount bankAccount = new BankAccount()
                    {
                        Balance = 0.0m,
                        AvailableFounds = 0.0m,
                        Lock = 0.0m,
                        BankAccountNumber = NewBankAcocuntNumber(),
                        CreationDate = DateTime.Today,
                        BankAccountTypeID = Input.BankAccountTypeID,
                        CurrencyID = _context.Currencies.Single(c => c.Code == "PLN").ID
                    };

                    _context.BankAccounts.Add(bankAccount);
                    profile.BankAccounts.Add(bankAccount);
                    _context.Profiles.Add(profile);
                    _context.SaveChanges();

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {

                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private string NewBankAcocuntNumber()
        {
            string last = _context.BankAccounts.OrderByDescending(b => b.BankAccountNumber).First().BankAccountNumber;
            int parts = int.Parse(last.Split(' ')[5] + last.Split(' ')[6]) + 1;
            string newNumber = last;
            newNumber = newNumber.Replace(newNumber.Split(' ')[5], parts.ToString().Substring(0, 4));
            newNumber = newNumber.Replace(newNumber.Split(' ')[6], parts.ToString().Substring(4, 4));

            return newNumber;
        }
    }
}
