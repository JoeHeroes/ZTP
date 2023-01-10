using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZTP.Models;
using ZTP.Models.DTO;
using ZTP.Models.Enum;

namespace ZTP.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly ZTPDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasherUser;
        private readonly Authentication authentications;

        public AccountController(ZTPDbContext dbContext, IPasswordHasher<User> passwordHasherUser, Authentication authentications)
        {
            this.dbContext = dbContext;
            this.passwordHasherUser = passwordHasherUser;
            this.authentications = authentications;
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("name");
            HttpContext.Session.Remove("id");
            return RedirectToAction("Login");
        }

        [Route("Welcome")]
        public IActionResult Welcome()
        {
            ViewBag.username = HttpContext.Session.GetString("name");
            return View("Welcome");
        }


        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult Register(RegisterUserDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                ViewBag.msg = "Invalid Password";
                return View("Register");
            }

            if (ModelState.IsValid)
            {
                var newUser = new User()
                {
                    Email = dto.Email,
                    UserName = dto.UserName,
                    PasswordHash = dto.Password,
                    Points = 0,
                    Difficulty = Difficulty.Normal,
                };

                var hashedPass = this.passwordHasherUser.HashPassword(newUser, dto.Password);

                newUser.PasswordHash = hashedPass;
                this.dbContext.Users.Add(newUser);
                this.dbContext.SaveChanges();

                return RedirectToAction("Welcome");
            }
            ViewBag.msg = "Invalid";
            return View("Register");
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult Login(LoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = this.dbContext
                                .Users
                                .FirstOrDefault(u => u.Email == dto.Email);

                if (user is null)
                {
                    ViewBag.msg = "Email or password is invalid.";
                    return View("Login");
                }

                var result = this.passwordHasherUser.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    ViewBag.msg = "Email or password is invalid.";
                    return View("Login");
                }
                HttpContext.Session.SetString("name", user.UserName);
                HttpContext.Session.SetString("id", user.Id.ToString());
                return RedirectToAction("Welcome");
            }
            return View(dto);
        }
    }
}
