using System;
using System.Threading.Tasks;
using gameapi.Models;

namespace gameapi.Repositories
{
    public interface IRepository
    {

        Task<Player> CreatePlayer(Player player);
        Task<Player> GetPlayer(int playerId);
        Task<Player> GetPlayerByName(string name);
        Task<Player[]> GetAllPlayers();
        Task<Player[]> GetTopTen();
        Task<Player[]> GetTopTenAccuracy();
        Task<Player[]> GetTopTenMatchRatio();
        Task<Player[]> GetTopTenDeathRatio();
        Task<Player[]> GetTopTenPickups();
         Task<Player[]> GetTopTenKillMatch();
        Task<Player> UpdatePlayerNameAndScore(string name, string newName, int score);
        Task<Player> DeletePlayer(int playerId);
        Task<Player> UpdatePlayer(Player player);



    }
}