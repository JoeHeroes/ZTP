using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZTP.Models;
using ZTP.Models.Enum;
using ZTP.Models.ModelView;
using ZTP.State;

namespace ZTP.Controllers
{
    public class WordsController : Controller
    {
        private readonly ZTPDbContext _context;

        private Context _contextState;

        private QuestionViewModel _questionViewModel;

        public WordsController(ZTPDbContext context)
        {
            _context = context;
            _contextState = new Context();
            _questionViewModel = new QuestionViewModel();
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.Words.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Words == null)
            {
                return NotFound();
            }

            var word = await _context.Words
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,PolishWord,ForeignLanguageWord")] Word word)
        {
            if (ModelState.IsValid)
            {
                _context.Add(word);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(word);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Words == null)
            {
                return NotFound();
            }

            var word = await _context.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PolishWord,ForeignLanguageWord")] Word word)
        {
            if (id != word.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(word);
                    await _context.SaveChangesAsync();
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Words == null)
            {
                return NotFound();
            }

            var word = await _context.Words
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Words == null)
            {
                return Problem("Entity set 'ZTPDbContext.Words'  is null.");
            }
            var word = await _context.Words.FindAsync(id);
            if (word != null)
            {
                _context.Words.Remove(word);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WordExists(int id)
        {
          return _context.Words.Any(e => e.Id == id);
        }

        /// <summary>
        /// </summary>
        public IActionResult ChangeLanguage()
        {
            return View();
        }

        public IActionResult ChangeDifficlulty()
        {
            return View();
        }

        public IActionResult NormalQuestion()
        {
            _questionViewModel.Difficulty = Difficulty.Normal;

            return View();
        }

        public IActionResult HardQuestion()
        {
            _questionViewModel.Difficulty = Difficulty.Hard;

            return View();
        }
        public IActionResult Test(int id)
        {
            id = 1;
            var test = _context.Words.FirstOrDefault(x => x.Id == id);
            _contextState.ChangedState(new LearningState());
            return View(test);
        }
        public IActionResult Learn(int id)
        {
            id = 1;
            var learn = _context.Words.FirstOrDefault(x => x.Id == id);
            _contextState.ChangedState(new TestingState());
            return View(learn);
        }
    }
}
