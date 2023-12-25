using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTransaction(IFormCollection formData)
    {
        Transactions transaction = new()
        {
            TransactionType = (TransactionTypes)Enum.Parse(typeof(TransactionTypes), formData["type"].ToString()),
            TransactionDescription = formData["description"],
            TransactionAmount = Convert.ToDecimal(formData["amount"]),
            TransactionDate = DateTime.UtcNow
        };
        var token = HttpContext.Session.GetString("AuthToken");
        var claim = _userRepository.GetClaimFromToken(token);
        try
        {
            await _userRepository.AddTransactionToUser(claim, transaction);
            ViewBag.Message = "Successfully Transaction";
            return RedirectToAction("UserPage");
        }
        catch (Exception e)
        {
            ViewBag.Message = e.Message;
            return RedirectToAction("UserPage");
        }
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

    [HttpGet]
    public async Task<IActionResult> FilterTransactions(int userId, int transactionTypeFromRequest, int filterType)
    {
        var transactionType =
            (TransactionTypes)Enum.Parse(typeof(TransactionTypes), transactionTypeFromRequest.ToString());
        
        var filter = (Filter)Enum.Parse(typeof(Filter), filterType.ToString());
        
        List<Transactions> transactions = new List<Transactions>();
        
        switch (filter)
        {
            case Filter.Filter:
                transactions = await _userRepository.FilterTransactions(userId, transactionType);
                break;
            case Filter.Reset:
                transactions = await _userRepository.GetTransactions(userId);
                break;
 
        }
        return PartialView("_TransactionsFilter", transactions);
    }

    [HttpGet]
    public async Task<IActionResult> SearchTransactions(int userId, string searchQuery)
    {
        var transactions = await _userRepository.SearchTransactions(userId, searchQuery);
        return PartialView("_TransactionsFilter", transactions);
    }

    [Route("User/MainPage")]
    public IActionResult UserPage()
    {
        var token = HttpContext.Session.GetString("AuthToken");
        var claim = _userRepository.GetClaimFromToken(token);
        var user = _userRepository.GetUserByEmail(claim);
        ViewBag.Message = TempData["Message"] as string ?? string.Empty;
        return View(user.Result);
    }

    [HttpGet]
    public async Task<JsonResult> GetTransactions(int userID)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        var transations = await _userRepository.GetTransactions(userID);
        var serializedData = JsonConvert.SerializeObject(transations, settings);
        return Json(serializedData);
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