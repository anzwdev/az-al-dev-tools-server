using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ALDevToolsServer host = new ALDevToolsServer(args[0]);
            host.Initialize();
            host.Run();
        }
    }
}
