using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.ContentGraph.Configuration
{
    /// <summary>
    /// Configures how OptiGraphOptions should handle the output of TransformAction when creating options
    /// </summary>
    public enum TransformActionBehaviour
    {
        /// <summary>
        /// Default, transform action changes the global options
        /// </summary>
        Default,
        /// <summary>
        /// Clone, transform action clones the options and applies transformation only on the clone
        /// </summary>
        Clone
    }
}
