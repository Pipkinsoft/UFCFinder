namespace UFCFinderApp
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.UFCPic = new System.Windows.Forms.PictureBox();
            this.LegalList = new System.Windows.Forms.OpenFileDialog();
            this.LegalListFile = new System.Windows.Forms.Label();
            this.LegalListLabel = new System.Windows.Forms.Label();
            this.LegalListButton = new System.Windows.Forms.Button();
            this.WatchListButton = new System.Windows.Forms.Button();
            this.WatchListLabel = new System.Windows.Forms.Label();
            this.WatchListFile = new System.Windows.Forms.Label();
            this.WatchList = new System.Windows.Forms.OpenFileDialog();
            this.PhrasesLabel = new System.Windows.Forms.Label();
            this.Phrases = new System.Windows.Forms.TextBox();
            this.Title = new System.Windows.Forms.Label();
            this.CoverUp = new System.Windows.Forms.Label();
            this.Go = new System.Windows.Forms.Button();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.CloseButton = new System.Windows.Forms.Button();
            this.HtmlPanel = new System.Windows.Forms.Panel();
            this.Processed = new System.Windows.Forms.Label();
            this.FacebookPanel = new System.Windows.Forms.Panel();
            this.FacebookTitle = new System.Windows.Forms.Label();
            this.FacebookCaption = new System.Windows.Forms.Label();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.FBEmail = new System.Windows.Forms.TextBox();
            this.FBPassword = new System.Windows.Forms.TextBox();
            this.FBLogin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UFCPic)).BeginInit();
            this.HtmlPanel.SuspendLayout();
            this.FacebookPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UFCPic
            // 
            this.UFCPic.BackgroundImage = global::UFCFinderApp.Properties.Resources.ufc;
            this.UFCPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.UFCPic.Dock = System.Windows.Forms.DockStyle.Top;
            this.UFCPic.Location = new System.Drawing.Point(0, 0);
            this.UFCPic.Name = "UFCPic";
            this.UFCPic.Size = new System.Drawing.Size(784, 490);
            this.UFCPic.TabIndex = 0;
            this.UFCPic.TabStop = false;
            // 
            // LegalList
            // 
            this.LegalList.DefaultExt = "xlsx";
            this.LegalList.Filter = "Excel Files|*.xlsx";
            this.LegalList.FileOk += new System.ComponentModel.CancelEventHandler(this.LegalList_FileOk);
            // 
            // LegalListFile
            // 
            this.LegalListFile.AutoEllipsis = true;
            this.LegalListFile.BackColor = System.Drawing.Color.LightGray;
            this.LegalListFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LegalListFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LegalListFile.ForeColor = System.Drawing.Color.Black;
            this.LegalListFile.Location = new System.Drawing.Point(27, 468);
            this.LegalListFile.Name = "LegalListFile";
            this.LegalListFile.Size = new System.Drawing.Size(162, 24);
            this.LegalListFile.TabIndex = 100;
            this.LegalListFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LegalListFile.Click += new System.EventHandler(this.LegalListFile_Click);
            // 
            // LegalListLabel
            // 
            this.LegalListLabel.AutoSize = true;
            this.LegalListLabel.BackColor = System.Drawing.Color.Transparent;
            this.LegalListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LegalListLabel.ForeColor = System.Drawing.Color.LightGray;
            this.LegalListLabel.Location = new System.Drawing.Point(24, 445);
            this.LegalListLabel.Name = "LegalListLabel";
            this.LegalListLabel.Size = new System.Drawing.Size(63, 15);
            this.LegalListLabel.TabIndex = 103;
            this.LegalListLabel.Text = "Legal List:";
            // 
            // LegalListButton
            // 
            this.LegalListButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LegalListButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LegalListButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.LegalListButton.Location = new System.Drawing.Point(182, 468);
            this.LegalListButton.Name = "LegalListButton";
            this.LegalListButton.Size = new System.Drawing.Size(28, 24);
            this.LegalListButton.TabIndex = 1;
            this.LegalListButton.Text = "...";
            this.LegalListButton.UseVisualStyleBackColor = false;
            this.LegalListButton.Click += new System.EventHandler(this.LegalListButton_Click);
            // 
            // WatchListButton
            // 
            this.WatchListButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.WatchListButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WatchListButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.WatchListButton.Location = new System.Drawing.Point(419, 468);
            this.WatchListButton.Name = "WatchListButton";
            this.WatchListButton.Size = new System.Drawing.Size(28, 24);
            this.WatchListButton.TabIndex = 2;
            this.WatchListButton.Text = "...";
            this.WatchListButton.UseVisualStyleBackColor = false;
            this.WatchListButton.Click += new System.EventHandler(this.WatchListButton_Click);
            // 
            // WatchListLabel
            // 
            this.WatchListLabel.AutoSize = true;
            this.WatchListLabel.BackColor = System.Drawing.Color.Transparent;
            this.WatchListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WatchListLabel.ForeColor = System.Drawing.Color.LightGray;
            this.WatchListLabel.Location = new System.Drawing.Point(261, 445);
            this.WatchListLabel.Name = "WatchListLabel";
            this.WatchListLabel.Size = new System.Drawing.Size(66, 15);
            this.WatchListLabel.TabIndex = 104;
            this.WatchListLabel.Text = "Watch List:";
            // 
            // WatchListFile
            // 
            this.WatchListFile.AutoEllipsis = true;
            this.WatchListFile.BackColor = System.Drawing.Color.LightGray;
            this.WatchListFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WatchListFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WatchListFile.ForeColor = System.Drawing.Color.Black;
            this.WatchListFile.Location = new System.Drawing.Point(264, 468);
            this.WatchListFile.Name = "WatchListFile";
            this.WatchListFile.Size = new System.Drawing.Size(162, 24);
            this.WatchListFile.TabIndex = 101;
            this.WatchListFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WatchListFile.Click += new System.EventHandler(this.WatchListFile_Click);
            // 
            // WatchList
            // 
            this.WatchList.DefaultExt = "xlsx";
            this.WatchList.Filter = "Excel Files|*.xlsx";
            this.WatchList.FileOk += new System.ComponentModel.CancelEventHandler(this.WatchList_FileOk);
            // 
            // PhrasesLabel
            // 
            this.PhrasesLabel.AutoSize = true;
            this.PhrasesLabel.BackColor = System.Drawing.Color.Transparent;
            this.PhrasesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhrasesLabel.ForeColor = System.Drawing.Color.LightGray;
            this.PhrasesLabel.Location = new System.Drawing.Point(498, 445);
            this.PhrasesLabel.Name = "PhrasesLabel";
            this.PhrasesLabel.Size = new System.Drawing.Size(97, 15);
            this.PhrasesLabel.TabIndex = 105;
            this.PhrasesLabel.Text = "Search Phrases:";
            // 
            // Phrases
            // 
            this.Phrases.BackColor = System.Drawing.Color.LightGray;
            this.Phrases.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Phrases.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Phrases.ForeColor = System.Drawing.Color.Black;
            this.Phrases.Location = new System.Drawing.Point(501, 469);
            this.Phrases.Multiline = true;
            this.Phrases.Name = "Phrases";
            this.Phrases.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Phrases.Size = new System.Drawing.Size(251, 61);
            this.Phrases.TabIndex = 3;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Title.Font = new System.Drawing.Font("Impact", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.LightGray;
            this.Title.Location = new System.Drawing.Point(219, 380);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(353, 59);
            this.Title.TabIndex = 10;
            this.Title.Text = "Event Host Finder";
            // 
            // CoverUp
            // 
            this.CoverUp.Location = new System.Drawing.Point(0, 382);
            this.CoverUp.Name = "CoverUp";
            this.CoverUp.Size = new System.Drawing.Size(784, 132);
            this.CoverUp.TabIndex = 11;
            this.CoverUp.Text = "label1";
            // 
            // Go
            // 
            this.Go.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Go.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Go.Font = new System.Drawing.Font("Impact", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Go.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Go.Location = new System.Drawing.Point(295, 516);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(163, 41);
            this.Go.TabIndex = 5;
            this.Go.Text = "Go";
            this.Go.UseVisualStyleBackColor = false;
            this.Go.Click += new System.EventHandler(this.Go_Click);
            // 
            // Progress
            // 
            this.Progress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Progress.Location = new System.Drawing.Point(0, 598);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(784, 23);
            this.Progress.TabIndex = 107;
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.ForeColor = System.Drawing.Color.Gainsboro;
            this.CloseButton.Location = new System.Drawing.Point(682, 10);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close ";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // HtmlPanel
            // 
            this.HtmlPanel.AutoScroll = true;
            this.HtmlPanel.BackColor = System.Drawing.Color.White;
            this.HtmlPanel.Controls.Add(this.CloseButton);
            this.HtmlPanel.Location = new System.Drawing.Point(3, 0);
            this.HtmlPanel.Name = "HtmlPanel";
            this.HtmlPanel.Size = new System.Drawing.Size(784, 60);
            this.HtmlPanel.TabIndex = 110;
            this.HtmlPanel.Visible = false;
            // 
            // Processed
            // 
            this.Processed.BackColor = System.Drawing.Color.Transparent;
            this.Processed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Processed.ForeColor = System.Drawing.Color.White;
            this.Processed.Location = new System.Drawing.Point(29, 568);
            this.Processed.Name = "Processed";
            this.Processed.Size = new System.Drawing.Size(725, 23);
            this.Processed.TabIndex = 113;
            this.Processed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FacebookPanel
            // 
            this.FacebookPanel.BackColor = System.Drawing.Color.DimGray;
            this.FacebookPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FacebookPanel.Controls.Add(this.FBLogin);
            this.FacebookPanel.Controls.Add(this.FBPassword);
            this.FacebookPanel.Controls.Add(this.FBEmail);
            this.FacebookPanel.Controls.Add(this.PasswordLabel);
            this.FacebookPanel.Controls.Add(this.EmailLabel);
            this.FacebookPanel.Controls.Add(this.FacebookCaption);
            this.FacebookPanel.Controls.Add(this.FacebookTitle);
            this.FacebookPanel.Location = new System.Drawing.Point(209, 109);
            this.FacebookPanel.Name = "FacebookPanel";
            this.FacebookPanel.Size = new System.Drawing.Size(376, 226);
            this.FacebookPanel.TabIndex = 114;
            this.FacebookPanel.Visible = false;
            // 
            // FacebookTitle
            // 
            this.FacebookTitle.AutoSize = true;
            this.FacebookTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FacebookTitle.ForeColor = System.Drawing.Color.White;
            this.FacebookTitle.Location = new System.Drawing.Point(101, 13);
            this.FacebookTitle.Name = "FacebookTitle";
            this.FacebookTitle.Size = new System.Drawing.Size(178, 25);
            this.FacebookTitle.TabIndex = 0;
            this.FacebookTitle.Text = "Log in to Facebook";
            // 
            // FacebookCaption
            // 
            this.FacebookCaption.AutoSize = true;
            this.FacebookCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FacebookCaption.ForeColor = System.Drawing.Color.White;
            this.FacebookCaption.Location = new System.Drawing.Point(30, 38);
            this.FacebookCaption.Name = "FacebookCaption";
            this.FacebookCaption.Size = new System.Drawing.Size(309, 15);
            this.FacebookCaption.TabIndex = 1;
            this.FacebookCaption.Text = "This only needs to be done the first time you run the app";
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmailLabel.ForeColor = System.Drawing.Color.White;
            this.EmailLabel.Location = new System.Drawing.Point(52, 96);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(52, 20);
            this.EmailLabel.TabIndex = 2;
            this.EmailLabel.Text = "Email:";
            this.EmailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.ForeColor = System.Drawing.Color.White;
            this.PasswordLabel.Location = new System.Drawing.Point(22, 133);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(82, 20);
            this.PasswordLabel.TabIndex = 3;
            this.PasswordLabel.Text = "Password:";
            this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FBEmail
            // 
            this.FBEmail.BackColor = System.Drawing.Color.Gainsboro;
            this.FBEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FBEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FBEmail.Location = new System.Drawing.Point(114, 95);
            this.FBEmail.Name = "FBEmail";
            this.FBEmail.Size = new System.Drawing.Size(223, 23);
            this.FBEmail.TabIndex = 4;
            // 
            // FBPassword
            // 
            this.FBPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.FBPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FBPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FBPassword.Location = new System.Drawing.Point(114, 133);
            this.FBPassword.Name = "FBPassword";
            this.FBPassword.PasswordChar = '*';
            this.FBPassword.Size = new System.Drawing.Size(223, 23);
            this.FBPassword.TabIndex = 5;
            // 
            // FBLogin
            // 
            this.FBLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.FBLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FBLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FBLogin.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FBLogin.Location = new System.Drawing.Point(140, 180);
            this.FBLogin.Name = "FBLogin";
            this.FBLogin.Size = new System.Drawing.Size(87, 29);
            this.FBLogin.TabIndex = 6;
            this.FBLogin.Text = "Login";
            this.FBLogin.UseVisualStyleBackColor = false;
            this.FBLogin.Click += new System.EventHandler(this.FBLogin_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(784, 621);
            this.Controls.Add(this.FacebookPanel);
            this.Controls.Add(this.HtmlPanel);
            this.Controls.Add(this.Processed);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.Go);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.Phrases);
            this.Controls.Add(this.PhrasesLabel);
            this.Controls.Add(this.WatchListButton);
            this.Controls.Add(this.WatchListLabel);
            this.Controls.Add(this.WatchListFile);
            this.Controls.Add(this.LegalListButton);
            this.Controls.Add(this.LegalListLabel);
            this.Controls.Add(this.LegalListFile);
            this.Controls.Add(this.CoverUp);
            this.Controls.Add(this.UFCPic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UFC Event Host Finder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UFCPic)).EndInit();
            this.HtmlPanel.ResumeLayout(false);
            this.FacebookPanel.ResumeLayout(false);
            this.FacebookPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox UFCPic;
        private System.Windows.Forms.OpenFileDialog LegalList;
        private System.Windows.Forms.Label LegalListFile;
        private System.Windows.Forms.Label LegalListLabel;
        private System.Windows.Forms.Button LegalListButton;
        private System.Windows.Forms.Button WatchListButton;
        private System.Windows.Forms.Label WatchListLabel;
        private System.Windows.Forms.Label WatchListFile;
        private System.Windows.Forms.OpenFileDialog WatchList;
        private System.Windows.Forms.Label PhrasesLabel;
        private System.Windows.Forms.TextBox Phrases;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label CoverUp;
        private System.Windows.Forms.Button Go;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Panel HtmlPanel;
        private System.Windows.Forms.Label Processed;
        private System.Windows.Forms.Panel FacebookPanel;
        private System.Windows.Forms.Button FBLogin;
        private System.Windows.Forms.TextBox FBPassword;
        private System.Windows.Forms.TextBox FBEmail;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Label FacebookCaption;
        private System.Windows.Forms.Label FacebookTitle;
    }
}

