using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public enum Element //enum to store types of element
    {
        Fire,
        Ice,
        Wind,
        Earth,
        Null
    }
    public class Spell
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public int MinimumDamage { get; set; }
        public int ManaCost { get; set; }
        public int LevelRequiredToLearn { get; set; }
        
        public Element Element { get; set; }
        public Spell(int id, string name, int maximumDamage, int minimumDamage, int manaCost, int levelRequiredToLearn, Element element)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            MinimumDamage = minimumDamage;
            ManaCost = manaCost;
            LevelRequiredToLearn = levelRequiredToLearn;
            Element = element;
        }
    }
}
