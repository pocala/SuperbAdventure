using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Engine
{
    public class LivingCreature : INotifyPropertyChanged //Creates an interface. An interface is a contract that defines a set of methods/properties/events
    {
        private int _currentHitPoints;
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged("CurrentHitPoints"); 
            }
        }
        public int MaximumHitPoints { get; set; }
        public int MaximumManaPoints { get; set; }

        private int _currentManaPoints;
        public int CurrentManaPoints
        {
            get { return _currentManaPoints; }
            set
            {
                _currentManaPoints = value;
                OnPropertyChanged("CurrentManaPoints");
            }
        }
        public LivingCreature(int currentHitPoints, int maximumHitPoints, int currentManaPoints, int maximumManaPoints)
        {
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
            CurrentManaPoints = currentManaPoints;
            MaximumManaPoints = maximumManaPoints;
        }

        public event PropertyChangedEventHandler PropertyChanged;//Declares the event
        //An event is a mechanism that enables a class/object to provide notifications to other class/objects 
        //This event is what the UI will subscribe to or any other class/objects that are interested to know if a property changed
        //In this case, if "CurrentHitPoints" was changed, subscribers will receive notifications
        protected void OnPropertyChanged(string name) //Method to raise the event
        {
            if (PropertyChanged != null) //Checks to see if there are any subscribers
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name)); //Raises the event (i.e allows notifications to be sent out by activating the event)
            }
        }
    }
}
