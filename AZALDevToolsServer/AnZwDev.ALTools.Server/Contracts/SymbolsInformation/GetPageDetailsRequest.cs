using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetPageDetailsRequest : GetSymbolInformationDetailsRequest
    {

        public bool getPageFields { get; set; }
        public bool getAvailableFields { get; set; }

    }
}
