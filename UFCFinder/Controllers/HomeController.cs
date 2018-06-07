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
using System.Threading;
using System.Threading.Tasks;
using UFCFinder.Services;
using System.Web.Hosting;

namespace UFCFinder.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            UFCSearch search = new UFCSearch();
            
            // Check for a FB access token
            if (Request.QueryString["token"] != null) search.FBAccessToken = Request.QueryString["token"];

            return View(search);
        }

        [HttpPost]
        public ActionResult Index(UFCSearch search)
        {
            if (HttpContext.Application["Processing"] != null)
            {
                ModelState.AddModelError("General", "A list is currently being processed, please wait for the results before processing another list");
                return View(search);
            }

            bool errors = false;

            // Check for input errors

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

            if (string.IsNullOrEmpty(search.Email))
            {
                ModelState.AddModelError("Email", "An email address is required to receive the results");
                errors = true;
            }

            if (!errors)
            {
                List<UFCLocation> legalList = new List<UFCLocation>();
                List<UFCLocation> watchList = new List<UFCLocation>();
                List<string> searchPhrases = new List<string>();

                foreach (string phrase in search.SearchPhrases.Trim().Split('\n'))
                {
                    if (!string.IsNullOrEmpty(phrase.Trim())) searchPhrases.Add(phrase.Trim().ToLower());
                }

                bool noErrors = true;

                // Process the entries in the legal list
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
                catch
                {
                    ModelState.AddModelError("LegalList", "There is a format problem with the Legal List");
                    noErrors = false;
                }

                if (noErrors)
                {
                    // Process the entries in the watch list
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
                    catch
                    {
                        ModelState.AddModelError("WatchList", "There is a format problem with the Watch List");
                        noErrors = false;
                    }

                    if (noErrors)
                    {
                        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => 
                            ListProcessor.ProcessList(
                                HttpContext.Application,
                                HttpContext.Server,
                                legalList, 
                                watchList, 
                                searchPhrases, 
                                search.Email, 
                                search.FBAccessToken,
                                cancellationToken
                                ));
                        ViewBag.Success = true;
                    }
                }
            }

            return View(search);
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
    }
}