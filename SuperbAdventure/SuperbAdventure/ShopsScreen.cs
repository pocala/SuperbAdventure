using Engine;
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
    public partial class ShopsScreen : Form
    {
        private Player _currentPlayer;
        public event EventHandler<MessageEventArgs> OnMessage; //Sends an event notification with MessageEventArgs object
        public ShopsScreen(Player player)
        {
            _currentPlayer = player;
            InitializeComponent();
            OnMessage += DisplayMessage;
            RaiseMessage("You see an Inn and a shop selling various wares", false);
            btnInnNo.Visible = false;
            btnInnYes.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnTrade_Click(object sender, EventArgs e)
        {
            TradingScreen tradingScreen = new TradingScreen(_currentPlayer);
            tradingScreen.StartPosition = FormStartPosition.CenterScreen;
            tradingScreen.ShowDialog(this); //ShowDialog displays the trading screen
        }

        private void btnInn_Click(object sender, EventArgs e)
        {
            RaiseMessage("It costs 50 Gold a night to sleep at the Inn", false);
            RaiseMessage("Do you want to rest at the Inn?", false);
            btnInnNo.Visible = true;
            btnInnYes.Visible = true;
            btnInn.Visible = false;
        }
        private void RaiseMessage(string message, bool addExtraNewLine = false) //function to raise event
        {
            if (OnMessage != null)
            {
                OnMessage(this, new MessageEventArgs(message, addExtraNewLine));
            }
        }
        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtbShopsScreen.Text += messageEventArgs.Message + Environment.NewLine;
            if (messageEventArgs.AddExtraNewLine)
            {
               rtbShopsScreen.Text += Environment.NewLine;
            }
            rtbShopsScreen.SelectionStart = rtbShopsScreen.Text.Length;
            rtbShopsScreen.ScrollToCaret();
        }

        private void btnInnYes_Click(object sender, EventArgs e)
        {
            RaiseMessage("You slept at the Inn. It's been a long day..", false);
            _currentPlayer.Gold -= 50;
            RaiseMessage("HP and MP fully recovered!");
            _currentPlayer.CurrentHitPoints = _currentPlayer.MaximumHitPoints;
            _currentPlayer.CurrentManaPoints = _currentPlayer.MaximumManaPoints;    
            btnInnNo.Visible = false;
            btnInnYes.Visible = false;
            btnInn.Visible = true;
        }

        private void btnInnNo_Click(object sender, EventArgs e)
        {
            RaiseMessage("You decided not to rest at the Inn. There are still monsters to be killed after all!", false);
            btnInnNo.Visible= false;
            btnInnYes.Visible = false;
            btnInn.Visible = true;
        }
    }
}
