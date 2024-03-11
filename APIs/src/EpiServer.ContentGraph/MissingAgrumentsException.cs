using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.ContentGraph
{
    public class MissingFieldException : Exception
    {
        Type _type;
        public MissingFieldException(Type type):base()
        {
            _type = type;
        }
        public override string Message => $"Can not find any field, you must select at least one field for type {_type.Name}!";
    }
}
