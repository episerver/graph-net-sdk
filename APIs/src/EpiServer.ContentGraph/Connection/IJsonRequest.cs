using EPiServer.ContentGraph.Tracing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EPiServer.ContentGraph.Connection
{
    public interface IJsonRequest : ITraceable, IDisposable
    {
        System.Text.Encoding Encoding { get; }
        Task<Stream> GetResponseStream(string body);
        void AddRequestHeader(string name, string value);
    }
}