using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSpyAgent
{
    class ServerConnectionException : Exception
    {
        public ServerConnectionException(string message) : base(message)
        {
        }
    }
}
