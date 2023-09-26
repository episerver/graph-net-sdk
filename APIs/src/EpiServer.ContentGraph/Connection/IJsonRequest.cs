using EPiServer.ContentGraph.Tracing;
using System;
using System.Collections.Generic;

namespace EPiServer.ContentGraph.Connection
{
    public interface IJsonRequest : ITraceable, IDisposable
    {
        System.Text.Encoding Encoding { get; }
        void WriteBody(string body);
        System.IO.Stream GetRequestStream(long contentLength);
        System.IO.Stream GetResponseStream();
        string GetResponse();

        void AddRequestHeader(string name, string value);
        Dictionary<string,string> GetResponseHeaders();
    }
}