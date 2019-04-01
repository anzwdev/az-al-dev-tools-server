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
            try
            {
                ALDevToolsServer host = new ALDevToolsServer(args[0]);
                host.Initialize();
                host.Run();
            }
            catch (Exception e)
            {
                System.IO.StreamWriter w = new System.IO.StreamWriter("c:\\temp\\aaa.txt", true);
                w.WriteLine(e.Message);
                w.WriteLine(e.StackTrace);
                w.Close();
            }
        }
    }
}
