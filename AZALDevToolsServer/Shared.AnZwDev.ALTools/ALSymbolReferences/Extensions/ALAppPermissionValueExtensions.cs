using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Extensions
{
    public static class ALAppPermissionValueExtensions
    {

        public static ALAppPermissionValue IndirectToDirect(this ALAppPermissionValue value)
        {
            value = value & ALAppPermissionValue.AllIndirect;
            value = (ALAppPermissionValue)(((int)value) >> 5);
            return value;
        }

        public static ALAppPermissionValue DirectToIndirect(this ALAppPermissionValue value)
        {
            value = value & ALAppPermissionValue.AllDirect;
            value = (ALAppPermissionValue)(((int)value) << 5);
            return value;
        }

        public static ALAppPermissionValue FromString(string source)
        {
            ALAppPermissionValue value = ALAppPermissionValue.Empty;
            if (source != null)
                for (int i = 0; i < source.Length; i++)
                    value = value | FromChar(source[i]);
            return value;
        }

        public static ALAppPermissionValue FromChar(char value)
        {
            switch (value)
            {
                case 'R': return ALAppPermissionValue.R;
                case 'I': return ALAppPermissionValue.I;
                case 'M': return ALAppPermissionValue.M;
                case 'D': return ALAppPermissionValue.D;
                case 'X': return ALAppPermissionValue.X;
                case 'r': return ALAppPermissionValue.IndirectR;
                case 'i': return ALAppPermissionValue.IndirectI;
                case 'm': return ALAppPermissionValue.IndirectM;
                case 'd': return ALAppPermissionValue.IndirectD;
                case 'x': return ALAppPermissionValue.IndirectX;
            }
            return ALAppPermissionValue.Empty;
        }

    }
}
