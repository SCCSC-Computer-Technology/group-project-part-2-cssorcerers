using IQSport.Data.DbContext;
using IQSport.Models;
using IQSport.Models.SportModels.Models;
using IQSport.Models.SportModels.NBA.Models;
using IQSport.Models.SportsModels.NBA.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsData.Services
{
    public class NBAServices
    {
        public static async Task<List<NBATeamSeasonInfo>> GetNBATeamSeasonInfoList(int season, string? sortOrderStr, int page,
            int itemsPerPage, List<NBATeamSeasonStat> teamSeasonStats, SportsDbContext context)
        {

            List<NBATeam> teams = await context.NBATeam.Where(x => teamSeasonStats.Select(y => y.TeamID).Contains(x.ID)).OrderBy(x => x.ID).ToListAsync();

            List<NBATeamSeasonInfo> teamInfo = new List<NBATeamSeasonInfo>();
            foreach (NBATeam team in teams)
            {

                var seasonStat = teamSeasonStats.Where(stat => stat.TeamID == team.ID && (season == -1 || stat.Season == season)).ToList();

                foreach (NBATeamSeasonStat stat in seasonStat)
                {
                    teamInfo.Add(new NBATeamSeasonInfo
                    {
                        TeamInfo = team,
                        TeamStats = stat
                    });
                }
            }

            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.Season).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fg_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.FieldGoals).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fga_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.FieldGoalAttempts).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "3pt_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.ThreePoints).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "3pta_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.ThreePointAttempts).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.TwoPoints).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pta_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.TwoPointAttempts).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ft_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.FreeThrows).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fta_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.FreeThrowAttempts).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "offr_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.OffensiveRebounds).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "defr_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.DefesniveRebounds).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ast_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.Assists).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "stl_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.Steals).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "blk_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.Blocks).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tnvr_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.Turnovers).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "pfl_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.PersonalFouls).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tpt_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TeamStats.TotalPoints).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.Season).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fg_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.FieldGoals).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fga_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.FieldGoalAttempts).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "3pt_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.ThreePoints).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "3pta_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.ThreePointAttempts).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.TwoPoints).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pta_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.TwoPointAttempts).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ft_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.FreeThrows).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fta_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.FreeThrowAttempts).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "offr_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.OffensiveRebounds).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "defr_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.DefesniveRebounds).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ast_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.Assists).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "stl_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.Steals).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "blk_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.Blocks).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tnvr_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.Turnovers).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "pfl_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.PersonalFouls).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tpt_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TeamStats.TotalPoints).ThenBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                default:
                    teamInfo = teamInfo.OrderBy(x => x.TeamInfo.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
            }
            return teamInfo;
        }

        public static async Task<List<NBAPlayerInfo>> GetNBAPlayerInfoList(bool? isActive, int teamID, string? sortOrderStr, int page,
               int itemsPerPage, SportsDbContext context)
        {

            IQueryable<NBAPlayer> players;
            if (isActive == null)
            {

                players = context.NBAPlayer.Where(x => teamID == -1 || x.TeamID == teamID)
                        .OrderByDescending(x => x.ID).ThenBy(x => x.TeamID);
            }
            else
            {
                players = context.NBAPlayer.Where(x => x.IsActive == isActive && (teamID == -1 || x.TeamID == teamID))
                         .OrderByDescending(x => x.ID).ThenBy(x => x.TeamID);
            }


            List<NBAPlayerInfo> playerInfo = new List<NBAPlayerInfo>();
            string[] teams = await context.NBATeam.Select(x => x.Name).ToArrayAsync();
            NBAPlayerCareerStat[] stats = await context.NBAPlayerCareerStat.ToArrayAsync();
            foreach (NBAPlayer player in players)
            {
                NBAPlayerInfo info = new NBAPlayerInfo()
                {
                    NBAPlayer = player,
                    TeamName = (player.TeamID.HasValue) ? teams[player.TeamID.Value - 1] : null,
                    CareerStats = stats.Where(x => x.PlayerID == player.ID).SingleOrDefault()
                };
                playerInfo.Add(info);
            }


            switch (sortOrderStr?.ToLower())
            {
                case "name_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.NBAPlayer.Name).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fg_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.FieldGoals).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fga_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.FieldGoalAttempts).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "3pt_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.ThreePoints).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "3pta_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.ThreePointAttempts).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pt_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.TwoPoints).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pta_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.TwoPointAttempts).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ft_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.FreeThrows).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fta_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.FreeThrowAttempts).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "offr_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.OffensiveRebounds).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "defr_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.DefesniveRebounds).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ast_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.Assists).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "stl_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.Steals).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "blk_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.Blocks).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tnvr_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.Turnovers).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "pfl_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.PersonalFouls).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tpt_desc":
                    playerInfo = playerInfo.OrderByDescending(x => x.CareerStats.TotalPoints).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "name_asc":
                    playerInfo = playerInfo.OrderBy(x => x.NBAPlayer.Name).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fg_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.FieldGoals).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fga_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.FieldGoalAttempts).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "3pt_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.ThreePoints).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "3pta_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.ThreePointAttempts).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pt_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.TwoPoints).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pta_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.TwoPointAttempts).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ft_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.FreeThrows).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fta_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.FreeThrowAttempts).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "offr_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.OffensiveRebounds).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "defr_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.DefesniveRebounds).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ast_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.Assists).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "stl_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.Steals).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "blk_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.Blocks).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tnvr_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.Turnovers).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "pfl_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.PersonalFouls).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tpt_asc":
                    playerInfo = playerInfo.OrderBy(x => x.CareerStats.TotalPoints).ThenBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                default:
                    playerInfo = playerInfo.OrderBy(x => x.NBAPlayer.ID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
            }


            return playerInfo;
        }

    }
}
