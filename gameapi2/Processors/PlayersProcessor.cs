using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using gameapi.Models;
using gameapi.Repositories;
using Newtonsoft.Json;

namespace gameapi.Processors
{
    public class PlayersProcessor
    {
        private readonly IRepository _repository;
        public PlayersProcessor(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Player[]> GetAll()
        {
            return _repository.GetAllPlayers();
        }

        public Task<Player> Get(int id)
        {
            return _repository.GetPlayer(id);
        }
        public Task<Player> GetByName(string name)
        {
            return _repository.GetPlayerByName(name);
        }

        public Task<Player> Create(Player player)
        {
            using (StreamReader r = new StreamReader("data.txt"))
            {
                string json = r.ReadToEnd();
                List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);
                foreach (var p in players)
                    if (p.Id == player.Id)
                    {
                        player.Level = p.Level;
                        player.Score = p.Score;
                        player.Name = p.Name;
                        player.Powerups = p.Powerups;
                        player.Kills = p.Kills;
                        player.Deaths = p.Deaths;
                        player.Wins = p.Wins;
                        player.Losses = p.Losses;
                        player.Matches = p.Matches;
                        player.Accuracy = p.Accuracy;
                        if (player.Deaths > 0)
                            player.KDRatio = ((float)player.Kills / (float)player.Deaths);
                        else
                            player.KDRatio = player.Kills;

                        if (player.Losses > 0)
                            player.WinRatio = ((float)player.Wins / (float)player.Losses);
                        else
                            player.WinRatio = player.Wins;
                        if (player.Matches > 0)
                            player.KillsPerMatchRatio = ((float)player.Kills / (float)player.Matches);
                        else
                            player.KillsPerMatchRatio = player.Kills;
                        foreach (var item in player.Powerups)
                        {
                            player.Pickups += item.count;
                        }
                    }
            }
            return _repository.CreatePlayer(player);
        }
        public async Task<Player[]> GetTopTen()
        {
            return await _repository.GetTopTen();
        }

        public async Task<Player> UpdatePlayerNameAndScore(string name, string newName, int score)
        {
            return await _repository.UpdatePlayerNameAndScore(name, newName, score);
        }
        public Task<Player> Delete(int id)
        {
            return _repository.DeletePlayer(id);
        }


        public async Task<Player> Update(int id, Powerup powerup)
        {
            Player player = await _repository.GetPlayer(id);
            foreach (var item in player.Powerups)
            {
                if (item.PowerupName == powerup.PowerupName)
                {
                    item.count += powerup.count;
                    await _repository.UpdatePlayer(player);
                }

            }
            if (powerup.PowerupName != null)
            {
                player.Powerups.Add(powerup);
                await _repository.UpdatePlayer(player);
            }

            return player;
        }
        public async Task<Player[]> GetTopTenAccuracy()
        {
            return await _repository.GetTopTenAccuracy();
        }
        public async Task<Player[]> GetTopTenMatches()
        {
            return await _repository.GetTopTenMatchRatio();
        }
        public async Task<Player[]> GetTopTenKDR()
        {
            return await _repository.GetTopTenDeathRatio();
        }
        public async Task<Player[]> GetTopTenPickups()
        {
            return await _repository.GetTopTenPickups();
        }
        public async Task<Player[]> GetTopTenKillMatch()
        {
            return await _repository.GetTopTenKillMatch();
        }
    }
}