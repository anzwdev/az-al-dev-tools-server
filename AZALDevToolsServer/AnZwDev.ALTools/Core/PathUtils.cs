using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AnZwDev.ALTools.Core
{
    public static class PathUtils
    {

        public static bool ContainsPath(string parentPath, string path)
        {
            return ((path.StartsWith(parentPath + Path.DirectorySeparatorChar)) || (path.Equals(parentPath)));
        }

        public static string GetRelativePath(string parentPath, string fullPath)
        {
            if (fullPath.StartsWith(parentPath))
            {
                string relativePath = fullPath.Substring(parentPath.Length);
                if ((relativePath.Length > 0) && (relativePath[0] == Path.DirectorySeparatorChar))
                {
                    return relativePath.Substring(1);
                }
            }
            return null;
        }


    }
}
