using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppTypeDefinition : ALAppElementWithName
    {

        public List<int> ArrayDimensions { get; set; }
        public bool Temporary { get; set; }
        public ALAppSubtypeDefinition Subtype { get; set; }
        public List<ALAppTypeDefinition> TypeArguments { get; set; }
        public List<string> OptionMembers { get; set; }

        public ALAppTypeDefinition()
        {
        }

        public override string GetSourceCode()
        {
            string fullName = this.Name;

            if (!this.EmptySubtype())
                return this.Name + " " + ALSyntaxHelper.EncodeName(this.Subtype.Name.Trim());

            if ((this.TypeArguments != null) && (this.TypeArguments.Count > 0))
            {
                fullName = fullName + " of [";
                for (int i = 0; i < this.TypeArguments.Count; i++)
                {
                    if (i > 0)
                        fullName = fullName + ",";
                    fullName = fullName + this.TypeArguments[i].GetSourceCode();
                }
                fullName = fullName + "]";
            }

            if ((this.ArrayDimensions != null) && (this.ArrayDimensions.Count > 0))
            {
                string array = "array[";
                for (int i = 0; i < this.ArrayDimensions.Count; i++)
                {
                    if (i > 0)
                        array = array + ",";
                    array = array + this.ArrayDimensions[i].ToString();
                }
                fullName = array + "] of " + fullName;
            }

            return fullName;
        }

        protected override ALSymbolInformation CreateMainALSymbol()
        {
            return new ALSymbolInformation(this.GetALSymbolKind(), this.GetSourceCode());
        }

        public bool EmptySubtype()
        {
            return ((this.Subtype == null) || (String.IsNullOrWhiteSpace(this.Subtype.Name)) || (this.Subtype.Name.ToLower() == "none"));
        }

        public bool IsEmpty()
        {
            return ((String.IsNullOrWhiteSpace(this.Name)) || (this.Name.ToLower() == "none"));
        }

    }
}
