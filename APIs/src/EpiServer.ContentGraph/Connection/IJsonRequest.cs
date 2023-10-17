using EPiServer.ContentGraph.Tracing;
using System;
using System.Collections.Generic;

namespace EPiServer.ContentGraph.Connection
{
    public interface IJsonRequest : ITraceable, IDisposable
    {
        System.Text.Encoding Encoding { get; }
        Task<Stream> GetResponseStream(string body);
        void AddRequestHeader(string name, string value);
    }
}