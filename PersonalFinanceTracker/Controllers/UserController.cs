using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Repositories;

namespace PersonalFinanceTracker.Controllers;

public class UserController : Controller
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

    public IActionResult SignOut()
    {
        HttpContext.Session.Remove("AuthToken");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        ViewBag.Message = TempData["Message"] as string ?? string.Empty;
        return View();
    }

    public IActionResult UserPage()
    {
        var token = HttpContext.Session.GetString("AuthToken");

        var claim = _userRepository.GetClaimFromToken(token);
        var user = _userRepository.GetUserByEmail(claim);
        return View(user.Result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> SignIn(UserLogin userLogin)
    {
        try
        {
            var token = await _userRepository.LoginUser(userLogin);
            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Session.SetString("AuthToken", token);

                return RedirectToAction("UserPage");
            }

            ViewBag.Message = "Failed to generate token";
            return View("SignIn");
        }
        catch (Exception e)
        {
            ViewBag.Message = e.Message;
            return View("SignIn");
        }
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