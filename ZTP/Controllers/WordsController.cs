using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ZTP.DesignPatterns;
using ZTP.Fascade;
using ZTP.Models;
using ZTP.Models.Enum;
using ZTP.Models.ModelView;
using ZTP.State;

namespace ZTP.Controllers
{
    public class WordsController : Controller
    {
        private readonly Context contextState;
        private readonly DatabaseConnection database;
        private readonly ZTPDbContext context;

        public WordsController(ZTPDbContext context)
        {
            this.contextState = new Context();
            this.context = context;
            this.database = new DatabaseConnection(context);
        }

        public IActionResult Index()
        {
            return View(this.database.GetWords());
        }

        public IActionResult Details(int? id)
        {
            if (id == null || this.database.GetWords() == null)
            {
                return NotFound();
            }

            var word = this.database.GetWord((int)id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,PolishWord,ForeignLanguageWord")] Word word)
        {
            if (ModelState.IsValid)
            {
                this.database.AddWord(word);
                this.database.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(word);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || this.database.GetWords() == null)
            {
                return NotFound();
            }

            var word = this.database.GetWord((int)id);
            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,PolishWord,ForeignLanguageWord")] Word word)
        {
            if (id != word.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.database.UpdateWord(word);
                    this.database.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordExists(word.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(word);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || this.database.GetWords == null)
            {
                return NotFound();
            }

            var word = this.database.GetWord((int)id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (this.database.GetWords == null)
            {
                return Problem("Entity set 'ZTPDbContext.Words'  is null.");
            }
            var word = this.database.GetWord((int)id);
            if (word != null)
            {
                this.database.RemoveWord(word);
            }

            this.database.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool WordExists(int id)
        {
            return this.database.AnyWord(id);
        }

        public IActionResult ChangeLanguage(string language)
        {
            if(language != "")
            {
                switch (language)
                {
                    case "pl":
                        HttpContext.Session.Remove("lang");
                        HttpContext.Session.SetString("lang", "pl");
                        break;

                    case "eng":
                        HttpContext.Session.Remove("lang");
                        HttpContext.Session.SetString("lang", "eng");
                        break;
                }

                HttpContext.Session.Remove("numberOfQuestionsTest");
                HttpContext.Session.Remove("answersQuestionsTest");
                HttpContext.Session.Remove("numberOfQuestionsLearn");
                HttpContext.Session.Remove("answersQuestionsLearn");

                if(language == "pl")
                {
                    TempData["Success"] = "You change language on Polish";
                }
                else
                {
                    TempData["Success"] = "You change language on English";
                }
                

            }
            else
            {
                TempData["Error"] = "Something wrong with language";
            }
           

            return RedirectToAction("Lang");
        }

        public IActionResult Lang()
        {
            return View();
        }

        public IActionResult Learn()
        {
            HttpContext.Session.Remove("numberOfQuestionsTest");
            HttpContext.Session.Remove("answersQuestionsTest");
            int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));

            contextState.ChangedState(new LearningState());

            string answersString = HttpContext.Session.GetString("answersQuestionsLearn");
            AnswersQuestions answersQuestions;
            QuestionViewModel question;
            if (answersString == null)
            {
                answersQuestions = new AnswersQuestions(context, userId);
                answersQuestions.ContextState = contextState;
                answersQuestions.GenerateQuestions(5);

                question = answersQuestions.Iterator.First();
                HttpContext.Session.SetInt32("numberOfQuestionsLearn", 1);
                HttpContext.Session.SetString("answersQuestionsLearn", JsonConvert.SerializeObject(answersQuestions));
            }
            else
            {
                answersQuestions = JsonConvert.DeserializeObject<AnswersQuestions>(answersString);
                int? numberOfQuestions = HttpContext.Session.GetInt32("numberOfQuestionsLearn");
                answersQuestions.Iterator.numberOfQuestions = (int)numberOfQuestions;
                question = answersQuestions.GetQuestion();

                numberOfQuestions++;
                HttpContext.Session.SetInt32("numberOfQuestionsLearn", (int)numberOfQuestions);
            }

            if (question == null)
            {
                return RedirectToAction("LearnEnd");
            }

            string lang = HttpContext.Session.GetString("lang");
            if (lang != "pl" || lang == "eng")
            {
                ViewBag.Lang = "eng";
            }
            else
            {
                ViewBag.Lang = "pl";
            }

            return View(question);
        }

        [HttpPost]
        public IActionResult Learn(QuestionViewModel model)
        {
            string answersString = HttpContext.Session.GetString("answersQuestionsLearn");
            AnswersQuestions answersQuestions = JsonConvert.DeserializeObject<AnswersQuestions>(answersString);
            QuestionViewModel? question = answersQuestions.Iterator.questions[model.QuestionNumber - 1];
            model.Answers = question.Answers;
            model.CorrectWord = question.CorrectWord;

            string lang = HttpContext.Session.GetString("lang");
            if (lang != "pl" || lang == "eng")
            {
                ViewBag.Lang = "eng";
            }
            else
            {
                ViewBag.Lang = "pl";
            }

            if (!string.IsNullOrEmpty(model.Answer))
            {
                if (model.Answer == model.CorrectWord.PolishWord || model.Answer == model.CorrectWord.ForeignLanguageWord)
                {
                    int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));
                    UserWord userWord = new ();
                    userWord.UserId = userId;
                    userWord.WordId = model.CorrectWord.Id;
                    userWord.IsLearned = false;

                    this.database.AddUserWord(userWord);
                    this.database.SaveChanges();

                    return RedirectToAction("Learn");
                }

                model.Answer = "";
                ViewBag.NoAnswer = "Wrong answer";

                return View(model);
            }

            ViewBag.NoAnswer = "Pick answer";

            return View(model);
        }

        public IActionResult LearnEnd()
        {
            HttpContext.Session.Remove("numberOfQuestionsLearn");
            HttpContext.Session.Remove("answersQuestionsLearn");
            return View();
        }

        public IActionResult Test()
        {
            HttpContext.Session.Remove("numberOfQuestionsLearn");
            HttpContext.Session.Remove("answersQuestionsLearn");
            int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));

            contextState.ChangedState(new TestingState());

            string answersString = HttpContext.Session.GetString("answersQuestionsTest");
            AnswersQuestions answersQuestions;
            QuestionViewModel question;
            if (answersString == null)
            {
                List<int> ints = this.database.FindUserWordInts(userId);
                if (ints.Count < 5)
                {
                    HttpContext.Session.Remove("answersQuestionsTest");
                    return RedirectToAction("TestToEarly");
                }

                answersQuestions = new AnswersQuestions(context, userId);
                answersQuestions.ContextState = contextState;
                answersQuestions.GenerateQuestions(5);

                question = answersQuestions.Iterator.First();
                HttpContext.Session.Remove("points");
                HttpContext.Session.SetInt32("points", 0);
                HttpContext.Session.SetInt32("numberOfQuestionsTest", 1);
                HttpContext.Session.SetString("answersQuestionsTest", JsonConvert.SerializeObject(answersQuestions));
            }
            else
            {
                answersQuestions = JsonConvert.DeserializeObject<AnswersQuestions>(answersString);
                int? numberOfQuestions = HttpContext.Session.GetInt32("numberOfQuestionsTest");
                answersQuestions.Iterator.numberOfQuestions = (int)numberOfQuestions;
                question = answersQuestions.GetQuestion();

                numberOfQuestions++;
                HttpContext.Session.SetInt32("numberOfQuestionsTest", (int)numberOfQuestions);
            }

            if (question == null)
            {
                return RedirectToAction("TestEnd");
            }

            string lang = HttpContext.Session.GetString("lang");
            if (lang != "pl" || lang == "eng")
            {
                ViewBag.Lang = "eng";
            }
            else
            {
                ViewBag.Lang = "pl";
            }

            ViewBag.Points = HttpContext.Session.GetInt32("points");

            return View(question);
        }

        [HttpPost]
        public IActionResult Test(QuestionViewModel model)
        {
            string answersString = HttpContext.Session.GetString("answersQuestionsTest");
            AnswersQuestions answersQuestions = JsonConvert.DeserializeObject<AnswersQuestions>(answersString);
            QuestionViewModel? question = answersQuestions.Iterator.questions[model.QuestionNumber - 1];
            model.Answers = question.Answers;
            model.CorrectWord = question.CorrectWord;

            string lang = HttpContext.Session.GetString("lang");
            if (lang != null || lang == "eng")
            {
                ViewBag.Lang = "eng";
            }
            else
            {
                ViewBag.Lang = "pl";
            }

            if (!string.IsNullOrEmpty(model.Answer))
            {
                if (model.Answer == model.CorrectWord.PolishWord || model.Answer == model.CorrectWord.ForeignLanguageWord)
                {
                    int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));
                    UserWord userWord = this.database.FindUserWord(userId, model.CorrectWord.Id);
                    userWord.IsLearned = true;

                    this.database.UpdateUserWord(userWord);
                    this.database.SaveChanges();

                    int? points = HttpContext.Session.GetInt32("points");
                    points += 20;
                    HttpContext.Session.Remove("points");
                    HttpContext.Session.SetInt32("points", (int)points);

                    return RedirectToAction("Test");
                }

                model.Answer = "";
                return RedirectToAction("Test");
            }

            ViewBag.Points = HttpContext.Session.GetInt32("points");
            ViewBag.NoAnswer = "Pick answer";

            return View(model);
        }

        public IActionResult TestEnd()
        {
            HttpContext.Session.Remove("numberOfQuestionsTest");
            HttpContext.Session.Remove("answersQuestionsTest");

            int? points = HttpContext.Session.GetInt32("points");
            int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));
            User user = this.database.GetUserById(userId);

            user.Points += (int)points;

            if (user.Difficulty == Difficulty.Easy)
            {
                user.Difficulty = Difficulty.Normal;
            }
            else if (user.Difficulty == Difficulty.Normal)
            {
                user.Difficulty = Difficulty.Hard;
            }

            this.database.UpdateUser(user);
            this.database.SaveChanges();

            ViewBag.Points = points;

            return View();
        }

        public IActionResult TestToEarly()
        {
            return View();
        }
    }
}