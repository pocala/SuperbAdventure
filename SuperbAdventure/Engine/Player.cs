﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;
using System.Collections;

namespace Engine
{
    public class Player : LivingCreature
    {
        private int _gold;
        private int _experiencePoints;
        private int _level;
        private Location _currentLocation;
        private Monster _currentMonster;
        public event EventHandler<MessageEventArgs> OnMessage; //Sends an event notification with MessageEventArgs object
        //Object with the message to be displayed
        public int Gold
        {
            get
            {
                return _gold;
            }
            set //Set is to set a value for that property initially
            {
                _gold = value;
                OnPropertyChanged("Gold");
            }
        }
        public int ExperiencePoints
        {
            get
            {
                return _experiencePoints;
            }
            set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePoints");
            }
        }

        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                OnPropertyChanged("Level");
            }
        }
        public Location CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                _currentLocation = value;
                OnPropertyChanged("CurrentLocation");
            }
        }
        public Monster CurrentMonster
        {
            get
            {
                return _currentMonster;
            }
            set
            {
                _currentMonster = value;
                OnPropertyChanged("CurrentMonster");
            }
        }
        public Weapon CurrentWeapon { get; set; }
        public BindingList<InventoryItem> Inventory { get; set; }

        public BindingList<PlayerQuest> Quests { get; set; } //Stores the list of quests the player has on hand
        public BindingList<PlayerSpell> LearntSpells { get; set; } //List of spells that player has learnt
        public List<Weapon> Weapons
        {
            get { return Inventory.Where(x => x.Details is Weapon).Select(x => x.Details as Weapon).ToList(); }
        } //Select (x => x.Details) only returns the Details property of the InventoryItem object

        public List<HealingPotion> HealingPotions
        {
            get { return Inventory.Where(x => x.Details is HealingPotion).Select(x => x.Details as HealingPotion).ToList(); }
        }

        public List<Spell> Spells //to display the list of spells for the cboSpells
        {
            get { return LearntSpells.Where(x => x.Details is Spell).Select(x => x.Details as Spell).ToList(); }
        }

        private Player(int gold, int experiencePoints, int currentHitPoints, int maximumHitPoints, int currentManaPoints, int maximumManaPoints) 
            : base(currentHitPoints, maximumHitPoints, currentManaPoints, maximumManaPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Inventory = new BindingList<InventoryItem>(); //initialise the inventory
            Quests = new BindingList<PlayerQuest>();
            LearntSpells = new BindingList<PlayerSpell>();
            _level = 1; //start from level 1
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(100, 0, 20, 20,20,20);
            player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_BARE_FISTS), 1));
            player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);
            return player;
        }

        public void AddExperience(int expGain) //Adds experience points to the player
        {
            ExperiencePoints += expGain;
            CalculatePlayerLevel(ExperiencePoints);
        }

        public double ExperienceRequiredToLevel(int currentLevel) //Setting the threshold to level up
        {
            return ((Math.Pow(currentLevel, 3) * 100) / 50);
        }

        public int CalculatePlayerLevel(int currentExperiencePoints) //Calculates level based on the player current experience points
        {
            if (currentExperiencePoints >= ExperienceRequiredToLevel(_level))
            {
                LevelUp();
            }
            return _level;
        }
        public void LevelUp()
        {
            MaximumHitPoints += 5;
            CurrentHitPoints = MaximumHitPoints;
            _level++;
            RaiseMessage("You levelled up!");
            CheckIfPlayerLearnSpells(_level); 
        }

        public void CheckIfPlayerLearnSpells(int currentLevel) //checks if player learn spells based on current level
        {
            foreach (Spell spell in World.Spells)
            {
                if (currentLevel == spell.LevelRequiredToLearn && !LearntSpells.Any(s => s.Details.ID == spell.ID))
                {
                    LearntSpells.Add(new PlayerSpell(spell));
                    RaiseMessage("You learnt " + spell.Name);
                    MarkSpellAsLearnt(spell); //marks the spell to learn as learnt
                    OnPropertyChanged("Spells"); //raise property change for Spells list
                }
            }
        }
        public void MarkSpellAsLearnt(Spell spellLearnt)
        {
            PlayerSpell spellToAddAsLearnt = LearntSpells.SingleOrDefault(s => s.Details.ID == spellLearnt.ID);
            if (spellToAddAsLearnt != null && spellToAddAsLearnt.IsLearnt == false)
            {
                spellToAddAsLearnt.IsLearnt = true;
            }
        }
        public bool HasRequiredItemToEnterLocation(Location location) //To check if the player has the item to enter an area
        {
            if (location.ItemRequiredToEnter == null)
            {
                return true;
            }
            return Inventory.Any(ii => ii.Details.ID == location.ItemRequiredToEnter.ID);
        }

        public bool HasThisQuest(Quest quest) //Checks if the player already has this quest
        {
            return Quests.Any(pq => pq.Details.ID == quest.ID);
        }

        public bool CompletedThisQuest(Quest quest) //Checks to see if the player has completed this quest 
        {
            foreach (PlayerQuest playerquest in Quests)
            {
                if (playerquest.Details.ID == quest.ID)
                {
                    return playerquest.IsCompleted;
                }
            }
            return false;
        }
        public bool HasAllRequiredItemsToClearQuest(Quest quest) //Checks to see if the player has all items to clear the quest of a location
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems) //Check what are the location's quest required items
            {
                if (!Inventory.Any(ii => ii.Details.ID == qci.Details.ID && ii.Quantity >= qci.Quantity))
                {
                    return false;
                }
            }
            return true;
        }
        public void RemoveQuestItemsFromInventory(Quest quest) //Removes the items from player inventory for the quest
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == qci.Details.ID);
                if (item != null)
                {
                    RemoveItemFromInventory(item.Details, qci.Quantity);
                }
            }
        }
        public void AddItemToInventory(Item itemToAdd, int quantity = 1) //To add a specific quantity
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToAdd.ID);
            if (item == null)
            {
                Inventory.Add(new InventoryItem(itemToAdd, quantity));
            }
            else
            {
                item.Quantity += quantity;
            }
            RaiseInventoryChangedEvent(itemToAdd);
        }

        public void MarkQuestAsCompleted(Quest quest)
        {
            PlayerQuest playerQuest = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);
            if (playerQuest != null)
            {
                playerQuest.IsCompleted = true;
            }
        }
        private void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            if (OnMessage != null)
            {
                OnMessage(this, new MessageEventArgs(message, addExtraNewLine));
            }
        }

        public string ToXmlString()
        {
            XmlDocument playerData = new XmlDocument();

            //Creating the player node
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);

            //Creating the "Stats" child node to hold the player's statistics nodes
            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats); //Append it to the XmlDocument

            //Creating the child nodes for the "Stats" node 
            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");
            currentHitPoints.AppendChild(playerData.CreateTextNode(this.CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(playerData.CreateTextNode(this.MaximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.Gold.ToString()));
            stats.AppendChild(gold);

            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(this.ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode level = playerData.CreateElement("Level");
            level.AppendChild(playerData.CreateTextNode(this.Level.ToString()));
            stats.AppendChild(level);

            XmlNode maximumManaPoints = playerData.CreateElement("MaximumManaPoints");
            maximumManaPoints.AppendChild(playerData.CreateTextNode(this.MaximumManaPoints.ToString()));
            stats.AppendChild(maximumManaPoints);

            XmlNode currentManaPoints = playerData.CreateElement("CurrentManaPoints");
            currentManaPoints.AppendChild(playerData.CreateTextNode(this.CurrentManaPoints.ToString()));
            stats.AppendChild(currentManaPoints);

            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.CurrentLocation.ID.ToString()));
            stats.AppendChild(currentLocation);

            if (CurrentWeapon != null)
            {
                XmlNode currentWeapon = playerData.CreateElement("CurrentWeapon");
                currentWeapon.AppendChild(playerData.CreateTextNode(this.CurrentWeapon.ID.ToString()));
                stats.AppendChild(currentWeapon);
            }

            //Creating "InventoryItems" child node to hold each InventoryItem node
            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);

            //Creating an "InventoryItem" node for each item in the player's inventory
            foreach (InventoryItem item in this.Inventory)
            {
                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = item.Details.ID.ToString();
                inventoryItem.Attributes.Append(idAttribute);

                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = item.Quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);

                inventoryItems.AppendChild(inventoryItem);
            }

            //Create a "PlayerQuests" node to hold each PlayerQuest node
            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);

            //Creating a "PlayerQuest" node for each item in the player's quest list
            foreach (PlayerQuest quest in this.Quests)
            {
                XmlNode playerQuest = playerData.CreateElement("PlayerQuest");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = quest.Details.ID.ToString();
                playerQuest.Attributes.Append(idAttribute);

                XmlAttribute isCompletedAttribute = playerData.CreateAttribute("IsCompleted");
                isCompletedAttribute.Value = quest.IsCompleted.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);

                playerQuests.AppendChild(playerQuest);
            }

            //Create a "LearntSpells" node for each spell learnt by the player
            XmlNode playerSpells = playerData.CreateElement("PlayerSpells");
            player.AppendChild(playerSpells);

            //Creating a "LearntSpell" node for each spell 
            foreach (PlayerSpell spell in this.LearntSpells)
            {
                XmlNode playerSpell = playerData.CreateElement("PlayerSpell");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = spell.Details.ID.ToString();
                playerSpell.Attributes.Append(idAttribute);

                XmlAttribute isLearntAttribute = playerData.CreateAttribute("IsLearnt");
                isLearntAttribute.Value = spell.IsLearnt.ToString();
                playerSpell.Attributes.Append(isLearntAttribute);

                playerSpells.AppendChild(playerSpell);
            }
            return playerData.InnerXml;
        }

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();
                playerData.LoadXml(xmlPlayerData);

                int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);
                int level = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Level").InnerText);
                int currentManaPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentManaPoints").InnerText);
                int maximumManaPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumManaPoints").InnerText);

                Player player = new Player(gold, experiencePoints, currentHitPoints, maximumHitPoints, currentManaPoints, maximumManaPoints); //Creates a player based on the values from xml document
                player.Level = level;

                int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                player.CurrentLocation = World.LocationByID(currentLocationID);

                if (playerData.SelectSingleNode("/Player/Stats/CurrentWeapon") != null)
                {
                    int currentWeaponID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);
                    player.CurrentWeapon = (Weapon)World.ItemByID(currentWeaponID);
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    for (int i = 0; i < quantity; i++)
                    {
                        player.AddItemToInventory(World.ItemByID(id));
                    }
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);

                    PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(id));
                    playerQuest.IsCompleted = isCompleted;

                    player.Quests.Add(playerQuest);
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/PlayerSpells/PlayerSpell"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isLearnt = Convert.ToBoolean(node.Attributes["IsLearnt"].Value);

                    PlayerSpell playerSpell = new PlayerSpell(World.SpellByID(id));
                    playerSpell.IsLearnt = isLearnt;

                    player.LearntSpells.Add(playerSpell);
                }
                return player;
            }
            catch
            {
                return Player.CreateDefaultPlayer();
            }
        }

        public static Player CreatePlayerFromDatabase(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int currentLocationID, int currentManaPoints, int maximumManaPoints)
        {
            Player player = new Player(currentHitPoints, maximumHitPoints, gold, experiencePoints, currentManaPoints, maximumManaPoints);
            player.MoveTo(World.LocationByID(currentLocationID));
            return player;
        }

        private void RaiseInventoryChangedEvent(Item item)
        {
            if (item is Weapon)
            {
                OnPropertyChanged("Weapons");
            }
            if (item is HealingPotion)
            {
                OnPropertyChanged("HealingPotions");
            }
        }
        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToRemove.ID);
            if (item == null)
            {
                //Item not in player inventory, may raise an error for this part
            }
            else
            {
                item.Quantity -= quantity;
                if (item.Quantity < 0)
                {
                    item.Quantity = 0;
                }
                if (item.Quantity == 0)
                {
                    Inventory.Remove(item);
                }
                RaiseInventoryChangedEvent(itemToRemove); //Notifies the UI that inventory has changed for weapon and potion
            }
        }

        public void MoveNorth()
        {
            if (CurrentLocation.LocationToNorth != null)
            {
                MoveTo(CurrentLocation.LocationToNorth);
            }
        }
        public void MoveEast()
        {
            if (CurrentLocation.LocationToEast != null)
            {
                MoveTo(CurrentLocation.LocationToEast);
            }
        }
        public void MoveSouth()
        {
            if (CurrentLocation.LocationToSouth != null)
            {
                MoveTo(CurrentLocation.LocationToSouth);
            }
        }
        public void MoveWest()
        {
            if (CurrentLocation.LocationToWest != null)
            {
                MoveTo(CurrentLocation.LocationToWest);
            }
        }
        public void MoveTo(Location newLocation)
        {

            //Does the location have any required items
            if (!HasRequiredItemToEnterLocation(newLocation))
            {
                RaiseMessage("You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location.");
                return; //to break the function, otherwise the player can still proceed even without the item
            }
            CurrentLocation = newLocation;
            if (newLocation.QuestAvailableHere != null) //Check if location has quests and if it does, call handlequest function
            {
                HandleQuest(newLocation);
            }
            if (newLocation.MonsterLivingHere != null)
            {
                MonsterAppears(newLocation);
            }
            else if (newLocation.MonsterLivingHere == null)
            {
                _currentMonster = null;
            }
        }
        private void MonsterAppears(Location location)
        {
            RaiseMessage("You see a " + location.MonsterLivingHere.Name);

            Monster standardMonster = World.MonsterByID(location.MonsterLivingHere.ID); //make a new instance instead of _currentMonster = location.MonsterLivingHere
            _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints,
                standardMonster.CurrentManaPoints,standardMonster.MaximumManaPoints, standardMonster.ElementWeakness); //then add it to the private instance

            foreach (LootItem lootitem in standardMonster.LootTable)
            {
                _currentMonster.LootTable.Add(lootitem); //Add the loottable to the current monster as well
            }
        }
        private void MonsterAttacks()
        {
            RaiseMessage("The " + _currentMonster.Name + " attacked! ");
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);
            CurrentHitPoints -= damageToPlayer;
            RaiseMessage("The " + _currentMonster.Name + " hit you for " + damageToPlayer.ToString() + "points of damage", true);

            //lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            if (CurrentHitPoints <= 0)
            {
                PlayerDies();
            }
        }
        private void MonsterIsDefeated()
        {
            RaiseMessage("You defeated the " + _currentMonster.Name, true);
            Gold += _currentMonster.RewardGold;
            AddExperience(_currentMonster.RewardExperiencePoints);
            AddMonsterLootToInventory();
            CurrentMonster = null;
        }
        private void AddMonsterLootToInventory()
        {
            List<InventoryItem> lootedItem = new List<InventoryItem>(); //Create a list to temporarily store looted items from the monster
            foreach (LootItem lootItem in _currentMonster.LootTable)
            {
                if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                {
                    lootedItem.Add(new InventoryItem(lootItem.Details, 1));
                }
            }
            if (lootedItem.Count == 0) //If nothing is looted, give the default item
            {
                foreach (LootItem lootItem in _currentMonster.LootTable)
                {
                    if (lootItem.IsDefaultItem)
                    {
                        lootedItem.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }
            }
            foreach (InventoryItem inventoryItem in lootedItem)
            {
                AddItemToInventory(inventoryItem.Details);
                if (inventoryItem.Quantity == 1)
                {
                    RaiseMessage("You looted " + inventoryItem.Quantity + " " + inventoryItem.Details.Name, true);

                }
                else
                {
                    RaiseMessage("You looted " + inventoryItem.Quantity + " " + inventoryItem.Details.NamePlural, true);
                }

            }
        }
        private void PlayerDies()
        {
            RaiseMessage("You died to the " + _currentMonster.Name + "!", true);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            CurrentHitPoints = MaximumHitPoints; //Make to max hp
            CurrentManaPoints = MaximumManaPoints;
        }

        public void UseWeapon(Weapon currentWeapon)
        {
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            _currentMonster.CurrentHitPoints -= damageToMonster;
            RaiseMessage("You dealt " + damageToMonster.ToString() + " points of damage to the " + _currentMonster.Name, true);
            if (_currentMonster.CurrentHitPoints <= 0)
            {
                MonsterIsDefeated();
            }
            else
            {
                MonsterAttacks();
            }
        }

        public void UsePotion(HealingPotion potionToUse)
        {
            int healingAmount = potionToUse.HealingAmount;
            RaiseMessage("You drank a " + potionToUse.Name + "for " + healingAmount.ToString() + "points of health", true);
            CurrentHitPoints += healingAmount;

            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
            RemoveItemFromInventory(potionToUse, 1);
            MonsterAttacks();
        }

        public void UseSpell(Spell currentSpell)
        {
            if (CurrentManaPoints >= currentSpell.ManaCost)
            {
                RaiseMessage("You cast " + currentSpell.Name);
                int damageToMonster = SpellElementalDamageCheck(currentSpell);
                _currentMonster.CurrentHitPoints -= damageToMonster;
                RaiseMessage("You dealt " + damageToMonster.ToString() + " points of damage to the " + _currentMonster.Name, true);
                CurrentManaPoints -= currentSpell.ManaCost;
                if (_currentMonster.CurrentHitPoints <= 0)
                {
                    MonsterIsDefeated();
                }
                else
                {
                    MonsterAttacks();
                }
            }
            else //when the player does not have enough mana points
            {
                RaiseMessage("You do not have enough mana to cast this spell!");
            }
        }

        public int SpellElementalDamageCheck(Spell currentSpell)
        {
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentSpell.MinimumDamage, currentSpell.MaximumDamage);
            if (currentSpell.Element == _currentMonster.ElementWeakness)
            {
                damageToMonster = damageToMonster * 2;
                RaiseMessage("Its really effective! The " + _currentMonster.Name + " seems to be weak to " + currentSpell.Element);
            }
            return damageToMonster;
        }
        private void CheckPlayerManaPointsForSpells(int playerManaPoints, Spell currentSpell)
        {
            if (playerManaPoints >= currentSpell.ManaCost)
            {
                CurrentManaPoints -= currentSpell.ManaCost;
            }
            else if (playerManaPoints <= currentSpell.ManaCost)
            {
                RaiseMessage("You do not have enough mana to cast this spell!");
            }
        }
        public void HandleQuest(Location location) //To handle if player has completed quests and reward the player if the quest is done
        {
            if (location.QuestAvailableHere != null) //Check if location has quests
            {
                bool playerAlreadyHasQuest = HasThisQuest(location.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = CompletedThisQuest(location.QuestAvailableHere);

                if (playerAlreadyHasQuest)
                {
                    if (!playerAlreadyCompletedQuest)
                    {
                        bool playerHasRequiredItemsToCompleteQuest = HasAllRequiredItemsToClearQuest(location.QuestAvailableHere); //See if player has all items   
                        if (playerHasRequiredItemsToCompleteQuest)
                        {
                            HandleQuestCompleted(location);
                            //UpdatePlayerStats();
                        }
                    }
                }
                else //Player does not have the quest
                {
                    HandleQuestReceived(location);
                }
            }
        }

        private void HandleQuestReceived(Location location)
        {
            RaiseMessage("You received a quest: " + location.QuestAvailableHere.Name, true);
            RaiseMessage(location.QuestAvailableHere.Description, true);
            foreach (QuestCompletionItem qci in location.QuestAvailableHere.QuestCompletionItems)
            {
                if (qci.Quantity == 1)
                {
                    RaiseMessage(qci.Quantity.ToString() + " " + qci.Details.Name + " " + "is required to complete this quest", true);
                }
                else
                {
                    RaiseMessage(qci.Quantity.ToString() + " " + qci.Details.NamePlural + " " + "are required to complete this quest", true);
                }
            }
            RaiseMessage(" ", true);
            Quests.Add(new PlayerQuest(location.QuestAvailableHere));
        }

        private void HandleQuestCompleted(Location location)
        {

            RaiseMessage("You completed " + location.QuestAvailableHere.Name, true);
            RemoveQuestItemsFromInventory(location.QuestAvailableHere);

            RaiseMessage("You received " + location.QuestAvailableHere.RewardGold.ToString() + " gold", true);
            RaiseMessage("You received " + location.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points", true);
            RaiseMessage("You received " + location.QuestAvailableHere.RewardItem.Name, true);

            //Give quest rewards
            Gold += location.QuestAvailableHere.RewardGold;
            ExperiencePoints += location.QuestAvailableHere.RewardExperiencePoints;
            AddItemToInventory(location.QuestAvailableHere.RewardItem);

            //Mark the quest as completed
            MarkQuestAsCompleted(location.QuestAvailableHere);
        }

    }
}
