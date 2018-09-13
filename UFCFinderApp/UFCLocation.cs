using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFCFinderApp
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
        public List<UFCLocation> LegalLocations { get; set; }
        public List<UFCResult> Results { get; set; }
        public List<string> Errors { get; set; }
    }
}
