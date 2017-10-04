using System;
using System.Threading.Tasks;
using gameapi.Exceptions;
using gameapi.Models;
using gameapi.ModelValidation;
using gameapi.Processors;
using Microsoft.AspNetCore.Mvc;

namespace gameapi.Controllers
{

    [Route("api/players")]
    public class PlayersController : Controller
    {
        private PlayersProcessor _processor;
        public PlayersController(PlayersProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        public Task<Player[]> GetAll()
        {
            return _processor.GetAll();
        }

        [HttpGet("{id:int}")]
        public Task<Player> Get(int id)
        {
            return _processor.Get(id);
        }

        [HttpGet("{name}")]
        public Task<Player> Get(string name)
        {
            return _processor.GetByName(name);
        }

        [HttpGet("toptenaccuracy")]
        public Task<Player[]> GetTopTenAccuracy()
        {
            return _processor.GetTopTenAccuracy();
        }

        [HttpGet("toptenmatches")]
        public Task<Player[]> GetTopTenMatches()
        {
            return _processor.GetTopTenMatches();
        }

        [HttpGet("toptenkdr")]
        public Task<Player[]> GetTopTenKDR()
        {
            return _processor.GetTopTenKDR();
        }

        [HttpGet("toptenpickups")]
        public Task<Player[]> GetTopTenPickups()
        {
            return _processor.GetTopTenPickups();
        }
        [HttpGet("toptenkillmatch")]
        public Task<Player[]> GetTopTenKillsPerMatch()
        {
            return _processor.GetTopTenKillMatch();
        }

        [HttpPut("{name:minlength(1)}")]
        public Task<Player> UpdatePlayerNameAndScore(string name, string newName, int score)
        {
            return _processor.UpdatePlayerNameAndScore(name, newName, score);
        }

        [HttpPut("{id:int}")]
        public Task<Player> Update(int id, Powerup powerup)
        {
            return _processor.Update(id, powerup);
        }

        [HttpDelete("{id}")]
        public Task<Player> Delete(int id)
        {
            return _processor.Delete(id);
        }

        [HttpPost]
        [ValidateModel]
        public Task<Player> Create(Player player)
        {
            return _processor.Create(player);
        }

    }
}