using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.Extensions;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableFieldInformaton : SymbolWithIdInformation
    {

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("fieldClass")]
        public string FieldClass { get; set; }

        [JsonProperty("state")]
        public ALAppTableFieldState State { get; set; }

        [JsonProperty("captionLabel")]
        public LabelInformation CaptionLabel { get; set; }

        [JsonProperty("toolTips")]
        public List<string> ToolTips { get; set; }

        public TableFieldInformaton()
        {
            this.InitializeLabels();
        }

        public TableFieldInformaton(int id, string name, string dataType)
        {
            this.Id = id;
            this.Name = name;
            this.Caption = name;
            this.DataType = dataType;
            this.InitializeLabels();
        }

        public TableFieldInformaton(ALProject project, ALAppTableField symbolReference)
        {
            this.InitializeLabels();
            this.Id = symbolReference.Id;
            this.Name = symbolReference.Name;
            if (symbolReference.Properties != null)
            {
                this.Caption = symbolReference.Properties.GetValue("Caption");
                this.FieldClass = symbolReference.Properties.GetValue("FieldClass");
                this.CaptionLabel.Update(symbolReference.Properties);
            } 

            if (String.IsNullOrWhiteSpace(this.Caption))
            {
                string caption = this.Name;
                if (project != null)
                    caption = caption.RemovePrefixSuffix(project.MandatoryPrefixes, project.MandatorySuffixes, project.MandatoryAffixes);
                this.Caption = caption;
                this.CaptionLabel.SetValue(this.Caption);
            }

            this.DataType = symbolReference.TypeDefinition.Name;
        }

        private void InitializeLabels()
        {
            this.CaptionLabel = new LabelInformation("Caption");
        }

        public void UpdateProperties(ALAppPropertiesCollection propertiesCollection)
        {
            if (propertiesCollection != null)
            {
                string caption = propertiesCollection.GetValue("Caption");
                if (!String.IsNullOrWhiteSpace(caption))
                {
                    this.Caption = caption;
                    this.CaptionLabel.Update(propertiesCollection);
                }
            }
        }

    }
}
