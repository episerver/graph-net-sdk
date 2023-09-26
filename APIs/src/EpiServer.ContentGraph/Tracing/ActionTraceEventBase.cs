using System.Collections.Generic;
using System.Linq;

namespace EPiServer.ContentGraph.Tracing
{
    public class ActionTraceEventBase<TAction> : TraceEvent
    {
        private IList<TAction> Actions { get; }

        public ActionTraceEventBase(ITraceable source, string message, IEnumerable<TAction> actions = null) : base(source, message)
        {
            Actions = actions.ToList();
        }
    }
}