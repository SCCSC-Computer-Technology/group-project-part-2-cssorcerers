using Microsoft.EntityFrameworkCore;
using SportsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsData.Models;

namespace SportsData.Services
{
    public class NFLServices
    {
        public static async Task<List<NFLTeamSeasonInfo>> GetNFLTeamSeasonInfo(int season, string? sortOrderStr, int page,
            int itemsPerPage, List<NFLTeamSeasonStat> teamSeasonStats, SportsDbContext context)
        {
           

            List<NFLTeam> teams = await context.NFLTeam.Where(x => teamSeasonStats.Select(y => y.ID).Contains(x.ID)).OrderBy(x => x.ID).ToListAsync();

            List<NFLTeamSeasonInfo> teamInfo = new List<NFLTeamSeasonInfo>();
            foreach (NFLTeam team in teams)
            {
                
                var seasonStat = teamSeasonStats.Where(stat => stat.ID == team.ID && (season == -1 || stat.Season == season)).ToList();
                
                foreach (NFLTeamSeasonStat stat in seasonStat)
                {
                    teamInfo.Add(new NFLTeamSeasonInfo
                    {
                        TeamID = team.ID,
                        TeamName = team.Name,
                        Season = stat.Season,
                        Touchdowns = stat.TotalTouchdowns,
                        ExtraPoints = stat.ExtraPointsMade,
                        TwoPoints = stat.TwoPoints,
                        FieldGoals = stat.FieldGoalsMade,
                        Wins = stat.Wins,
                        Losses = stat.Losses,
                        Ties = stat.Ties
                    });
                }
            }

            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Season).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "td_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Touchdowns).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TwoPoints).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "xpt_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.ExtraPoints).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fg_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.FieldGoals).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "win_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Wins).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "loss_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Losses).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tie_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Ties).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Season).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "td_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Touchdowns).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TwoPoints).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "xpt_asc":
                    teamInfo = teamInfo.OrderBy(x => x.ExtraPoints).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fg_asc":
                    teamInfo = teamInfo.OrderBy(x => x.FieldGoals).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "win_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Wins).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "loss_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Losses).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tie_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Ties).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                default:
                    teamInfo = teamInfo.OrderByDescending(x => x.Season).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
            }
            return teamInfo;
        }
    }
}
