using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class World
    {
        public static readonly List<Item> Items = new List<Item>(); //Make a list of items available
        public static readonly List<Monster> Monsters = new List<Monster>(); //List of monsters
        public static readonly List<Quest> Quests = new List<Quest>(); //List of quests
        public static readonly List<Location> Locations = new List<Location>(); //Lists of Locations
        public static readonly List<Spell> Spells = new List<Spell>(); //Lists of spells available

        public const int UNSELLABLE_ITEM_PRICE = -1;

        //make IDs for items
        public const int ITEM_ID_BARE_FISTS = 0; //Default Weapon in case the player sells all other weapons
        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_SLIME_GEL = 2;
        public const int ITEM_ID_SLIME_TAIL = 3;
        public const int ITEM_ID_SPIDER_FANG = 4;
        public const int ITEM_ID_SPIDER_SILK = 5;
        public const int ITEM_ID_RAT_TAIL = 6;
        public const int ITEM_ID_PIECE_OF_FUR = 7;
        public const int ITEM_ID_HEALING_POTION = 8;
        public const int ITEM_ID_CASTLE_PERMIT = 9;
        public const int ITEM_ID_COPPER_SWORD = 10;
        
        //make IDs for monsters
        public const int MONSTER_ID_SLIME = 1;
        public const int MONSTER_ID_SPIDER = 2;
        public const int MONSTER_ID_RAT = 3;

        //make IDs for quests
        public const int QUEST_ID_FARMERS_FIELD = 1;
        public const int QUEST_ID_MEADOWS = 2;
        public const int QUEST_ID_SHOPDISTRICT = 3;

        //make IDs for locations
        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_SHOP_DISTRICT = 3; //for putting shops here
        public const int LOCATION_ID_FARMERS_FIELD = 4;
        public const int LOCATION_ID_MEADOWS = 5;
        public const int LOCATION_ID_FOREST_OF_MUSHROOMS = 6;
        public const int LOCATION_ID_CAVE = 7;
        public const int LOCATION_ID_CASTLE_GATE = 8;

        //make IDs for spells
        public const int SPELL_ID_FLAME = 1;
        public const int SPELL_ID_FIREBALL = 2;
        public const int SPELL_ID_ICICLE = 3;
        public const int SPELL_ID_ICEBEAM = 4;
        public const int SPELL_ID_WINDBLADE = 5;
        public const int SPELL_ID_WINDJET = 6;
        public const int SPELL_ID_ROCKTHROW = 7;
        public const int SPELL_ID_BOULDERSLAM = 8;

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
            PopulateSpells();
        }
        private static void PopulateItems() //To add the available items in the game to a list 
        {
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty Sword", "Rusty Swords", 0, 5, 10));
            Items.Add(new Weapon(ITEM_ID_COPPER_SWORD, "Copper Sword", "Copper Swords", 3, 7, 15));
            Items.Add(new Item(ITEM_ID_SLIME_GEL, "Slime Gel", "Slime Gels",1));
            Items.Add(new Item(ITEM_ID_SLIME_TAIL, "Slime Tail", "Slime Tails", 1));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider Fang", "Spider Fangs", 2));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider Silk", "Spider Silks", 2));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat Tail", "Rat Tails", 1));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of Fur", "Pieces of Fur", 2));
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing Potion", "Healing Potions",20,10)); 

            Items.Add(new Item(ITEM_ID_CASTLE_PERMIT, "Castle Permit", "Castle Permits", UNSELLABLE_ITEM_PRICE)); //-1 for unsellable items
            Items.Add(new Weapon(ITEM_ID_BARE_FISTS, "Bare Fists", "Bare Fists", 0,3,UNSELLABLE_ITEM_PRICE));
            

        }

        private static void PopulateMonsters() //To add monsters to a list and fill up the loottable which contain monster drops
        {
            //unlike Items, Monster has other class references hence objects were created to make it simpler
            Monster slime = new Monster(MONSTER_ID_SLIME, "Slime", 5, 10, 5, 10, 10, 0, 0, Element.Fire); //create a new monster with the stats
            slime.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SLIME_GEL), 50, true)); //add the monster drop from item list to the LootTable list
            slime.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SLIME_TAIL), 25, false));

            Monster spider = new Monster(MONSTER_ID_SPIDER, "Spider", 5, 10, 5, 10, 10, 0, 0, Element.Ice);
            spider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 50, true));
            spider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 20, false));

            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 5, 10, 5, 10, 10, 0, 0, Element.Null);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 50, true));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, false));
            

            Monsters.Add(slime);
            Monsters.Add(spider);
            Monsters.Add(rat);

        }

        private static void PopulateQuests() //To add quests to a list
        {
            Quest questFarmersField = new Quest(QUEST_ID_FARMERS_FIELD, "Flower Crisis",
                "Kill the rats that have been ruining the fields", 50, 100);
            questFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 5)); //To populate the completion item list 
            questFarmersField.RewardItem = ItemByID(ITEM_ID_HEALING_POTION); //Reward item for this quest (variable declared separately)

            Quest questMeadows = new Quest(QUEST_ID_MEADOWS, "Meadows Trouble",
                "Slimes have been rampaging on the meadows recently. Wipe them out for the safety of the village", 100, 150);
            questMeadows.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SLIME_GEL), 5));
            questMeadows.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest questShopDistrict = new Quest(QUEST_ID_SHOPDISTRICT, "Rat Issues", "Rats have been chewing out on shop goods. Won't you take care of them?", 150, 100);
            questShopDistrict.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 5));
            questShopDistrict.RewardItem = ItemByID(ITEM_ID_CASTLE_PERMIT);

            Quests.Add(questFarmersField);
            Quests.Add(questMeadows);
            Quests.Add(questShopDistrict);
        }

        private static void PopulateLocations() //To add locations to a list
        {
            Location home = new Location(LOCATION_ID_HOME, "Home", "You were never one to clean your room.");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town Square", "The fountain in the middle of the square has a statue of an eagle. To this day, no one knows why.");

            Location shopDistrict = new Location(LOCATION_ID_SHOP_DISTRICT, "Shop District", "A place where merchants gather together to sell their wares");
            Vendor itemShop = new Vendor("Item Shop");
            itemShop.AddItemToInventory(ItemByID(ITEM_ID_SLIME_GEL),5);
            itemShop.AddItemToInventory(ItemByID(ITEM_ID_SLIME_TAIL), 5);
            itemShop.AddItemToInventory(ItemByID(ITEM_ID_HEALING_POTION), 5);
            itemShop.AddItemToInventory(ItemByID(ITEM_ID_RUSTY_SWORD), 5);
            itemShop.AddItemToInventory(ItemByID(ITEM_ID_COPPER_SWORD), 5);
            shopDistrict.VendorWorkingHere = itemShop;
            shopDistrict.QuestAvailableHere = QuestByID(QUEST_ID_SHOPDISTRICT);
            

            Location farmersField = new Location(LOCATION_ID_FARMERS_FIELD, "Farmers Field", "Wheat stretches as far as the eyes can see");
            farmersField.QuestAvailableHere = QuestByID(QUEST_ID_FARMERS_FIELD);
            farmersField.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location meadows = new Location(LOCATION_ID_MEADOWS, "Meadows", "A great place for a picnic with snow peak mountains in the view");
            meadows.QuestAvailableHere = QuestByID(QUEST_ID_MEADOWS);
            meadows.MonsterLivingHere = MonsterByID(MONSTER_ID_SLIME);

            Location forestOfMushrooms = new Location(LOCATION_ID_FOREST_OF_MUSHROOMS, "Forest of Mushrooms", "Known for its abundance in mushrooms, especially poisonous ones");

            Location cave = new Location(LOCATION_ID_CAVE, "Cave", "Home to creatures like slimes and spiders");

            Location castleGate = new Location(LOCATION_ID_CASTLE_GATE, "Castle Gate",
                "Entrance to the home of the king and queen. You see guards patrolling vehemently for any monsters", ItemByID(ITEM_ID_CASTLE_PERMIT));

            //Linking the locations together
            home.LocationToNorth = townSquare;

            townSquare.LocationToNorth = shopDistrict;
            townSquare.LocationToSouth = home;
            townSquare.LocationToEast = castleGate;
            townSquare.LocationToWest = farmersField;

            shopDistrict.LocationToSouth = townSquare;

            castleGate.LocationToWest = townSquare;  

            farmersField.LocationToEast = townSquare;
            farmersField.LocationToWest = meadows;
            farmersField.LocationToNorth = forestOfMushrooms;

            meadows.LocationToEast = farmersField;

            forestOfMushrooms.LocationToNorth = cave;
            forestOfMushrooms.LocationToSouth = farmersField;

            cave.LocationToSouth = forestOfMushrooms;

            //Adding to the list of locations
            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(shopDistrict);
            Locations.Add(farmersField);
            Locations.Add(meadows);
            Locations.Add(forestOfMushrooms);
            Locations.Add(cave);
            Locations.Add(castleGate);
        }
        private static void PopulateSpells() //To add available spells in the game
        {
            Spells.Add(new Spell(SPELL_ID_FIREBALL, "Fire Ball", 10, 5, 3, 3, Element.Fire));
            Spells.Add(new Spell(SPELL_ID_FLAME, "Flame", 15, 8, 8, 7, Element.Fire));
            Spells.Add(new Spell(SPELL_ID_ICICLE, "Icicle", 12, 6, 6, 5, Element.Ice));
            Spells.Add(new Spell(SPELL_ID_ICEBEAM, "Ice Beam", 12, 6, 6, 9, Element.Ice));
            Spells.Add(new Spell(SPELL_ID_WINDBLADE, "Wind Blade", 7, 3, 2, 10, Element.Wind));
            Spells.Add(new Spell(SPELL_ID_WINDJET, "Wind Jet", 11, 5, 5, 15, Element.Wind));
            Spells.Add(new Spell(SPELL_ID_ROCKTHROW, "Rock Throw", 6, 2, 1, 16, Element.Earth));
            Spells.Add(new Spell(SPELL_ID_BOULDERSLAM, "Boulder Slam", 13, 8, 7, 20, Element.Earth));
        }
        public static Item ItemByID(int id) //returns an item by its id
        {
            foreach (Item item in Items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }
            return null;
        }
        public static Quest QuestByID(int id) //returns a quest by its id
        {
            foreach (Quest quest in Quests)
            {
                if (quest.ID == id)
                {
                    return quest;
                }
            }
            return null;
        }
        public static Monster MonsterByID(int id) //returns a monster by its id
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }
            return null;
        }
        public static Location LocationByID(int id) //returns a location by its id
        {
            foreach (Location location in Locations)
            {
                if (location.ID == id)
                {
                    return location;
                }
            }
            return null;
        }
        public static Spell SpellByID(int id)
        {
            foreach (Spell spell in Spells)
            {
                if (spell.ID == id)
                {
                    return spell;
                }
            }
            return null;
        }
    }
}
