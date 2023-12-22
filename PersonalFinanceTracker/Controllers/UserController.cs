using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Repositories;

namespace PersonalFinanceTracker.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly string? _token;
    

    public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _token = httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        ViewBag.Message = TempData["Message"] as string ?? string.Empty;
        return View();
    }

    public IActionResult UserPage()
    {
        if (string.IsNullOrEmpty(_token))
        {
            TempData["Message"] = "Unauthorized";
            return RedirectToAction("SignIn");
        }

        if (!_userRepository.CheckTokenExpire(_token))
        {
            TempData["Message"] = "Unauthorized";
            return RedirectToAction("SignIn");
        }
        var claim = _userRepository.GetClaimFromToken(_token);
        try
        {
            var user = _userRepository.GetUserByEmail(claim);
            return View(user.Result);
        }
        catch (Exception e)
        {
            TempData["Message"] = e.Message;
            return RedirectToAction("SignIn");
        }

    }
    
    [HttpPost]
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