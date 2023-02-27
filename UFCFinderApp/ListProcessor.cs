using CefSharp;
using CefSharp.OffScreen;
using CefSharp.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace UFCFinderApp
{
    public static class ListProcessor
    {
        public static bool Loaded = false;
        public static bool? Found = null;

        private static void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(5000);
                    Loaded = true;
                });
            }
        }

        public class FindResult : IFindHandler
        {
            public void OnFindResult(IWebBrowser chromiumWebBrowser, IBrowser browser, int identifier, int count, Rect selectionRect, int activeMatchOrdinal, bool finalUpdate)
            {
                Found = (count > 0);
            }
        }

        public static void ProcessList(
            ProgressBar Progress,
            Label Processed,
            Panel HtmlPanel,
            ChromiumWebBrowser Browser,
            List<UFCLocation> legalList,
            List<UFCLocation> watchList,
            List<string> searchPhrases
            )
        {
            Progress.Maximum = watchList.Select(w => w.URLs.Count()).Sum();
            Progress.Value = 0;
            Processed.Text = "0 / " + Progress.Maximum.ToString() + " urls processed";

            UFCSearch search = new UFCSearch()
            {
                LegalLocations = new List<UFCLocation>(),
                Results = new List<UFCResult>(),
                Errors = new List<string>()
            };

            Browser.LoadError += Browser_LoadError;

            // Go through the locations in the watch list
            foreach (UFCLocation location in watchList)
            {
                // Check if the location is also in the legal list.  If so, don't process
                if (legalList.Where(l => l.LocationString == location.LocationString).Any())
                {
                    search.LegalLocations.Add(location);
                    Progress.Value += location.URLs.Count();
                    Processed.Text = Progress.Value.ToString() + " / " + Progress.Maximum.ToString() + " urls processed";
                    Application.DoEvents();
                    continue;
                }

                // Process each URL for the location
                foreach (string url in location.URLs)
                {
                    Loaded = false;
                    Browser.LoadingStateChanged += Browser_LoadingStateChanged;

                    // Load url in browser
                    Browser.Load(url);
                    
                    while (!Loaded)
                        Application.DoEvents();

                    Browser.LoadingStateChanged -= Browser_LoadingStateChanged;
                    Browser.Stop();

                    if (Debugger.IsAttached)
                    {
                        Bitmap screenshot = Browser.ScreenshotOrNull();

                        if (screenshot != null)
                        {
                            screenshot.Save(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Url" + Progress.Value.ToString() + ".jpg");

                            screenshot.Dispose();
                        }
                    }

                    List<string> matchedPhrases = new List<string>();

                    // Check if any of the search phrases match the result
                    foreach (string phrase in searchPhrases)
                    {
                        Found = null;

                        Browser.Find(0, phrase, true, false, false);

                        while (!Found.HasValue)
                            Application.DoEvents();

                        Browser.StopFinding(true);

                        if (Found.Value)
                            matchedPhrases.Add(phrase);
                    }

                    // If there are matched phrases, add the location to the result set
                    if (matchedPhrases.Count() > 0)
                    {
                        search.Results.Add(new UFCResult()
                        {
                            Location = location,
                            URL = url,
                            PhraseMatches = matchedPhrases
                        });
                    }

                    Progress.Value++;
                    Processed.Text = Progress.Value.ToString() + " / " + Progress.Maximum.ToString() + " urls processed";
                    Application.DoEvents();
                }
            }

            string html =
                @"<html><head></head><body style=""margin: 10px;"">" +
                "<h1>UFC Event Host Finder Results</h1>" +
                "<h3>Legal Locations on Watch List</h3>";

            if (search.LegalLocations.Count() == 0)
            {
                html += "No legal locations found";
            }
            else
            {
                html +=
                @"
                <div class=""results"" style=""margin-top: 10px"">
                    <table class=""resultsTable"">
                        <thead>
                            <tr>
                                <td class=""business-col"" style=""width: 50%; padding: 10px; background-color: #555555; color: #eeeeee;"">Business</td>
                                <td class=""address-col"" style=""width: 50%; padding: 10px; background-color: #555555; color: #eeeeee;"">Address</td>
                            </tr>
                        </thead>
                        <tbody>
                ";

                foreach (UFCLocation location in search.LegalLocations)
                {
                    html +=
                        string.Format(
                        @"
                    <tr>
                        <td class=""business-col"" style=""width: 50%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">{0}</td>
                        <td class=""address-col"" style=""width: 50%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">
                            {1}<br />
                            {2}, {3} {4}
                        </td>
                    </tr>
                    ",
                        HttpUtility.HtmlEncode(location.Business),
                        HttpUtility.HtmlEncode(location.Address),
                        HttpUtility.HtmlEncode(location.City),
                        HttpUtility.HtmlEncode(location.State),
                        HttpUtility.HtmlEncode(location.Zip)
                        );
                }

                html +=
                    @"
                        </tbody>
                    </table>
                </div>
                ";
            }

            html += "<h2>Query Results</h2>";

            if (search.Results.Count() == 0)
            {
                html += "No locations matched your query";
            }
            else
            {
                html +=
                    @"
                <div class=""results"" style=""margin-top: 10px"">
                    <table class=""resultsTable"">
                        <thead>
                            <tr>
                                <td class=""first-col"" style=""width: 25%; padding: 10px; background-color: #555555; color: #eeeeee;"">Business</td>
                                <td class=""second-col"" style=""width: 20%; padding: 10px; background-color: #555555; color: #eeeeee;"">Address</td>
                                <td class=""third-col"" style=""width: 40%; padding: 10px; background-color: #555555; color: #eeeeee;"">URL</td>
                                <td class=""fourth-col"" style=""width: 15%; padding: 10px; background-color: #555555; color: #eeeeee;"">Matched Phrases</td>
                            </tr>
                        </thead>
                        <tbody>
                ";

                foreach (UFCResult result in search.Results)
                {
                    html +=
                        string.Format(
                        @"
                    <tr>
                        <td class=""first-col"" style=""width: 25%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">{0}</td>
                        <td class=""second-col"" style=""width: 20%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">
                            {1}<br />
                            {2}, {3} {4}
                        </td>
                        <td class=""third-col"" style=""width: 40%; padding: 10px; border-bottom: solid 1px #aaaaaa;""><a href=""{5}"" target=""_blank"">{6}</a></td>
                        <td class=""fourth-col"" style=""width: 15%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">
                            {7}
                        </td>
                    </tr>
                    ",
                        HttpUtility.HtmlEncode(result.Location.Business),
                        HttpUtility.HtmlEncode(result.Location.Address),
                        HttpUtility.HtmlEncode(result.Location.City),
                        HttpUtility.HtmlEncode(result.Location.State),
                        HttpUtility.HtmlEncode(result.Location.Zip),
                        result.URL,
                        HttpUtility.HtmlEncode(result.URL),
                        string.Join("<br/>", result.PhraseMatches.Select(p => HttpUtility.HtmlEncode(p)).ToArray())
                        );
                }

                html +=
                    @"
                        </tbody>
                    </table>
                </div>
                ";
            }

            html += 
                "<br/>&nbsp;<br/>&nbsp;" +
                "</body></html>";

            ((HtmlLabel)HtmlPanel.Controls.Find("HtmlView", true).First()).Text = html;
            HtmlPanel.Show();
        }

        private static void Browser_LoadError(object sender, LoadErrorEventArgs e)
        {
            var test = 1;
        }
    }
}
