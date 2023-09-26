using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EPiServer.ContentGraph.Api.Querying.Queries;
using EPiServer.ContentGraph.Helpers.Reflection;

namespace EPiServer.ContentGraph.ClientConventions
{
    public class AutoBoostConvention
    {
        private List<DateDecayTypeParameters> dateDecayParameters;

        public void SetDecayParameters<DocumentType>(DateDecayParameters parameters)
        {
            var types = from type in AppDomain.CurrentDomain.GetAssemblies().Types()
                             where typeof(DocumentType).IsAssignableFrom(type)
                             select type;
            
            if (dateDecayParameters == null)
            {
                dateDecayParameters = new List<DateDecayTypeParameters>();
            }
            dateDecayParameters.Add(new EPiServer.Find.Api.Querying.Queries.DateDecayTypeParameters(types, parameters));
        }

        public List<DateDecayTypeParameters> GetDecayParameters()
        {
            return dateDecayParameters;
        }
    }
}
