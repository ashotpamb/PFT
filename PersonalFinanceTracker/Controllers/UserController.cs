using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Repositories;

namespace PersonalFinanceTracker.Controllers;

public class UserController:Controller
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(UserRegister userRegister)
    {
        try
        {
            await _userRepository.RegisterUser(userRegister);
            return RedirectToAction("SignIn");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}