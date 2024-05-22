﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Monster : LivingCreature
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold {  get; set; }
        public List<LootItem> LootTable { get; set; } //List of what the monster can potentially drop 
        public Element ElementWeakness  { get; set; }
        public Monster(int id, string name, int maximumDamage, int rewardExperiencePoints, int rewardGold, int currentHitPoints, 
            int maximumHitPoints, int currentManaPoints, int maximumManaPoints, Element elementWeakness) 
            : base(currentHitPoints, maximumHitPoints, currentManaPoints, maximumManaPoints)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            LootTable = new List<LootItem>(); //initialise the list
            ElementWeakness = elementWeakness;
        }

    }
}
