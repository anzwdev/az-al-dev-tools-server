﻿/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Nav2018.Contracts
{
    public class GetSyntaxTreeRequest
    {
        public string source { get; set; }
        public string path { get; set; }
        public bool open { get; set; }
    }
}
