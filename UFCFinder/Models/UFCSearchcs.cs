using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UFCFinder.Models
{
    public struct UFCLocation
    {
        public string Business { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string LocationString { get; set; }
        public List<string> URLs { get; set; }
    }

    public struct UFCResult
    {
        public UFCLocation Location { get; set; }
        public string URL { get; set; }
        public List<string> PhraseMatches { get; set; }
    }

    public class UFCSearch
    {
        public HttpPostedFileBase LegalList { get; set; }
        public HttpPostedFileBase WatchList { get; set; }
        public string Email { get; set; }
        public string SearchPhrases { get; set; }
        public string FBAccessToken { get; set; }
        public List<UFCLocation> LegalLocations { get; set; }
        public List<UFCResult> Results { get; set; }
        public List<string> Errors { get; set; }
    }
}