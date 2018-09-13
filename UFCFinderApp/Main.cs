using CefSharp;
using CefSharp.OffScreen;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;
using UFCFinderApp;

namespace UFCFinderApp
{
    public partial class Main : Form
    {
        public static ChromiumWebBrowser Browser;
        public bool IsFBLoggedIn = false;
        private string fbEmail = null;
        private string fbPassword = null;
        private string fbDataFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ufcfinder.dat";

        public Main()
        {
            InitializeComponent();

            Cef.Initialize(new CefSettings()
            {
                IgnoreCertificateErrors = true,
                UserAgent = "Mozilla/5.0 (Linux; Android 7.0; SM-G930V Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.125 Mobile Safari/537.36"
            });

            DependencyChecker.AssertAllDependenciesPresent();

            Browser = new ChromiumWebBrowser(string.Empty, new BrowserSettings()
            {
                ImageLoading = CefState.Disabled
            });
            Browser.FindHandler = new ListProcessor.FindResult();
            Browser.Size = new System.Drawing.Size(1024, 16000);

            Go.Enabled = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            HtmlLabel panel = new HtmlLabel();
            panel.Name = "HtmlView";
            panel.AutoSizeHeightOnly = true;
            HtmlPanel.Controls.Add(panel);
            HtmlPanel.Left = 0;
            HtmlPanel.Top = 0;
            HtmlPanel.Width = this.Width - SystemInformation.VerticalScrollBarWidth;
            HtmlPanel.Height = 621;
            HtmlPanel.VerticalScroll.Enabled = true;
            HtmlPanel.VerticalScroll.Visible = true;
            panel.Left = 0;
            panel.Top = 0;
            panel.Width = HtmlPanel.Width - SystemInformation.VerticalScrollBarWidth;
            panel.SendToBack();
            HtmlPanel.BringToFront();

            if (File.Exists(fbDataFile))
            {
                string data = File.ReadAllText(fbDataFile);

                if (data.IndexOf("||") > -1 && data.IndexOf("||") < data.Length - 2)
                {
                    fbEmail = data.Substring(0, data.IndexOf("||"));
                    fbPassword = data.Substring(data.IndexOf("||") + 2);
                }
            }

            while (!Browser.IsBrowserInitialized)
                Application.DoEvents();

            Task.Factory.StartNew(() => { logInToFB(); });
        }

        private void logInToFB()
        {

            if (string.IsNullOrEmpty(fbEmail) || string.IsNullOrEmpty(fbPassword))
            {
                Invoke((MethodInvoker)delegate { FacebookPanel.Show(); });
            }
            else
            {
                Browser.FrameLoadEnd += Browser_FacebookLoaded;
                Browser.Load("https://www.facebook.com");
            }
        }

        private void Browser_FacebookLoaded(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                Browser.FrameLoadEnd -= Browser_FacebookLoaded;
                Browser.FrameLoadEnd += Browser_FacebookLoggedIn;

                Browser.ExecuteScriptAsync(
                    string.Format(
                    @"                      
                        document.getElementById('m_login_email').value = '{0}';
                        document.getElementById('m_login_password').value = '{1}';
                        document.getElementById('login_form').getElementsByTagName('button')[0].click();
                    ",
                    Cryptography.Decrypt(fbEmail).Replace("'", "\\'"),
                    Cryptography.Decrypt(fbPassword).Replace("'", "\\'")
                    ));
            }
        }

        private void Browser_FacebookLoggedIn(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                Browser.FrameLoadEnd -= Browser_FacebookLoggedIn;
                Invoke((MethodInvoker)delegate { Go.Enabled = true; });
            }
        }

        private void LegalListButton_Click(object sender, EventArgs e)
        {
            LegalList.ShowDialog();
        }

        private void LegalListFile_Click(object sender, EventArgs e)
        {
            LegalList.ShowDialog();
        }

        private void WatchListButton_Click(object sender, EventArgs e)
        {
            WatchList.ShowDialog();
        }

        private void WatchListFile_Click(object sender, EventArgs e)
        {
            WatchList.ShowDialog();
        }

        private void LegalList_FileOk(object sender, CancelEventArgs e)
        {
            LegalListFile.Text = Path.GetFileName(LegalList.FileName);
        }

        private void WatchList_FileOk(object sender, CancelEventArgs e)
        {
            WatchListFile.Text = Path.GetFileName(WatchList.FileName);
        }

        private void Go_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LegalList.FileName.Trim()))
            {
                MessageBox.Show("A legal list is required");
                return;
            }

            if (string.IsNullOrEmpty(WatchList.FileName.Trim()))
            {
                MessageBox.Show("A watch list is required");
                return;
            }

            if (string.IsNullOrEmpty(Phrases.Text.Trim()))
            {
                MessageBox.Show("At least one search phrase is required");
                return;
            }

            Go.Enabled = false;
            WatchListButton.Enabled = false;
            LegalListButton.Enabled = false;
            Phrases.Enabled = false;

            List<UFCLocation> legalList = new List<UFCLocation>();
            List<UFCLocation> watchList = new List<UFCLocation>();
            List<string> searchPhrases = new List<string>();

            foreach (string phrase in Phrases.Text.Trim().Split('\n'))
            {
                if (!string.IsNullOrEmpty(phrase.Trim())) searchPhrases.Add(phrase.Trim().ToLower());
            }

            // Process the entries in the legal list
            try
            {
                using (SLDocument workbook = new SLDocument(LegalList.FileName))
                {
                    int row = 3;

                    while (!string.IsNullOrEmpty(workbook.GetCellValueAsString(row, 1)))
                    {
                        UFCLocation location = new UFCLocation()
                        {
                            Business = workbook.GetCellValueAsString(row, 1).Trim(),
                            Address = workbook.GetCellValueAsString(row, 2).Trim(),
                            City = workbook.GetCellValueAsString(row, 3).Trim(),
                            State = workbook.GetCellValueAsString(row, 4).Trim(),
                            Zip = workbook.GetCellValueAsString(row, 5).Trim()
                        };

                        location.LocationString = getLocationString(location.Address, location.City, location.State, location.Zip);

                        legalList.Add(location);

                        row++;
                    }
                }
            }
            catch
            {
                MessageBox.Show("There is a format problem with the Legal List");
                Go.Enabled = true;
                WatchListButton.Enabled = true;
                LegalListButton.Enabled = true;
                Phrases.Enabled = true;
                return;
            }

            // Process the entries in the watch list
            try
            {
                using (SLDocument workbook = new SLDocument(WatchList.FileName))
                {
                    int row = 2;

                    while (!string.IsNullOrEmpty(workbook.GetCellValueAsString(row, 1)))
                    {
                        UFCLocation location = new UFCLocation()
                        {
                            Business = workbook.GetCellValueAsString(row, 1).Trim(),
                            Address = workbook.GetCellValueAsString(row, 2).Trim(),
                            City = workbook.GetCellValueAsString(row, 3).Trim(),
                            State = workbook.GetCellValueAsString(row, 4).Trim(),
                            Zip = workbook.GetCellValueAsString(row, 5).Trim(),
                            URLs = new List<string>()
                        };

                        location.LocationString = getLocationString(location.Address, location.City, location.State, location.Zip);

                        string[] urls = workbook.GetCellValueAsString(row, 7).Trim().Split('\n');

                        foreach (string url in urls)
                        {
                            if (!string.IsNullOrEmpty(url.Trim())) location.URLs.Add(url.Trim());
                        }

                        watchList.Add(location);

                        row++;
                    }
                }
            }
            catch
            {
                Go.Enabled = true;
                WatchListButton.Enabled = true;
                LegalListButton.Enabled = true;
                Phrases.Enabled = true;
                MessageBox.Show("There is a format problem with the Watch List");
                return;
            }

            try
            {
                ListProcessor.ProcessList(
                    Progress,
                    Processed,
                    HtmlPanel,
                    Browser,
                    legalList,
                    watchList,
                    searchPhrases
                    );
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an issue processing your results.\n\n" + ex.ToString());
            }

            Progress.Value = 0;
            Processed.Text = string.Empty;
            Go.Enabled = true;
            WatchListButton.Enabled = true;
            LegalListButton.Enabled = true;
            Phrases.Enabled = true;
        }

        private string getLocationString(string address, string city, string state, string zip)
        {
            // Create a uniform location string that can be used for matching
            return
                (
                    address.Trim().ToLower() +
                    city.Trim().ToLower() +
                    state.Trim().ToLower() +
                    zip.Trim().ToLower()
                )
                .Replace(" ", "")
                .Replace(",", "")
                .Replace("-", "")
                .Replace(".", "")
                .Replace("#", "")
                .Replace("avenue", "ave")
                .Replace("street", "st")
                .Replace("circle", "cir")
                .Replace("road", "rd")
                .Replace("boulevard", "blvd")
                .Replace("highway", "hwy")
                .Replace("parkway", "pkwy")
                .Replace("lane", "ln")
                .Replace("place", "pl");
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            HtmlPanel.Hide();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Browser.Dispose();
            Cef.Shutdown();
        }

        private void FBLogin_Click(object sender, EventArgs e)
        {
            if (FBEmail.Text.Trim().Length > 0 && FBPassword.Text.Trim().Length > 0)
            {
                fbEmail = Cryptography.Encrypt(FBEmail.Text);
                fbPassword = Cryptography.Encrypt(FBPassword.Text);

                File.WriteAllText(fbDataFile, fbEmail + "||" + fbPassword);

                FacebookPanel.Hide();

                logInToFB();
            }
        }
    }
}
