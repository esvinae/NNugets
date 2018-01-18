using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSphereMqClientTest
{
    [System.Flags]
    public enum QueueAccess
    {
        None = 0,
        /// <summary>
        /// Get messages
        /// </summary>
        Get = 1,
        /// <summary>
        /// Put messages
        /// </summary>
        Put = 2,
        /// <summary>
        /// Know message queue length
        /// </summary>
        Browse = 4,
    }
}
