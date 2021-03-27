using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppXmlPortNode: ALAppBaseElement
    {

        public string Name { get; set; }
        public ALAppXmlPortNodeKind Kind { get; set; }
        public string Expression { get; set; }
        public ALAppElementsCollection<ALAppXmlPortNode> Schema { get; set; }

        public ALAppXmlPortNode()
        {
            this.Schema = null;
        }

        public void AddChildNode(ALAppXmlPortNode node)
        {
            if (this.Schema == null)
                this.Schema = new ALAppElementsCollection<ALAppXmlPortNode>();
            this.Schema.Add(node);
        }

    }
}
