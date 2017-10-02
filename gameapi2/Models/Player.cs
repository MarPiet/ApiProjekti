using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gameapi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public List<Powerup> Powerups {get; set;}
      

    }

    public class NewPlayer
    {
        [Required]
        public string Name { get; set; }
    }

    public class ModifiedPlayer
    {
        [Required]
        public int Level { get; set; }
    }
    public class Powerup
    {
        public string PowerupName {get; set;}
        public int count {get; set;}
        

    }
}