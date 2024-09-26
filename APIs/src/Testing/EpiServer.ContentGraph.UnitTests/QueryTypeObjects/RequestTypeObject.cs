using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpiServer.ContentGraph.UnitTests.QueryTypeObjects
{
    internal class RequestTypeObject
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
        public NestedObject Property3 { get; set; }
        public IEnumerable<NestedObject> NestedObjects { get; set; }
    }
}
