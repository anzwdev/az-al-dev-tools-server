using AnZwDev.ALTools.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectAppPackageFile : ALProjectFile
    {

        #region Public properties

        public string FullName { get; private set; }
        public string NameWithPublisher { get; private set; }
        public VersionNumber Version { get; private set; }

        #endregion

        public ALProjectAppPackageFile(ALProject project, string relativePath): base(project, relativePath)
        {
        }

        protected override void OnRelativePathChanged()
        {
            base.OnRelativePathChanged();

            this.FullName = Path.GetFileNameWithoutExtension(this.RelativePath);
            int versionPos = this.FullName.LastIndexOf("_");
            if (versionPos >= 0)
            {
                this.Version = new VersionNumber(this.FullName.Substring(versionPos + 1).Trim());
                this.NameWithPublisher = this.FullName.Substring(0, versionPos);
            }
            else
            {
                this.Version = new VersionNumber();
                this.NameWithPublisher = this.FullName;
            }
        }

        public bool IsMatch(string name, string publisher, VersionNumber version)
        {
            string findNameWithPublisher = publisher + "_" + name;
            return ((this.NameWithPublisher.Equals(findNameWithPublisher, StringComparison.CurrentCultureIgnoreCase)) &&
                    (this.Version.GreaterOrEqual(version)));
        }

    }

}
