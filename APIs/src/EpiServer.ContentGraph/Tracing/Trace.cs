using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.ContentGraph.Helpers.Reflection;

namespace EPiServer.ContentGraph.Tracing
{
    public class Trace : ITraceListener
    {
        static IEnumerable<ITraceListener> listeners; 
        static readonly Trace instance = new Trace();

        private Trace()
        {
            try
            {
                var assemblyLocator = new AppDomainAssemblyLocator();
                listeners = assemblyLocator.AssembliesWithReferenceToAssemblyOf<ITraceListener>()
                    .AssignableTo<ITraceListener>()
                    .Concrete()
                    .Where(x => x != GetType())
                    .Select(x => (ITraceListener)Activator.CreateInstance(x)).ToList();
            }
            catch (ReflectionTypeLoadException)
            {
                // could not load some types
                listeners = new List<ITraceListener>();
            }
            
        }

        public static Trace Instance
        {
            get { return instance; }
        }

        public void Add(ITraceEvent traceEvent)
        {
            foreach (var traceListener in listeners)
            {
                traceListener.Add(traceEvent);
            }
        }
    }
}
