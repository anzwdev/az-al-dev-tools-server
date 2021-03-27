using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetXmlPortTableElementDetailsRequest : GetSymbolInformationDetailsRequest
    {

        public string xmlPortName { get; set; }
        public bool getXmlPortTableFields { get; set; }
        public bool getAvailableFields { get; set; }

    }
}
