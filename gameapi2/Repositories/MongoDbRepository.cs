using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using gameapi.Models;
using gameapi.mongodb;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gameapi.Repositories
{
    //Gets from and updates data to MongoDb 
    public class MongoDbRepository : IRepository
    {
        private IMongoCollection<Player> _collection;
        public MongoDbRepository(MongoDBClient client)
        {
            //Getting the database with name "game"
            IMongoDatabase database = client.GetDatabase("game");

            //Getting collection with name "players"
            _collection = database.GetCollection<Player>("players");
        }
        public async Task<Player> CreatePlayer(Player player)
        {
            await _collection.InsertOneAsync(player);
            return player;
        }

        public async Task<Player> DeletePlayer(int playerId)
        {
            var result = await _collection.FindAsync(a => a.Id == playerId);
            var player = await result.FirstAsync();
            await _collection.DeleteOneAsync(a => a.Id == playerId);
            return player;
        }

        public async Task<Player[]> GetAllPlayers()
        {
            var countfilter = Builders<Player>.Filter.Empty;
            int playerCount = (int)_collection.Count(countfilter);
            int counter = 0;
            Player[] player = new Player[playerCount];

            var filter = Builders<Player>.Filter.Empty;
            var cursor = await _collection.FindAsync(filter);
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<Player> batch = cursor.Current;
                foreach (Player document in batch)
                {
                    player[counter] = document;
                    counter++;
                }

            }
            return player;
        }

        public async Task<Player> GetPlayer(int playerId)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var cursor = await _collection.FindAsync(filter);
            var player = await cursor.FirstAsync();
            return player;
        }
        public async Task<Player> GetPlayerByName(string name)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Name, name);
            var cursor = await _collection.FindAsync(filter);
            var player = await cursor.FirstAsync();
            return player;
        }

        public async Task<Player[]> GetTopTen()
        {
            var filter = Builders<Player>.Filter.Empty;
            var list = await _collection.Find(filter).Sort(Builders<Player>.Sort.Descending("Score")).ToListAsync();
            Player[] player = new Player[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                player[i] = list[i];
                if (i == 9)
                    break;
            }
            return player;
        }

        public async Task<Player> UpdatePlayer(Player player)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
            await _collection.ReplaceOneAsync(filter, player);
            return player;
        }

        public async Task<Player> UpdatePlayerNameAndScore(string name, string newName, int score)
        {
            FilterDefinition<Player> filter1;
            if (newName != null)
            {
                var filter = Builders<Player>.Filter.Eq(p => p.Name, name);
                var update = Builders<Player>.Update.Set("Name", newName).Set("Score", score);
                await _collection.UpdateOneAsync(filter, update);

                filter1 = Builders<Player>.Filter.Eq(p => p.Name, newName);
                var cursor = await _collection.FindAsync(filter1);
                var player = await cursor.FirstAsync();
                return player;
            }
            else if (score != 0)
            {
                var filter = Builders<Player>.Filter.Eq(p => p.Name, name);
                var update = Builders<Player>.Update.Set("Score", score);
                await _collection.UpdateOneAsync(filter, update);

                var cursor = await _collection.FindAsync(filter);
                var player = await cursor.FirstAsync();
                return player;
            }
            return null;

        }
    }
}
