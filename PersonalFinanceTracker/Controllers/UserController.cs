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
    // [ValidateAntiForgeryToken]
    public IActionResult Register(UserRegister userRegister)
    {
        Console.WriteLine(userRegister.Email);
        return View();
        // _userRepository.RegisterUser(userRegister);
        // return View();
    }
}