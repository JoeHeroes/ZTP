using ZTP.Models;

namespace ZTP.Seeder
{
    public class ZTPSeeder
    {
        private readonly ZTPDbContext _dbContext;

        public ZTPSeeder(ZTPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Words.Any())
                {
                    var info = GetWords();
                    _dbContext.Words.AddRange(info);
                    _dbContext.SaveChanges();
                }
            }
        }

        public IEnumerable<Word> GetWords()
        {
            return new List<Word>()
            {
                new Word()
                {
                    PolishWord = "Zolty",
                    ForeignLanguageWord = "Yellow"
                },
                new Word()
                {
                    PolishWord = "Zielony",
                    ForeignLanguageWord = "Green"
                },
                new Word()
                {
                    PolishWord = "Czarny",
                    ForeignLanguageWord = "Black"
                },
                new Word()
                {
                    PolishWord = "Biały",
                    ForeignLanguageWord = "White"
                },
                new Word()
                {
                    PolishWord = "Różowy",
                    ForeignLanguageWord = "Pink"
                },
                new Word()
                {
                    PolishWord = "Niebieski",
                    ForeignLanguageWord = "Blue"
                },
                new Word()
                {
                    PolishWord = "Fioletowy",
                    ForeignLanguageWord = "Purple"
                },
                new Word()
                {
                    PolishWord = "Pomarańczowy",
                    ForeignLanguageWord = "Orange"
                }
            };
        }
    }
}