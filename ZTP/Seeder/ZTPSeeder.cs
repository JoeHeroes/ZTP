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
                    PolishWord = "zolty",
                    ForeignLanguageWord = "yellow"
                },
                new Word()
                {
                    PolishWord = "zielony",
                    ForeignLanguageWord = "green"
                },
                new Word()
                {
                    PolishWord = "czarny",
                    ForeignLanguageWord = "black"
                },
                new Word()
                {
                    PolishWord = "biały",
                    ForeignLanguageWord = "white"
                },
                new Word()
                {
                    PolishWord = "różowy",
                    ForeignLanguageWord = "pink"
                },
                new Word()
                {
                    PolishWord = "niebieski",
                    ForeignLanguageWord = "blue"
                },
                new Word()
                {
                    PolishWord = "fioletowy",
                    ForeignLanguageWord = "purple"
                },
                new Word()
                {
                    PolishWord = "pomarańczowy",
                    ForeignLanguageWord = "orange"
                },
                new Word()
                {
                    PolishWord = "ludzie",
                    ForeignLanguageWord = "people"
                },
                new Word()
                {
                    PolishWord = "zwierze",
                    ForeignLanguageWord = "animal"
                },
                new Word()
                {
                    PolishWord = "kurczak",
                    ForeignLanguageWord = "chicken"
                },
                new Word()
                {
                    PolishWord = "małpa",
                    ForeignLanguageWord = "monkey"
                },
                new Word()
                {
                    PolishWord = "królik",
                    ForeignLanguageWord = "rabbit"
                },
                new Word()
                {
                    PolishWord = "butterfly",
                    ForeignLanguageWord = "motyl"
                },
                new Word()
                {
                    PolishWord = "lis",
                    ForeignLanguageWord = "fox"
                },
                new Word()
                {
                    PolishWord = "jaszczurka",
                    ForeignLanguageWord = "lizard"
                },
                new Word()
                {
                    PolishWord = "czas",
                    ForeignLanguageWord = "time"
                },
                new Word()
                {
                    PolishWord = "rok",
                    ForeignLanguageWord = "year"
                },
                new Word()
                {
                    PolishWord = "dzień",
                    ForeignLanguageWord = "day"
                },
                new Word()
                {
                    PolishWord = "kobieta",
                    ForeignLanguageWord = "woman"
                },
                new Word()
                {
                    PolishWord = "szkoła",
                    ForeignLanguageWord = "school"
                }
            };
        }
    }
}