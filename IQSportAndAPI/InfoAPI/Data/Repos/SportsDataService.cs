using InfoAPI.Models;

namespace InfoAPI.Data.Repos
{
    public class SportsDataService : ISportsDataService
    {
        private readonly List<string> _sports = new() { "f1", "soccer", "nba", "nfl", "csgo" };
        private readonly List<Highlight> _highlights;
        private readonly List<Schedule> _schedules;

        public SportsDataService()
        {
            // Expanded Highlights Data (past events)
            _highlights = new List<Highlight>
            {
                // F1 Highlights
                new Highlight { Id = 1, Sport = "f1", Title = "Verstappen Overtakes Hamilton", Description = "Dramatic last-lap overtake in Monaco GP.", MatchDate = DateTime.UtcNow.AddDays(-2), VideoUrl = "https://youtu.be/MTe12fH2xtQ?feature=shared1", ImageUrl = "https://media.formula1.com/image/upload/f_auto,c_limit,w_1440,q_auto/t_16by9Centre/f_auto/q_auto/fom-website/2024/Miscellaneous/F1%20season%20launch%20event%20-%2018%20February%202024%20(press%20image)" },
                new Highlight { Id = 2, Sport = "f1", Title = "Leclerc's Pole Lap in Bahrain", Description = "Charles Leclerc sets a blistering lap to take pole.", MatchDate = DateTime.UtcNow.AddDays(-7), VideoUrl = "https://streamable.com/l7yh4", ImageUrl = "https://d2n9h2wits23hf.cloudfront.net/image/v1/static/6057949432001/cf81274a-749f-4ea1-9226-be7b3daa2d2f/749b713b-423f-43c6-a79e-2d7eddd7d051/432x243/match/image.jpg" },
                new Highlight { Id = 3, Sport = "f1", Title = "Norris Wins First GP in Miami", Description = "Lando Norris secures his first F1 victory.", MatchDate = DateTime.UtcNow.AddDays(-14), VideoUrl = "https://www.youtube.com/live/SHcgZwW61uE?feature=shared", ImageUrl = "https://media.cnn.com/api/v1/images/stellar/prod/ap24126818513818.jpg?c=16x9&q=h_833,w_1480,c_fill" },

                // Soccer Highlights
                new Highlight { Id = 4, Sport = "soccer", Title = "Messi Free Kick Goal", Description = "Stunning 30-yard free kick in UCL.", MatchDate = DateTime.UtcNow.AddDays(-1), VideoUrl = "https://www.uefa.com/uefachampionsleague/history/video/0250-0c5118216ed9-e3b8dc75be2f-1000--watch-all-of-lionel-messi-s-european-free-kick-goals/", ImageUrl = "https://compote.slate.com/images/2081a747-c118-4699-ace9-dcf308b2c605.jpg" },
                new Highlight { Id = 5, Sport = "soccer", Title = "Haaland's Hattrick vs Man Utd", Description = "Erling Haaland scores three in a thrilling derby.", MatchDate = DateTime.UtcNow.AddDays(-5), VideoUrl = "https://youtu.be/IkaclqsQFh4?feature=shared", ImageUrl = "https://www.westernslopenow.com/wp-content/uploads/sites/95/2024/09/66f072fd93c538.10558512.jpeg?w=1280" },
                new Highlight { Id = 6, Sport = "soccer", Title = "Mbappe's Solo Run vs Real Madrid", Description = "Kylian Mbappe dribbles past four defenders to score.", MatchDate = DateTime.UtcNow.AddDays(-10), VideoUrl = "https://youtu.be/LnFh6cbvoWM?feature=shared", ImageUrl = "https://cdn.britannica.com/39/239139-050-49A950D1/French-soccer-player-Kylian-Mbappe-FIFA-World-Cup-December-10-2022.jpg" },

                // NBA Highlights
                new Highlight { Id = 7, Sport = "nba", Title = "Curry's Game-Winning 3", Description = "Steph sinks a 35-footer at the buzzer.", MatchDate = DateTime.UtcNow.AddDays(-3), VideoUrl = "https://youtu.be/flm-cZ7r6eE?feature=shared", ImageUrl = "https://i0.wp.com/www.mercurynews.com/wp-content/uploads/2023/10/BNG-L-WARRIORS-1019-11.jpg?fit=620%2C9999px&ssl=1" },
                new Highlight { Id = 8, Sport = "nba", Title = "LeBron's Dunk Over Durant", Description = "LeBron James throws down a massive dunk in crunch time.", MatchDate = DateTime.UtcNow.AddDays(-8), VideoUrl = "https://youtu.be/xYkfmqX0vqk?feature=shared", ImageUrl = "https://landonbuford.com/wp-content/uploads/2023/02/lebron-james-kevin-durant-dunkjpg.jpg" },
                new Highlight { Id = 9, Sport = "nba", Title = "Jokic's No-Look Pass", Description = "Nikola Jokic delivers a stunning no-look assist.", MatchDate = DateTime.UtcNow.AddDays(-12), VideoUrl = "https://youtu.be/WetOYfJ1fGM?feature=shared", ImageUrl = "https://e0.365dm.com/22/04/768x432/skysports-nikola-jokic-nba_5753822.jpg?20220428064759" },

                // NFL Highlights
                new Highlight { Id = 10, Sport = "nfl", Title = "Mahomes 50-Yard TD Pass", Description = "Perfect spiral to Hill in overtime.", MatchDate = DateTime.UtcNow.AddDays(-4), VideoUrl = "https://youtu.be/gytN9r3RpI4?feature=shared", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSXEGKoJAwQApmgMkgouSegmoQ_xvRZA5lzQQ&s" },
                new Highlight { Id = 11, Sport = "nfl", Title = "Lamar Jackson's 70-Yard Run", Description = "Lamar Jackson breaks away for a long TD run.", MatchDate = DateTime.UtcNow.AddDays(-6), VideoUrl = "https://youtu.be/Erm85_8GCw4?feature=shared", ImageUrl = "https://i.dailymail.co.uk/1s/2024/11/08/04/91835829-0-image-a-49_1731040401389.jpg" },
                new Highlight { Id = 12, Sport = "nfl", Title = "Diggs Interception Return", Description = "Stefon Diggs picks off a pass and returns it for a TD.", MatchDate = DateTime.UtcNow.AddDays(-9), VideoUrl = "https://youtu.be/l_eKiQvCtYc?feature=shared", ImageUrl = "https://images.seattletimes.com/wp-content/uploads/2023/01/01012023_Diggs_194706.jpg?d=2040x1360" },

                // CS:GO Highlights
                new Highlight { Id = 13, Sport = "csgo", Title = "s1mple AWP Ace", Description = "s1mple clutches 1v5 in Major.", MatchDate = DateTime.UtcNow.AddDays(-1), VideoUrl = "https://youtu.be/QQIX-ylS7YU?feature=shared", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRZk1te7-GkIkdD25Lv1ts8swOl7inkJA0_oQ&s" },
                new Highlight { Id = 14, Sport = "csgo", Title = "NiKo's Deagle Headshots", Description = "NiKo lands three consecutive headshots in a clutch.", MatchDate = DateTime.UtcNow.AddDays(-3), VideoUrl = "https://youtu.be/cfqYAQ5j8Bg?feature=shared", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTDfpJ59GGAQatCbVGBspDrrWnoFxXMf-8KgQ&s" },
                new Highlight { Id = 15, Sport = "csgo", Title = "ZywOo's 1v3 Clutch", Description = "ZywOo turns the game with a stunning 1v3 play.", MatchDate = DateTime.UtcNow.AddDays(-5), VideoUrl = "https://youtu.be/B-mW-DSfPs8?feature=shared", ImageUrl = "https://i.ytimg.com/vi/-Hj5-IXhY8o/hq720.jpg?sqp=-oaymwE7CK4FEIIDSFryq4qpAy0IARUAAAAAGAElAADIQj0AgKJD8AEB-AHUBoAC4AOKAgwIABABGEsgVChlMA8=&rs=AOn4CLCEYLSqphWglbouZWmap2xDP2Wj_g" }
            };

            // Expanded Schedules Data (upcoming events)
            _schedules = new List<Schedule>
            {
                // F1 Schedules
                new Schedule { Id = 1, Sport = "f1", EventName = "Austrian GP", MatchDate = DateTime.UtcNow.AddDays(7), Teams = "All Teams", Location = "Spielberg" },
                new Schedule { Id = 2, Sport = "f1", EventName = "British GP", MatchDate = DateTime.UtcNow.AddDays(14), Teams = "All Teams", Location = "Silverstone" },
                new Schedule { Id = 3, Sport = "f1", EventName = "Hungarian GP", MatchDate = DateTime.UtcNow.AddDays(21), Teams = "All Teams", Location = "Hungaroring" },

                // Soccer Schedules
                new Schedule { Id = 4, Sport = "soccer", EventName = "Premier League: Man Utd vs Liverpool", MatchDate = DateTime.UtcNow.AddDays(3), Teams = "Man Utd vs Liverpool", Location = "Old Trafford" },
                new Schedule { Id = 5, Sport = "soccer", EventName = "La Liga: Barcelona vs Real Madrid", MatchDate = DateTime.UtcNow.AddDays(5), Teams = "Barcelona vs Real Madrid", Location = "Camp Nou" },
                new Schedule { Id = 6, Sport = "soccer", EventName = "UCL: Bayern Munich vs PSG", MatchDate = DateTime.UtcNow.AddDays(8), Teams = "Bayern Munich vs PSG", Location = "Allianz Arena" },

                // NBA Schedules
                new Schedule { Id = 7, Sport = "nba", EventName = "Lakers vs Warriors", MatchDate = DateTime.UtcNow.AddDays(5), Teams = "Lakers vs Warriors", Location = "Crypto.com Arena" },
                new Schedule { Id = 8, Sport = "nba", EventName = "Celtics vs Nets", MatchDate = DateTime.UtcNow.AddDays(7), Teams = "Celtics vs Nets", Location = "TD Garden" },
                new Schedule { Id = 9, Sport = "nba", EventName = "Nuggets vs Suns", MatchDate = DateTime.UtcNow.AddDays(9), Teams = "Nuggets vs Suns", Location = "Ball Arena" },

                // NFL Schedules
                new Schedule { Id = 10, Sport = "nfl", EventName = "Chiefs vs Eagles", MatchDate = DateTime.UtcNow.AddDays(10), Teams = "Chiefs vs Eagles", Location = "Arrowhead Stadium" },
                new Schedule { Id = 11, Sport = "nfl", EventName = "Ravens vs Steelers", MatchDate = DateTime.UtcNow.AddDays(12), Teams = "Ravens vs Steelers", Location = "M&T Bank Stadium" },
                new Schedule { Id = 12, Sport = "nfl", EventName = "49ers vs Rams", MatchDate = DateTime.UtcNow.AddDays(14), Teams = "49ers vs Rams", Location = "Levi's Stadium" },

                // CS:GO Schedules
                new Schedule { Id = 13, Sport = "csgo", EventName = "ESL Pro League Finals", MatchDate = DateTime.UtcNow.AddDays(8), Teams = "NaVi vs FaZe", Location = "Online" },
                new Schedule { Id = 14, Sport = "csgo", EventName = "BLAST Premier Spring Finals", MatchDate = DateTime.UtcNow.AddDays(10), Teams = "G2 vs Vitality", Location = "London" },
                new Schedule { Id = 15, Sport = "csgo", EventName = "IEM Cologne 2025", MatchDate = DateTime.UtcNow.AddDays(15), Teams = "TBD", Location = "Cologne, Germany" }
            };
        }

        public IEnumerable<string> GetSports()
        {
            return _sports;
        }

        public bool IsValidSport(string sport)
        {
            return _sports.Contains(sport.ToLower());
        }

        public async Task<IEnumerable<Highlight>> GetHighlightsAsync(string sport, int page = 1, int size = 10)
        {
            await Task.Delay(100); // Simulate async DB call
            var highlights = _highlights.Where(h => h.Sport.Equals(sport, StringComparison.OrdinalIgnoreCase));
            return highlights.Skip((page - 1) * size).Take(size);
        }

        public async Task<IEnumerable<Highlight>> GetAllHighlightsAsync(int page = 1, int size = 10)
        {
            await Task.Delay(100); // Simulate async DB call
            var highlights = _highlights
                .OrderBy(h => h.MatchDate)
                .Skip((page - 1) * size)
                .Take(size);
            return highlights;
        }

        public int GetTotalHighlightsCount()
        {
            return _highlights.Count;
        }

        public async Task<IEnumerable<Schedule>> GetScheduleAsync(string sport, int page = 1, int size = 10)
        {
            await Task.Delay(100); // Simulate async DB call
            var schedules = _schedules.Where(s => s.Sport.Equals(sport, StringComparison.OrdinalIgnoreCase));
            return schedules.Skip((page - 1) * size).Take(size);
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync(int page = 1, int size = 10)
        {
            await Task.Delay(100); // Simulate async DB call
            var schedules = _schedules
                .OrderBy(s => s.MatchDate)
                .Skip((page - 1) * size)
                .Take(size);
            return schedules;
        }

        public int GetTotalSchedulesCount()
        {
            return _schedules.Count;
        }
    }
}