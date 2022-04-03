﻿using EspionSpotify.Extensions;
using EspionSpotify.Models;
using EspionSpotify.Properties;
using MetroFramework.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using EspionSpotify.Translations;

namespace EspionSpotify.Controls
{
    public class FrmSpotifyAPICredentials : MetroFramework.Forms.MetroForm
    {
        private TableLayoutPanel tableLayoutPanel2;
        private MetroLabel lblClientId;
        private MetroTextBox txtSecretId;
        private MetroLabel lblSecretId;
        private MetroTextBox txtClientId;
        private MetroLink lnkFAQSpotifyAPI;

        private readonly string _pastSpotifyAPIClientId;
        private readonly string _pastSpotifyAPISecretId;
        private readonly string _pastSpotifyAPIRedirectURL;

        private const int OAUTH2_KEY_LENGTH = 32;
        private MetroFramework.Components.MetroToolTip tip;
        private MetroLink lnkSpotifyAPIDashboard;
        private MetroLabel lblRedirectURL;
        private MetroTextBox txtRedirectURL;
        private readonly Analytics _analytics;

        public FrmSpotifyAPICredentials(Analytics analytics)
        {
            InitializeComponent();

            _analytics = analytics;

            _pastSpotifyAPIClientId = Settings.Default.app_spotify_api_client_id?.Trim();
            _pastSpotifyAPISecretId = Settings.Default.app_spotify_api_client_secret?.Trim();
            _pastSpotifyAPIRedirectURL = Settings.Default.app_spotify_api_redirect_url?.Trim();

            if (string.IsNullOrEmpty(_pastSpotifyAPIRedirectURL))
            {
                _pastSpotifyAPIRedirectURL = API.SpotifyAPI.SPOTIFY_API_DEFAULT_REDIRECT_URL;
            }

            txtClientId.Text = _pastSpotifyAPIClientId;
            txtSecretId.Text = _pastSpotifyAPISecretId;
            txtRedirectURL.Text = _pastSpotifyAPIRedirectURL;

            Text = FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.TitleSpotifyAPICredentials);

            lblClientId.Text = FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.LblClientId);
            lblSecretId.Text = FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.LblSecretId);
            lblRedirectURL.Text = FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.LblRedirectURL);

            txtClientId.WaterMark = FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.WatermarkClientId);
            txtSecretId.WaterMark = FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.WatermarkSecretId);
            txtRedirectURL.WaterMark = FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.WatermarkRedirectURL);

            tip.SetToolTip(lnkFAQSpotifyAPI, FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.TipFAQSpotifyAPI));
            tip.SetToolTip(lnkSpotifyAPIDashboard, FrmEspionSpotify.Instance.Rm.GetString(I18NKeys.TipSpotifyAPIDashboard));
        }

        #region Components
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSpotifyAPICredentials));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtRedirectURL = new MetroFramework.Controls.MetroTextBox();
            this.txtClientId = new MetroFramework.Controls.MetroTextBox();
            this.txtSecretId = new MetroFramework.Controls.MetroTextBox();
            this.lblClientId = new MetroFramework.Controls.MetroLabel();
            this.lblSecretId = new MetroFramework.Controls.MetroLabel();
            this.lblRedirectURL = new MetroFramework.Controls.MetroLabel();
            this.lnkFAQSpotifyAPI = new MetroFramework.Controls.MetroLink();
            this.tip = new MetroFramework.Components.MetroToolTip();
            this.lnkSpotifyAPIDashboard = new MetroFramework.Controls.MetroLink();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.txtRedirectURL, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtClientId, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtSecretId, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblClientId, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblSecretId, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblRedirectURL, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(10, 60);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(400, 122);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // txtRedirectURL
            // 
            this.txtRedirectURL.BackColor = System.Drawing.Color.Black;
            // 
            // 
            // 
            this.txtRedirectURL.CustomButton.Image = null;
            this.txtRedirectURL.CustomButton.Location = new System.Drawing.Point(238, 1);
            this.txtRedirectURL.CustomButton.Name = "";
            this.txtRedirectURL.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.txtRedirectURL.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtRedirectURL.CustomButton.TabIndex = 1;
            this.txtRedirectURL.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtRedirectURL.CustomButton.UseSelectable = true;
            this.txtRedirectURL.CustomButton.Visible = false;
            this.txtRedirectURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRedirectURL.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtRedirectURL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtRedirectURL.Lines = new string[0];
            this.txtRedirectURL.Location = new System.Drawing.Point(131, 83);
            this.txtRedirectURL.MaxLength = 32;
            this.txtRedirectURL.Name = "txtRedirectURL";
            this.txtRedirectURL.PasswordChar = '\0';
            this.txtRedirectURL.PromptText = "PASTE_REDIRECT_URL_HERE";
            this.txtRedirectURL.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtRedirectURL.SelectedText = "";
            this.txtRedirectURL.SelectionLength = 0;
            this.txtRedirectURL.SelectionStart = 0;
            this.txtRedirectURL.ShortcutsEnabled = true;
            this.txtRedirectURL.Size = new System.Drawing.Size(266, 29);
            this.txtRedirectURL.Style = MetroFramework.MetroColorStyle.Green;
            this.txtRedirectURL.TabIndex = 40;
            this.txtRedirectURL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRedirectURL.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtRedirectURL.UseCustomBackColor = true;
            this.txtRedirectURL.UseSelectable = true;
            this.txtRedirectURL.WaterMark = "PASTE_REDIRECT_URL_HERE";
            this.txtRedirectURL.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtRedirectURL.WaterMarkFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRedirectURL.TextChanged += new System.EventHandler(this.TxtRedirectURL_TextChanged);
            // 
            // txtClientId
            // 
            this.txtClientId.BackColor = System.Drawing.Color.Black;
            // 
            // 
            // 
            this.txtClientId.CustomButton.Image = null;
            this.txtClientId.CustomButton.Location = new System.Drawing.Point(238, 1);
            this.txtClientId.CustomButton.Name = "";
            this.txtClientId.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.txtClientId.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtClientId.CustomButton.TabIndex = 1;
            this.txtClientId.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtClientId.CustomButton.UseSelectable = true;
            this.txtClientId.CustomButton.Visible = false;
            this.txtClientId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtClientId.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtClientId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtClientId.Lines = new string[0];
            this.txtClientId.Location = new System.Drawing.Point(131, 13);
            this.txtClientId.MaxLength = 32;
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.PasswordChar = '\0';
            this.txtClientId.PromptText = "PASTE_KEY_ID_HERE";
            this.txtClientId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtClientId.SelectedText = "";
            this.txtClientId.SelectionLength = 0;
            this.txtClientId.SelectionStart = 0;
            this.txtClientId.ShortcutsEnabled = true;
            this.txtClientId.Size = new System.Drawing.Size(266, 29);
            this.txtClientId.Style = MetroFramework.MetroColorStyle.Green;
            this.txtClientId.TabIndex = 35;
            this.txtClientId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtClientId.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtClientId.UseCustomBackColor = true;
            this.txtClientId.UseSelectable = true;
            this.txtClientId.WaterMark = "PASTE_KEY_ID_HERE";
            this.txtClientId.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtClientId.WaterMarkFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientId.TextChanged += new System.EventHandler(this.TxtClientId_TextChanged);
            // 
            // txtSecretId
            // 
            this.txtSecretId.BackColor = System.Drawing.Color.Black;
            // 
            // 
            // 
            this.txtSecretId.CustomButton.Image = null;
            this.txtSecretId.CustomButton.Location = new System.Drawing.Point(238, 1);
            this.txtSecretId.CustomButton.Name = "";
            this.txtSecretId.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.txtSecretId.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtSecretId.CustomButton.TabIndex = 1;
            this.txtSecretId.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSecretId.CustomButton.UseSelectable = true;
            this.txtSecretId.CustomButton.Visible = false;
            this.txtSecretId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSecretId.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtSecretId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtSecretId.Lines = new string[0];
            this.txtSecretId.Location = new System.Drawing.Point(131, 48);
            this.txtSecretId.MaxLength = 32;
            this.txtSecretId.Name = "txtSecretId";
            this.txtSecretId.PasswordChar = '\0';
            this.txtSecretId.PromptText = "PASTE_KEY_ID_HERE";
            this.txtSecretId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSecretId.SelectedText = "";
            this.txtSecretId.SelectionLength = 0;
            this.txtSecretId.SelectionStart = 0;
            this.txtSecretId.ShortcutsEnabled = true;
            this.txtSecretId.Size = new System.Drawing.Size(266, 29);
            this.txtSecretId.Style = MetroFramework.MetroColorStyle.Green;
            this.txtSecretId.TabIndex = 36;
            this.txtSecretId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSecretId.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtSecretId.UseCustomBackColor = true;
            this.txtSecretId.UseSelectable = true;
            this.txtSecretId.WaterMark = "PASTE_KEY_ID_HERE";
            this.txtSecretId.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSecretId.WaterMarkFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSecretId.TextChanged += new System.EventHandler(this.TxtSecretId_TextChanged);
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClientId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblClientId.Location = new System.Drawing.Point(3, 10);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(122, 35);
            this.lblClientId.TabIndex = 37;
            this.lblClientId.Text = "LBL_CLIENT_ID";
            this.lblClientId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblClientId.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblSecretId
            // 
            this.lblSecretId.AutoSize = true;
            this.lblSecretId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSecretId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblSecretId.Location = new System.Drawing.Point(3, 45);
            this.lblSecretId.Name = "lblSecretId";
            this.lblSecretId.Size = new System.Drawing.Size(122, 35);
            this.lblSecretId.TabIndex = 38;
            this.lblSecretId.Text = "LBL_SECRET_ID";
            this.lblSecretId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSecretId.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblRedirectURL
            // 
            this.lblRedirectURL.AutoSize = true;
            this.lblRedirectURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRedirectURL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRedirectURL.Location = new System.Drawing.Point(3, 80);
            this.lblRedirectURL.Name = "lblRedirectURL";
            this.lblRedirectURL.Size = new System.Drawing.Size(122, 35);
            this.lblRedirectURL.TabIndex = 39;
            this.lblRedirectURL.Text = "LBL_REDIRECT_URL";
            this.lblRedirectURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRedirectURL.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lnkFAQSpotifyAPI
            // 
            this.lnkFAQSpotifyAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkFAQSpotifyAPI.Image = global::EspionSpotify.Properties.Resources.faq;
            this.lnkFAQSpotifyAPI.ImageSize = 12;
            this.lnkFAQSpotifyAPI.Location = new System.Drawing.Point(369, 5);
            this.lnkFAQSpotifyAPI.Margin = new System.Windows.Forms.Padding(0);
            this.lnkFAQSpotifyAPI.Name = "lnkFAQSpotifyAPI";
            this.lnkFAQSpotifyAPI.Size = new System.Drawing.Size(18, 18);
            this.lnkFAQSpotifyAPI.TabIndex = 44;
            this.lnkFAQSpotifyAPI.UseCustomBackColor = true;
            this.lnkFAQSpotifyAPI.UseSelectable = true;
            this.lnkFAQSpotifyAPI.Click += new System.EventHandler(this.LnkFAQSpotifyAPI_Click);
            // 
            // tip
            // 
            this.tip.Style = MetroFramework.MetroColorStyle.Default;
            this.tip.StyleManager = null;
            this.tip.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // lnkSpotifyAPIDashboard
            // 
            this.lnkSpotifyAPIDashboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkSpotifyAPIDashboard.Image = global::EspionSpotify.Properties.Resources.spotify;
            this.lnkSpotifyAPIDashboard.ImageSize = 14;
            this.lnkSpotifyAPIDashboard.Location = new System.Drawing.Point(344, 5);
            this.lnkSpotifyAPIDashboard.Margin = new System.Windows.Forms.Padding(0);
            this.lnkSpotifyAPIDashboard.Name = "lnkSpotifyAPIDashboard";
            this.lnkSpotifyAPIDashboard.Size = new System.Drawing.Size(18, 18);
            this.lnkSpotifyAPIDashboard.TabIndex = 45;
            this.lnkSpotifyAPIDashboard.UseCustomBackColor = true;
            this.lnkSpotifyAPIDashboard.UseSelectable = true;
            this.lnkSpotifyAPIDashboard.Click += new System.EventHandler(this.LnkSpotifyAPIDashboard_Click);
            // 
            // FrmSpotifyAPICredentials
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(420, 202);
            this.Controls.Add(this.lnkSpotifyAPIDashboard);
            this.Controls.Add(this.lnkFAQSpotifyAPI);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Movable = false;
            this.Name = "FrmSpotifyAPICredentials";
            this.Padding = new System.Windows.Forms.Padding(10, 60, 10, 20);
            this.Resizable = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "SPOTIFY_API_CREDENTIALS";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSpotifyAPICredentials_FormClosing);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion Components

        private void TxtClientId_TextChanged(object sender, EventArgs e)
        {
            var value = ((MetroTextBox)sender).Text.Trim();
            if (!string.IsNullOrEmpty(value) && value.Length != OAUTH2_KEY_LENGTH) return;

            Settings.Default.app_spotify_api_client_id = value;
            Settings.Default.Save();
        }

        private void TxtSecretId_TextChanged(object sender, EventArgs e)
        {
            var value = ((MetroTextBox)sender).Text.Trim();
            if (!string.IsNullOrEmpty(value) && value.Length != OAUTH2_KEY_LENGTH) return;

            Settings.Default.app_spotify_api_client_secret = value;
            Settings.Default.Save();
        }

        private void FrmSpotifyAPICredentials_FormClosing(object sender, FormClosingEventArgs e)
        {
            var haveIds = !string.IsNullOrEmpty(this.txtClientId.Text.Trim()) && !string.IsNullOrEmpty(txtSecretId.Text.Trim());

            var redirectURL = string.IsNullOrWhiteSpace(txtRedirectURL.Text)
                ? API.SpotifyAPI.SPOTIFY_API_DEFAULT_REDIRECT_URL
                : txtRedirectURL.Text;

            var sameClientId = _pastSpotifyAPIClientId == txtClientId.Text;
            var sameSecretId = _pastSpotifyAPISecretId == txtSecretId.Text;
            var sameRedirectURL = _pastSpotifyAPIRedirectURL == redirectURL;

            var allValuesSame = sameClientId && sameSecretId && sameRedirectURL;

            if (allValuesSame) this.DialogResult = DialogResult.Cancel;
            else if (haveIds) this.DialogResult = DialogResult.Yes;
            else this.DialogResult = DialogResult.No;
        }

        private void LnkFAQSpotifyAPI_Click(object sender, EventArgs e)
        {
            Process.Start(GitHub.WEBSITE_FAQ_SPOTIFY_API_URL);
            Task.Run(async () => await _analytics.LogAction($"faq-spotify-api"));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LnkSpotifyAPIDashboard_Click(object sender, EventArgs e)
        {
            Process.Start(API.SpotifyAPI.SPOTIFY_API_DASHBOARD_URL);
            Task.Run(async () => await _analytics.LogAction($"spotify-api-dashboard"));
        }

        private void TxtRedirectURL_TextChanged(object sender, EventArgs e)
        {
            var value = ((MetroTextBox)sender).Text.Trim();

            Settings.Default.app_spotify_api_redirect_url = string.IsNullOrEmpty(value)
                ? API.SpotifyAPI.SPOTIFY_API_DEFAULT_REDIRECT_URL
                : value;
            Settings.Default.Save();
        }
    }
}
