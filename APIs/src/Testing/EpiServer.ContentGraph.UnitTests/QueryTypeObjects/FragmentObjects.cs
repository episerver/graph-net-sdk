using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpiServer.ContentGraph.UnitTests.QueryTypeObjects
{
    internal class PromoObject
    {
        public int Id { get; set; }
        public int WorkId { get; set; }
        public string GuidValue { get; set; }
        public string ProviderName { get; set; }
        public string Url { get; set; }
        public RequestTypeObject Expanded { get; set; }
    }
    internal class FragmentObject
    {
        public virtual IEnumerable<string> ContentType { get; set; }

        public virtual string RouteSegment { get; set; }

        public virtual string Url { get; set; }

        public virtual string RelativePath { get; set; }

        public virtual string Status { get; set; }

        public virtual IEnumerable<string> Ancestors { get; set; }

        
        public virtual string Name { get; set; }

        public string SitePageTheme { get; set; }

        
        public string Heading { get; set; }

        
        public string Preamble { get; set; }

        
        public string PromoHeading { get; set; }

        
        public string PromoText { get; set; }

        
        public PromoObject PromoImage { get; set; } = new();

        
        public IEnumerable<PromoObject> Categories { get; set; }

        
        public PromoObject CompanyContentLink { get; set; } = new();

        public string ReadingTime { get; set; }

        public string ViewingTime { get; set; }

        public DateTime CommonDate { get; set; }
    }
}
