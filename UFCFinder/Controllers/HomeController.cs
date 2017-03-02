using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using UFCFinder.Models;
using SpreadsheetLight;
using System.Net;
using System.Text.RegularExpressions;

namespace UFCFinder.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            UFCSearch search = new UFCSearch();
            if (Request.QueryString["token"] != null) search.FBAccessToken = Request.QueryString["token"];

            return View(search);
        }

        [HttpPost]
        public ActionResult Index(UFCSearch search)
        {
            bool errors = false;

            if (string.IsNullOrEmpty(search.FBAccessToken))
            {
                ModelState.AddModelError("FBAccessToken", "You must log into Facebook before submitting a query");
                errors = true;
            }

            if (search.LegalList == null || search.LegalList.ContentLength == 0)
            {
                ModelState.AddModelError("LegalList", "A Legal List is required");
                errors = true;
            }
            else if (search.LegalList.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                ModelState.AddModelError("LegalList", "Legal List must be an Excel file (version 2007 or higher - xslx)");
                errors = true;
            }

            if (search.WatchList == null || search.WatchList.ContentLength == 0)
            {
                ModelState.AddModelError("WatchList", "A Watch List is required");
                errors = true;
            }
            else if (search.WatchList.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                ModelState.AddModelError("WatchList", "Watch List must be an Excel file (version 2007 or higher - xslx)");
                errors = true;
            }

            if (string.IsNullOrEmpty(search.SearchPhrases))
            {
                ModelState.AddModelError("SearchPhrases", "At least one search phrase is required");
                errors = true;
            }

            if (!errors)
            {
                List<UFCLocation> legalList = new List<UFCLocation>();
                List<UFCLocation> watchList = new List<UFCLocation>();
                List<string> searchPhrases = new List<string>();
                search.LegalLocations = new List<UFCLocation>();
                search.Results = new List<UFCResult>();

                foreach (string phrase in search.SearchPhrases.Trim().Split('\n'))
                {
                    if (!string.IsNullOrEmpty(phrase.Trim())) searchPhrases.Add(phrase.Trim().ToLower());
                }

                bool noErrors = true;

                try
                {
                    using (SLDocument workbook = new SLDocument(search.LegalList.InputStream))
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
                catch (Exception ex)
                {
                    ModelState.AddModelError("LegalList", "There is a format problem with the Legal List");
                    noErrors = false;
                }

                if (noErrors)
                {
                    try
                    {
                        using (SLDocument workbook = new SLDocument(search.WatchList.InputStream))
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
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("WatchList", "There is a format problem with the Watch List");
                        noErrors = false;
                    }

                    if (noErrors)
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.Headers[HttpRequestHeader.UserAgent] = "Opera/9.80 (J2ME/MIDP; Opera Mini/9 (Compatible; MSIE:9.0; iPhone; BlackBerry9700; AppleWebKit/24.746; U; en) Presto/2.5.25 Version/10.54";

                            foreach (UFCLocation location in watchList)
                            {
                                if (legalList.Where(l => l.LocationString == location.LocationString).Any())
                                {
                                    search.LegalLocations.Add(location);
                                    continue;
                                }

                                foreach (string url in location.URLs)
                                {
                                    List<string> matchedPhrases = new List<string>();
                                    string finalUrl = url;

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

                                    try
                                    {
                                        string html = client.DownloadString(finalUrl).ToLower().Replace("\\'", "'").Replace("\\n", " ");

                                        foreach (string phrase in searchPhrases)
                                        {
                                            if (html.IndexOf(phrase) > -1) matchedPhrases.Add(phrase);
                                        }

                                        if (matchedPhrases.Count() > 0)
                                        {
                                            search.Results.Add(new UFCResult()
                                            {
                                                Location = location,
                                                URL = url,
                                                PhraseMatches = matchedPhrases
                                            });
                                        }
                                    }
                                    catch (WebException ex)
                                    {
                                        ModelState.AddModelError("Results", "There was an issue downloading " + url);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return View(search);
        }

        private string getLocationString(string address, string city, string state, string zip)
        {
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
    }
}