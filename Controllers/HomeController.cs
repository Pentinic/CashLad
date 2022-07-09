using CashLad.Data;
using CashLad.Data.Services;
using CashLad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CashLad.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }



        #region Accounts
        public IActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userService.GetByEmailAsync(model.Email);
            if (user == null)
            {
                // User doesnt exist
            }

            var passHash = EncryptPassword(model.Password, user.Salt);
            if (passHash == user.PassHash)
            {
                // Correct Login
                HttpContext.Session.SetObject("User", user);
            }
            else
            {
                // Incorrect Login
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetObject("User", null);
            var url = Request.Headers["Referer"].ToString();
            return Redirect(url);
        }

        public IActionResult CreateAccount()
        {
            var model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount(LoginModel model)
        {
            if (await _userService.GetByEmailAsync(model.Email) != null)
            {
                // User exists
                return View(model);
            }

            var salt = GenerateSalt();

            // Create User
            var user = new User()
            {
                Email = model.Email,
                Username = model.Username,
                Salt = salt,
                PassHash = EncryptPassword(model.Password, salt)
            };
            _userService.Add(user);

            ViewData.Clear();

            return View(model);
        }

        private static string GenerateID()
        {
            return Guid.NewGuid().ToString("N");
        }
        private static string GenerateSalt()
        {
            var random = new RNGCryptoServiceProvider();

            // Maximum length of salt
            int max_length = 32;

            // Empty salt array
            byte[] salt = new byte[max_length];

            // Build the random bytes
            random.GetNonZeroBytes(salt);

            // Return the string encoded salt
            return Convert.ToBase64String(salt);
        }
        private static string EncryptPassword(string password, string salt)
        {
            string passHash;
            password += salt;

            // Encrypt Password
            var data = Encoding.UTF8.GetBytes(password);
            using (SHA512 shaM = new SHA512Managed())
            {
                var bytes = shaM.ComputeHash(data);
                passHash = Convert.ToBase64String(bytes);
            }

            return passHash;
        }
        #endregion


        #region
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
