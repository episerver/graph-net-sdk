using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.ContentGraph.Api.Querying
{
    public interface ILinkQueryBuilder : ITypeQueryBuilder
    {
        string GetLinkType();
    }

    public class LinkQueryBuilder<T> : TypeQueryBuilder<T>, ILinkQueryBuilder
    {
        private string _type;
        public LinkQueryBuilder() : base()
        {
        }
        public LinkQueryBuilder(string linkType) : base()
        {
            _type = linkType;
        }
        public LinkQueryBuilder<T> WithLinkType(string linkType)
        {
            if (_type == null)
            {
                _type = linkType;
            }
            return this;
        }
        public string GetLinkType()
        {
            return _type;
        }
    }
}
