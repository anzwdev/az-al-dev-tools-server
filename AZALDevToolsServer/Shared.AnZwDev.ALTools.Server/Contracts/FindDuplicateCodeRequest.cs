using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class FindDuplicateCodeRequest
    {
        public int minNoOfStatements { get; set; }
        public string path { get; set; }
    }
}
