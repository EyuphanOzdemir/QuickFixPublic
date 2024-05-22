using HttpService.Services.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Dto;
using Infrastructure.Utility;
using QuickFixWeb.Pages.Base;
using QuickFixWeb.Services;
using QuickFixWeb.Services.IService;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using static QuickFixWeb.Consts.Consts;

namespace QuickFixWeb.Pages.Account;
public class RegisterModel(IAuthService authService) : BasePageModel
{
    private readonly IAuthService _authService = authService;

    [BindProperty]
    [Required]
    public string Name { get; set; }

    [BindProperty]
    [EmailAddress]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }


    public override async Task<IActionResult> OnPostAsync(object model=null)
    {
        //throw new Exception("Dummy Exception");
        var result = await base.OnPostAsync(); // Call the base method 
        if (result is not null)
            return result;

        try
        {
            var registerModel = new RegistrationRequestDto
            {
                Name = Name,
                Email = Email,
                Password = Password
            };

            //check user's name alrady exists in the DB
            var response = await _authService.CheckName(Name);
            if ((bool)response.Result)
            {
                ViewData["Message"] = "A user with the same name/author address exists!";
                return Page();
            }

            ResponseDto responseDTO = await _authService.RegisterAsync(registerModel);
            ResponseDto assingRole;

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                if (string.IsNullOrEmpty(registerModel.Role))
                {
                    registerModel.Role = GlobalValues.RoleCustomer;
                }
                assingRole = await _authService.AssignRoleAsync(registerModel);
            }
            else
            {
                ViewData["Message"] = Error_Indicator + responseDTO!.Message;
                return Page();
            }
        }
        catch (Exception ex)
        {
            ViewData["Message"] =Error_Indicator + ex.Message;
            return Page();
        }

        return RedirectToPage("Login");
    }
}
