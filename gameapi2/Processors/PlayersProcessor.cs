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
                    return player;
                }

            }
            player.Powerups.Add(powerup);
            await _repository.UpdatePlayer(player);
            return player;
        }
    }
}