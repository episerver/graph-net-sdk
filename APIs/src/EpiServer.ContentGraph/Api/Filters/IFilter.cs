﻿namespace EPiServer.ContentGraph.Api.Filters
{
    public interface IFilter
    {
        public string FilterClause { get; }
        public List<IFilter> Filters { get; set; }
        //public IFilter Add(IFilter other);
    }
}