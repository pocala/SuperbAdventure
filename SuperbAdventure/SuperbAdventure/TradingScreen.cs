﻿using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperbAdventure
{
    public partial class TradingScreen : Form
    {
        private Player _currentPlayer;
        public TradingScreen(Player player)
        {
            _currentPlayer = player;
            InitializeComponent();

            //Style to display numeric column values
            DataGridViewCellStyle rightAlignedCellStyle = new DataGridViewCellStyle(); //object type is to define special formatting for dgv columns
            rightAlignedCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //Populate the datagrid with the player's inventory which is dgvMyItems
            dgvMyItems.RowHeadersVisible = false;
            dgvMyItems.AutoGenerateColumns = false;

            //This column holds the Item ID to know which item to sell
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemID",
                Visible = false
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Description"
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                Width = 30,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Quantity"
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle= rightAlignedCellStyle,
                DataPropertyName = "Price"
            });
            dgvMyItems.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "Sell 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ItemID"
            });
            
            dgvMyItems.DataSource = _currentPlayer.Inventory; //Bind player inventory to the dgv
            dgvMyItems.CellClick += dgvMyItems_CellClick; //Call this function when cell is clicked

            dgvVendorItems.RowHeadersVisible = false;
            dgvVendorItems.AutoGenerateColumns = false;

            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemID",
                Visible = false
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Description"
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Price"
            });
            dgvVendorItems.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "Buy 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ItemID"
            });
            dgvVendorItems.DataSource = _currentPlayer.CurrentLocation.VendorWorkingHere.Inventory; //Bind vendor inventory to the dgv
            dgvVendorItems.CellClick += dgvVendorItems_CellClick; //calls this function when row is clicked on
        }
       
        private void dgvMyItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4) //4 because it is a zero-based array, i.e first column starts from 0 instead of 1
            {
                //Gets the ID value of the item 
                var itemID = dgvMyItems.Rows[e.RowIndex].Cells[0].Value;
                //Gets the Item object from the selected item row
                Item itemBeingSold = World.ItemByID(Convert.ToInt32(itemID));
                if (itemBeingSold.Price == World.UNSELLABLE_ITEM_PRICE)
                {
                    MessageBox.Show("You cannot sell the " + itemBeingSold.Name + "!");
                }
                else
                {
                    _currentPlayer.RemoveItemFromInventory(itemBeingSold);
                    _currentPlayer.Gold += itemBeingSold.Price;
                }

            }
        }
        private void dgvVendorItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                var itemID = dgvVendorItems.Rows[e.RowIndex].Cells[0].Value;
                //Gets the Item object for the selected item row
                Item itemBeingBought = World.ItemByID(Convert.ToInt32(itemID));
                if (_currentPlayer.Gold >= itemBeingBought.Price)
                {
                    _currentPlayer.AddItemToInventory(itemBeingBought);
                    _currentPlayer.Gold -= itemBeingBought.Price;
                }
                else
                {
                    MessageBox.Show("You do not have enough gold to purchase the " + itemBeingBought.Name + "!");
                }

            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}


