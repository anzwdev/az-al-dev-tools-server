using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetTableFieldsListRequest : GetSymbolsInformationRequest
    {

        public string table { get; set; }

    }
}
