using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectSource
    {
        public string folderPath { get; set; }
        public string packageCachePath { get; set; }

        public ALProjectSource()
        {
        }

        public ALProjectSource(string newFolderPath, string newPackageCachePath)
        {
            this.folderPath = newFolderPath;
            this.packageCachePath = newPackageCachePath;
        }
    }
}
