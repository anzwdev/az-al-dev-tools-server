using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class CodeCompletionItem
    {

        public string label { get; set; }
        public CompletionItemKind kind { get; set; }
        public string filterText { get; set; }
        public string detail { get; set; }
        public List<CodeCompletionItemTag> tags { get; set; }

        public CodeCompletionItem()
        { 
        }

        public CodeCompletionItem(string label, CompletionItemKind kind)
        {
            this.label = label;
            this.kind = kind;
        }

    }
}
