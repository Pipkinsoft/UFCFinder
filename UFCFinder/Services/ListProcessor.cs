using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using UFCFinder.Models;

namespace UFCFinder.Services
{
    public static class ListProcessor
    {
        public static void ProcessList(
            HttpApplicationStateBase application,
            HttpServerUtilityBase server,
            List<UFCLocation> legalList,
            List<UFCLocation> watchList,
            List<string> searchPhrases,
            string email,
            string fbAccessToken,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            try
            {
                application.Add("Processing", true);

                DateTime LastFBProcess = DateTime.MinValue;
                Queue<Tuple<UFCLocation, string, string>> urlList = new Queue<Tuple<UFCLocation, string, string>>();
                UFCSearch search = new UFCSearch()
                {
                    FBAccessToken = fbAccessToken,
                    LegalLocations = new List<UFCLocation>(),
                    Results = new List<UFCResult>(),
                    Errors = new List<string>()
                };

                int fbInterval = int.Parse(ConfigurationManager.AppSettings["FBInterval"]);

                // Go through the locations in the watch list
                foreach (UFCLocation location in watchList)
                {
                    // Check if the location is also in the legal list.  If so, don't process
                    if (legalList.Where(l => l.LocationString == location.LocationString).Any())
                    {
                        search.LegalLocations.Add(location);
                        continue;
                    }

                    // Process each URL for the location
                    foreach (string url in location.URLs)
                    {
                        string finalUrl = url;

                        // Check for specific social media URLs and make them into API request URLs
                        if (url.IndexOf("facebook.com") > -1)
                        {
                            Regex regex = new Regex(@"^https?://www.facebook.com/(events/(?<alias>\d+)|pg/[^/]+\-(?<alias>\d+)|pages/[^/]+/(?<alias>\d+)|[^/]+\-(?<alias>\d+)|(?<alias>[^/]+)).*$");
                            Match match = regex.Match(url);

                            if (match.Success)
                            {
                                finalUrl =
                                    string.Format(
                                        "https://graph.facebook.com/{0}/posts?access_token={1}",
                                        match.Groups["alias"].Value,
                                        search.FBAccessToken
                                        );
                            }
                        }

                        urlList.Enqueue(new Tuple<UFCLocation, string, string>(location, url, finalUrl));
                    }
                }

                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.UserAgent] = "Opera/9.80 (J2ME/MIDP; Opera Mini/9 (Compatible; MSIE:9.0; iPhone; BlackBerry9700; AppleWebKit/24.746; U; en) Presto/2.5.25 Version/10.54";

                    List<string> matchedPhrases = new List<string>();

                    while (urlList.Count() > 0)
                    {
                        if (cancellationToken.IsCancellationRequested) throw new Exception("Process cancelled");

                        Tuple<UFCLocation, string, string> url = urlList.Dequeue();

                        if (url.Item3.IndexOf("graph.facebook.com") > -1)
                        {
                            if ((DateTime.Now - LastFBProcess).TotalSeconds < fbInterval)
                            {
                                urlList.Enqueue(url);
                                Thread.Sleep(1000);
                                continue;
                            }
                            else
                            {
                                LastFBProcess = DateTime.Now;
                            }
                        }

                        try
                        {
                            // Get response from the URL
                            string html = client.DownloadString(url.Item3).ToLower().Replace("\\'", "'").Replace("\\n", " ");

                            // Check if any of the search phrases match the result
                            foreach (string phrase in searchPhrases)
                            {
                                if (html.IndexOf(phrase) > -1) matchedPhrases.Add(phrase);
                            }

                            // If there are matched phrases, add the location to the result set
                            if (matchedPhrases.Count() > 0)
                            {
                                search.Results.Add(new UFCResult()
                                {
                                    Location = url.Item1,
                                    URL = url.Item2,
                                    PhraseMatches = matchedPhrases
                                });
                            }
                        }
                        catch (WebException ex)
                        {
                            search.Errors.Add("There was an issue downloading " + url.Item2 + ": " + ex.Message);
                        }
                    }
                }

                using (MailMessage msg = new MailMessage())
                {
                    msg.To.Add(email);
                    msg.Subject = "UFC Event Host Finder Results";
                    msg.IsBodyHtml = true;

                    string body =
                        "<h1>UFC Event Host Finder Results</h1>" +
                        "<h3>Legal Locations on Watch List</h3>";

                    if (search.LegalLocations.Count() == 0)
                    {
                        body += "No legal locations found";
                    }
                    else
                    {
                        body +=
                            @"
                        <div class=""results"" style=""margin-top: 10px"">
                            <table class=""resultsTable"">
                                <thead>
                                    <tr>
                                        <td class=""business-col"" syle=""width: 50%; padding: 10px; background-color: #555555; color: #eeeeee;"">Business</td>
                                        <td class=""address-col"" syle=""width: 50%; padding: 10px; background-color: #555555; color: #eeeeee;"">Address</td>
                                    </tr>
                                </thead>
                                <tbody>
                        ";

                        foreach (UFCLocation location in search.LegalLocations)
                        {
                            body +=
                                string.Format(
                                @"
                            <tr>
                                <td class=""business-col"" syle=""width: 50%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">@location.Business</td>
                                <td class=""address-col"" syle=""width: 50%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">
                                    {0}<br />
                                    {1}, {2} {3}
                                </td>
                            </tr>
                            ",
                                server.HtmlEncode(location.Address),
                                server.HtmlEncode(location.City),
                                server.HtmlEncode(location.State),
                                server.HtmlEncode(location.Zip)
                                );
                        }

                        body +=
                            @"
                                </tbody>
                            </table>
                        </div>
                        ";
                    }

                    body += "<h2>Query Results</h2>";

                    if (search.Results.Count() == 0)
                    {
                        body += "No locations matched your query";
                    }
                    else
                    {
                        body +=
                            @"
                        <div class=""results"" style=""margin-top: 10px"">
                            <table class=""resultsTable"">
                                <thead>
                                    <tr>
                                        <td class=""first-col"" syle=""width: 25%; padding: 10px; background-color: #555555; color: #eeeeee;"">Business</td>
                                        <td class=""second-col"" syle=""width: 20%; padding: 10px; background-color: #555555; color: #eeeeee;"">Address</td>
                                        <td class=""third-col"" syle=""width: 40%; padding: 10px; background-color: #555555; color: #eeeeee;"">URL</td>
                                        <td class=""fourth-col"" syle=""width: 15%; padding: 10px; background-color: #555555; color: #eeeeee;"">Matched Phrases</td>
                                    </tr>
                                </thead>
                                <tbody>
                        ";

                        foreach (UFCResult result in search.Results)
                        {
                            body +=
                                string.Format(
                                @"
                            <tr>
                                <td class=""first-col"" syle=""width: 25%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">{0}</td>
                                <td class=""second-col"" syle=""width: 20%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">
                                    {1}<br />
                                    {2}, {3} {4}
                                </td>
                                <td class=""third-col"" syle=""width: 40%; padding: 10px; border-bottom: solid 1px #aaaaaa;""><a href=""{5}"" target=""_blank"">{6}</a></td>
                                <td class=""fourth-col"" syle=""width: 15%; padding: 10px; border-bottom: solid 1px #aaaaaa;"">
                                    {7}
                                </td>
                            </tr>
                            ",
                                server.HtmlEncode(result.Location.Business),
                                server.HtmlEncode(result.Location.Address),
                                server.HtmlEncode(result.Location.City),
                                server.HtmlEncode(result.Location.State),
                                server.HtmlEncode(result.Location.Zip),
                                result.URL,
                                server.HtmlEncode(result.URL),
                                string.Join("<br/>", result.PhraseMatches.Select(p => server.HtmlEncode(p)).ToArray())
                                );
                        }

                        body +=
                            @"
                                </tbody>
                            </table>
                        </div>
                        ";
                    }

                    body += "<br/>&nbsp;<br/>&nbsp;";

                    msg.Body = body;

                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Send(msg);
                    }
                }

                application.Remove("Processing");
            }
            catch (Exception ex)
            {
                using (MailMessage msg = new MailMessage())
                {
                    msg.To.Add(email);
                    msg.CC.Add("dpipkin@pipkinsoft.com");
                    msg.Subject = "UFC Event Host Finder Error";
                    msg.Body = "An error occurred while processing your results:\n\n" + ex.ToString();

                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Send(msg);
                    }
                }
            }
            finally
            {
                if (application["Processing"] != null)
                    application.Remove("Processing");
            }
        }
    }
}