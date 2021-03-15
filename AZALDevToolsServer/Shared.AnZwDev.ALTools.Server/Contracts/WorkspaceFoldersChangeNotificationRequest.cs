using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class WorkspaceFoldersChangeNotificationRequest
    {

        public string[] added { get; set; }
        public string[] removed { get; set; }

    }
}
