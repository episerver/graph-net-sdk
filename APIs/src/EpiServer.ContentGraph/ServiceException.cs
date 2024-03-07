using System;

namespace EPiServer.ContentGraph
{
    [Serializable]
    public class ServiceException : ApplicationException
    {
        public ServiceException()
        { }

        public ServiceException(string message)
            : base(message)
        { }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
