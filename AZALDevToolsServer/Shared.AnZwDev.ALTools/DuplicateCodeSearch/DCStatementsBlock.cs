using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCStatementsBlock
    {
        public string SourceFilePath { get; set; }

        public List<DCStatementInstance> Statements { get; } = new List<DCStatementInstance>();

        public DCStatementsBlock()
        {
        }

        public DCStatementsBlock(string sourceFilePath)
        {
            this.SourceFilePath = sourceFilePath;
        }

    }
}
