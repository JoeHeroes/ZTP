using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZTP.Fascade;
using ZTP.Models;
using ZTP.Models.DTO;
using ZTP.Models.Enum;

namespace ZTP.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        
        private readonly IPasswordHasher<User> passwordHasherUser;
        private readonly Authentication authentications;
        private readonly DatabaseConnection database;
        private readonly ZTPDbContext context;

        public AccountController(IPasswordHasher<User> passwordHasherUser, Authentication authentications, ZTPDbContext context)
        {
            this.passwordHasherUser = passwordHasherUser;
            this.authentications = authentications;
            this.database = new DatabaseConnection(context);
            this.context = context;
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
            var user = this.database.GetUserByEmail(dto.Email);

            if (user != null)
            {
                ViewBag.msg = "Email is taken";
                return View("Register");
            }


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
                    RoleId = dto.RoleId,
                };

                var hashedPass = this.passwordHasherUser.HashPassword(newUser, dto.Password);

                newUser.PasswordHash = hashedPass;
                this.database.AddUser(newUser);
                this.database.SaveChanges();



                return RedirectToAction("Welcome");
            }

            ViewBag.msg = "Invalid";

            return View("Register");
        }

        [HttpPost]
        [Route("LoginUser")]                         //metoda logująca
        public IActionResult Login(LoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = this.database.GetUserByEmail(dto.Email);

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

                if (user.RoleId == 1)
                {
                    HttpContext.Session.SetString("role", "User");
                }
                else
                {
                    HttpContext.Session.SetString("role", "Admin");
                }
                HttpContext.Session.SetString("name", user.UserName);
                HttpContext.Session.SetString("id", user.Id.ToString());

                return RedirectToAction("Welcome");
            }

            return View(dto);
        }

        [Route("ChangeDifficulty")]                                 //zmiana trudności pytań
        public IActionResult ChangeDifficulty(int difficultyNumber)
        {
            if (difficultyNumber != 0)
            {
                HttpContext.Session.Remove("numberOfQuestionsLearn");
                HttpContext.Session.Remove("answersQuestionsLearn");
                HttpContext.Session.Remove("numberOfQuestionsTest");
                HttpContext.Session.Remove("answersQuestionsTest");

                int id = int.Parse(HttpContext.Session.GetString("id"));
                User user = this.database.GetUserById(id);
                if (difficultyNumber == 1)
                {
                    user.Difficulty = Difficulty.Easy;
                }
                else if (difficultyNumber == 2)
                {
                    user.Difficulty = Difficulty.Normal;
                }
                else if (difficultyNumber == 3)
                {
                    user.Difficulty = Difficulty.Hard;
                }
                HttpContext.Session.SetString("level", user.Difficulty.ToString());                                     
                TempData["Success"] = "Your difficulty is now set to " + user.Difficulty.ToString();   //wysłanie powiadomienia o zmianie trudności

                this.database.UpdateUser(user);
                this.database.SaveChanges();
            }
            else
            {
                TempData["Error"] = "Something went wrong with your difficulty level";
            }

            return View("Level");
        }

        [Route("Level")]
        public IActionResult Level(int difficultyNumber)
        {
            return View();
        }
    }
}