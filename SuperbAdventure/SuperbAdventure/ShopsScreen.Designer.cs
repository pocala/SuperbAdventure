using System;

namespace SuperbAdventure
{
    partial class ShopsScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.rtbShopsScreen = new System.Windows.Forms.RichTextBox();
            this.btnInn = new System.Windows.Forms.Button();
            this.btnTrade = new System.Windows.Forms.Button();
            this.btnInnYes = new System.Windows.Forms.Button();
            this.btnInnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(169, 297);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // rtbShopsScreen
            // 
            this.rtbShopsScreen.Location = new System.Drawing.Point(27, 27);
            this.rtbShopsScreen.Name = "rtbShopsScreen";
            this.rtbShopsScreen.Size = new System.Drawing.Size(349, 91);
            this.rtbShopsScreen.TabIndex = 1;
            this.rtbShopsScreen.Text = "";
            // 
            // btnInn
            // 
            this.btnInn.Location = new System.Drawing.Point(59, 149);
            this.btnInn.Name = "btnInn";
            this.btnInn.Size = new System.Drawing.Size(75, 23);
            this.btnInn.TabIndex = 2;
            this.btnInn.Text = "Inn";
            this.btnInn.UseVisualStyleBackColor = true;
            this.btnInn.Click += new System.EventHandler(this.btnInn_Click);
            // 
            // btnTrade
            // 
            this.btnTrade.Location = new System.Drawing.Point(274, 148);
            this.btnTrade.Name = "btnTrade";
            this.btnTrade.Size = new System.Drawing.Size(75, 23);
            this.btnTrade.TabIndex = 3;
            this.btnTrade.Text = "Trade";
            this.btnTrade.UseVisualStyleBackColor = true;
            this.btnTrade.Click += new System.EventHandler(this.btnTrade_Click);
            // 
            // btnInnYes
            // 
            this.btnInnYes.Location = new System.Drawing.Point(59, 178);
            this.btnInnYes.Name = "btnInnYes";
            this.btnInnYes.Size = new System.Drawing.Size(75, 23);
            this.btnInnYes.TabIndex = 4;
            this.btnInnYes.Text = "Yes";
            this.btnInnYes.UseVisualStyleBackColor = true;
            this.btnInnYes.Click += new System.EventHandler(this.btnInnYes_Click);
            // 
            // btnInnNo
            // 
            this.btnInnNo.Location = new System.Drawing.Point(59, 207);
            this.btnInnNo.Name = "btnInnNo";
            this.btnInnNo.Size = new System.Drawing.Size(75, 23);
            this.btnInnNo.TabIndex = 5;
            this.btnInnNo.Text = "No";
            this.btnInnNo.UseVisualStyleBackColor = true;
            this.btnInnNo.Click += new System.EventHandler(this.btnInnNo_Click);
            // 
            // ShopsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 332);
            this.Controls.Add(this.btnInnNo);
            this.Controls.Add(this.btnInnYes);
            this.Controls.Add(this.btnTrade);
            this.Controls.Add(this.btnInn);
            this.Controls.Add(this.rtbShopsScreen);
            this.Controls.Add(this.btnClose);
            this.Name = "ShopsScreen";
            this.Text = "ShopsScreen";
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox rtbShopsScreen;
        private System.Windows.Forms.Button btnInn;
        private System.Windows.Forms.Button btnTrade;
        private System.Windows.Forms.Button btnInnYes;
        private System.Windows.Forms.Button btnInnNo;
    }
}