﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using System.IO;

namespace SuperbAdventure
{
    public partial class SuperbAdventure : Form
    {
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";
        private Player _player;

        public SuperbAdventure()
        {
            InitializeComponent();
            //_player = PlayerDataMapper.CreateFromDatabase();
            if ( _player == null )
            {
                if (File.Exists(PLAYER_DATA_FILE_NAME))
                {
                    _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));

                }
                else
                {
                    _player = Player.CreateDefaultPlayer();
                }

            }

            lblHitPoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            lblExperience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lblLevel.DataBindings.Add("Text", _player, "Level");
            lblManaPoints.DataBindings.Add("Text", _player, "CurrentManaPoints");

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.DataSource = _player.Inventory; //"binds" the data to the UI
            
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn //Configures the datagridview for Inventory
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description" //The property to be displayed
            });

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });

            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;
            dgvQuests.DataSource = _player.Quests; //"binds" the data to the UI
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn //Configures the datagridview for PlayerQuest
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Name" //The property to be displayed
            });

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "IsCompleted"
            });

            cboWeapons.DataSource = _player.Weapons;
            cboWeapons.DisplayMember = "Name"; //What property to display
            cboWeapons.ValueMember = "ID"; //What property to use as value
            if (_player.CurrentWeapon != null)
            {
                cboWeapons.SelectedItem = _player.CurrentWeapon;
            }
            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged; //Existing function for when the player chooses a new weapon in the combobox 

            cboPotions.ValueMember = "ID";
            cboPotions.DisplayMember = "Name";
            cboPotions.DataSource = _player.HealingPotions;

            cboSpells.ValueMember = "ID";
            cboSpells.DisplayMember= "Name";
            cboSpells.DataSource = _player.Spells;

            _player.PropertyChanged += PlayerOnPropertyChanged; //Function to update combobox data when player inventory changes 
            //This line subscribes the PlayerOnPropertyChanged to the PropertyChanged event of the _player object
            //Whenever a property of _player changes and raise the PropertyChanged event, PlayerOnPropertyChanged will be invoked

            _player.OnMessage += DisplayMessage;

            _player.MoveTo(_player.CurrentLocation);
        }

        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtbMessages.Text += messageEventArgs.Message + Environment.NewLine;
            if (messageEventArgs.AddExtraNewLine)
            {
                rtbMessages.Text += Environment.NewLine;
            }
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        //sender is the object that raises the event, propertyChangedEventArgs contains info on the changed property    
        {
            if (propertyChangedEventArgs.PropertyName == "Weapons") //To see which property was changed on the Player Object. Value comes from Player.RaiseInventoryChangedEvent
            {
                cboWeapons.DataSource = _player.Weapons;
                if (!_player.Weapons.Any()) //.Any checks if there are anything in the list
                {
                    cboWeapons.Visible = false;
                    btnUseWeapon.Visible = false;
                }
            }
            if (propertyChangedEventArgs.PropertyName == "HealingPotions")
            {
                cboPotions.DataSource = _player.HealingPotions;
                if (!_player.HealingPotions.Any())
                {
                    cboPotions.Visible = false;
                    btnUsePotion.Visible = false;
                }
            }
            if ((propertyChangedEventArgs.PropertyName == "Spells"))
            {
                cboSpells.DataSource = _player.Spells;
            }
            if (propertyChangedEventArgs.PropertyName == "CurrentLocation")
            {
                //Display available movement buttons
                btnNorth.Visible = (_player.CurrentLocation.LocationToNorth != null);
                btnEast.Visible = (_player.CurrentLocation.LocationToEast != null);
                btnSouth.Visible = (_player.CurrentLocation.LocationToSouth != null);
                btnWest.Visible = (_player.CurrentLocation.LocationToWest != null);
                //Display current location description and name
                rtbLocation.Text = _player.CurrentLocation.Name + Environment.NewLine;
                rtbLocation.Text += _player.CurrentLocation.Description + Environment.NewLine;
                //Display trade and shops button
                btnShops.Visible = (_player.CurrentLocation.VendorWorkingHere != null);

                if (_player.CurrentLocation.MonsterLivingHere == null)
                {
                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    cboSpells.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;
                    btnUseSpell.Visible = false;
                }

                else
                {
                    cboWeapons.Visible = _player.Weapons.Any();
                    cboPotions.Visible = _player.HealingPotions.Any();
                    cboSpells.Visible = _player.Spells.Any();
                    btnUseWeapon.Visible = _player.Weapons.Any();
                    btnUsePotion.Visible = _player.HealingPotions.Any();
                    btnUseSpell.Visible = _player.Spells.Any();
                }
            }
            if (propertyChangedEventArgs.PropertyName == "CurrentMonster")
            {
                if (_player.CurrentMonster == null)
                {
                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    cboSpells.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;
                    btnUseSpell.Visible = false;
                }
            }
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            _player.MoveNorth();
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            _player.MoveSouth();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            _player.MoveWest();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            _player.MoveEast();
        }
        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;
            _player.UseWeapon(currentWeapon);
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {

            HealingPotion potionToUse = (HealingPotion)cboPotions.SelectedItem; //selects the current item
            _player.UsePotion(potionToUse);

        }

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void SuperbAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
            //PlayerDataMapper.SaveToDatabase(_player);
        }
        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }
        private void btnUseSpell_Click(object sender, EventArgs e)
        {
            Spell currentSpell = (Spell)cboSpells.SelectedItem;
            _player.UseSpell(currentSpell);
        }

        private void btnShops_Click(object sender, EventArgs e)
        {
            ShopsScreen shopsScreen = new ShopsScreen(_player);
            shopsScreen.StartPosition = FormStartPosition.CenterScreen;
            shopsScreen.ShowDialog(this); //ShowDialog displays the trading screen
        }
    }
}
