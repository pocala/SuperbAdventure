using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class PlayerSpell : INotifyPropertyChanged
    {
        private Spell _details;
        public Spell Details
        {
            get
            {
                return _details;
            }
            set
            {
                _details = value;
                OnPropertyChanged("Details");
            }
        }
        private bool _isLearnt;
        public bool IsLearnt
        {
            get 
            {
                return _isLearnt; 
            }
            set
            {
                _isLearnt = value;
                OnPropertyChanged("IsLearnt");
            }
        }
        public PlayerSpell (Spell details)
        {
            Details = details;
            IsLearnt = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
