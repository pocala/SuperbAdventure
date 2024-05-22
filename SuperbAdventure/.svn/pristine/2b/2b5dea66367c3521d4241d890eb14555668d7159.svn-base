using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Vendor : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public BindingList<InventoryItem> Inventory { get; set; }
        public Vendor(string name)
        {
            Name = name;
            Inventory = new BindingList<InventoryItem>();
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
            OnPropertyChanged("Inventory");
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
                    Inventory.Remove(item); //If quantity is 0, remove it from the list
                }
                OnPropertyChanged("Inventory"); //Notifies the UI that inventory has changed for weapon and potion
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
