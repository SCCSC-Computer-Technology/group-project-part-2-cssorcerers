using IQSport.Models.QuizModels;
using Microsoft.AspNetCore.Mvc;

namespace SportIQ.Controllers
{
    public class QuizController : Controller
    {
        // Define the teams for F1 quiz
        private readonly string[] teams = new[]
        {
            "Red Bull", "Mercedes", "Ferrari", "McLaren", "Aston Martin",
            "Alpine", "Williams", "Racing Bulls", "Kick Sauber", "Haas"
        };

        // This helper method returns the team name based on the index for the F1 quiz
        public string GetTeamName(int index)
        {
            if (index >= 0 && index < teams.Length)
            {
                return teams[index];
            }
            return "Unknown Team";
        }

        // Quiz index page
        public IActionResult Index()
        {
            return View();
        }

        // F1 Quiz GET method to render the F1 quiz page
        public IActionResult F1Quiz()
        {
            // Pass the GetTeamName method to the view via ViewBag
            ViewBag.GetTeamName = new Func<int, string>(GetTeamName);

            return View(); // Return the F1 Quiz view
        }

        // NFL Quiz GET method to render the NFL quiz page
        public IActionResult NFLQuiz()
        {
            return View(); // Return the NFL Quiz view
        }

        // NBA Quiz GET method to render the NBA quiz page
        public IActionResult NBAQuiz()
        {
            return View(); // Return the NBA Quiz view
        }

        // CSGO Quiz GET method to render the CS:GO quiz page
        public IActionResult CSGOQuiz()
        {
            return View(); // Return the CS:GO Quiz view
        }

        // Premier League Quiz GET method to render the Premier League quiz page
        public IActionResult PremierLeagueQuiz()
        {
            return View(); // Return the Premier League Quiz view
        }

        // F1 Quiz POST method to handle form submission
        [HttpPost]
        public IActionResult F1Quiz(F1QuizAnswersViewModel model)
        {
            // Example scoring logic (update it with your own correct answers)
            var correctAnswers = new List<List<string>>()
            {
                new() { "Max Verstappen", "Yuki Tsunoda" }, // Question 1 answers
                new() { "George Russell", "Kimi Antonelli" }, // Question 2 answers
                new() { "Charles Leclerc", "Lewis Hamilton" }, // Question 3 answers
                new() { "Lando Norris", "Oscar Piastri" }, // Question 4 answers
                new() { "Fernando Alonso", "Lance Stroll" }, // Question 5 answers
                new() { "Pierre Gasly", "Jack Doohan" }, // Question 6 answers
                new() { "Alex Albon", "Carlos Sainz" }, // Question 7 answers
                new() { "Isack Hadjar", "Liam Lawson" }, // Question 8 answers
                new() { "Nico Hulkenberg", "Gabriel Bortoleto" }, // Question 9 answers
                new() { "Esteban Ocon", "Ollie Bearman" }, // Question 10 answers
            };

            int score = 0;
            for (int i = 0; i < correctAnswers.Count; i++)
            {
                if (model.Answers.Count > i)
                {
                    var userAnswers = model.Answers[i] ?? new List<string>();
                    if (userAnswers.OrderBy(x => x).SequenceEqual(correctAnswers[i].OrderBy(x => x)))
                    {
                        score++;
                    }
                }
            }

            // Pass the score to the view
            ViewBag.Score = score;
            ViewBag.Total = correctAnswers.Count;

            ViewBag.GetTeamName = new Func<int, string>(GetTeamName);

            return View(model); // Return the same view with the model and score
        }


        // NFL Quiz POST method to handle quiz submission 
        [HttpPost]
        public IActionResult NFLQuiz(NFLQuizAnswersViewModel model)
        {
            var correctAnswers = new List<List<string>>()
            {
                new() { "Patrick Mahomes" },
                new() { "Kansas City Chiefs" },
                new() { "Tom Brady" },
                new() { "Green Bay Packers", "Chicago Bears" },
                new() { "Philadelphia Eagles" },
            };

            int score = 0;
            for (int i = 0; i < correctAnswers.Count; i++)
            {
                if (model.Answers.Count > i)
                {
                    var userAnswers = model.Answers[i] ?? new List<string>();
                    if (userAnswers.OrderBy(x => x).SequenceEqual(correctAnswers[i].OrderBy(x => x)))
                    {
                        score++;
                    }
                }
            }

            ViewBag.Score = score;
            ViewBag.Total = correctAnswers.Count;

            return View(model);
        }

        // NBA Quiz POST method to handle quiz submission 
        [HttpPost]
        public IActionResult NBAQuiz(NBAQuizAnswersViewModel model)
        {
            var correctAnswers = new List<List<string>>()
            {
                new() { "Denver Nuggets" },                      // Q1
                new() { "Stephen Curry" },                       // Q2
                new() { "Chicago Bulls" },                       // Q3
                new() { "Victor Wembanyama" },                   // Q4
                new() { "Milwaukee Bucks", "Boston Celtics" }    // Q5
            };

            int score = 0;
            for (int i = 0; i < correctAnswers.Count; i++)
            {
                if (model.Answers.Count > i)
                {
                    var userAnswers = model.Answers[i] ?? new List<string>();
                    if (userAnswers.OrderBy(x => x).SequenceEqual(correctAnswers[i].OrderBy(x => x)))
                    {
                        score++;
                    }
                }
            }

            ViewBag.Score = score;
            ViewBag.Total = correctAnswers.Count;

            return View(model);
        }

        // CSGO Quiz POST method to handle quiz submission 
        [HttpPost]
        public IActionResult CSGOQuiz(CSGOQuizAnswersViewModel model)
        {
            var correctAnswers = new List<List<string>>()
            {
                new() { "Natus Vincere" },                 // Q1
                new() { "s1mple" },                        // Q2
                new() { "FaZe Clan" },                     // Q3
                new() { "Dust2" },                         // Q4
                new() { "AWP", "AK-47" }                   // Q5
            };

            int score = 0;
            for (int i = 0; i < correctAnswers.Count; i++)
            {
                if (model.Answers.Count > i)
                {
                    var userAnswers = model.Answers[i] ?? new List<string>();
                    if (userAnswers.OrderBy(x => x).SequenceEqual(correctAnswers[i].OrderBy(x => x)))
                    {
                        score++;
                    }
                }
            }

            ViewBag.Score = score;
            ViewBag.Total = correctAnswers.Count;

            return View(model);
        }

        // Premier League Quiz POST method to handle quiz submission
        [HttpPost]
        public IActionResult PremierLeagueQuiz(PremierLeagueQuizAnswersViewModel model)
        {
            var correctAnswers = new List<List<string>>()
            {
                new() { "Manchester City" },               // Q1
                new() { "Erling Haaland" },                // Q2
                new() { "Arsenal"},                         // Q3
                new() { "Mohamed Salah" },                 // Q4
                new() { "Old Trafford" }                   // Q5
            };

            int score = 0;
            for (int i = 0; i < correctAnswers.Count; i++)
            {
                if (model.Answers.Count > i)
                {
                    var userAnswers = model.Answers[i] ?? new List<string>();
                    if (userAnswers.OrderBy(x => x).SequenceEqual(correctAnswers[i].OrderBy(x => x)))
                    {
                        score++;
                    }
                }
            }

            ViewBag.Score = score;
            ViewBag.Total = correctAnswers.Count;

            return View(model);
        }
    }
}