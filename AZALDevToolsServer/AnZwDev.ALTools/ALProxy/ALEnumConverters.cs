using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALProxy
{
    public static class ALEnumConverters
    {

        private static EnumConverter<ConvertedSyntaxKind> _syntaxKindConverter = null;
        public static EnumConverter<ConvertedSyntaxKind> SyntaxKindConverter
        {
            get
            {
                if (_syntaxKindConverter == null)
                    _syntaxKindConverter = new EnumConverter<ConvertedSyntaxKind>();
                return _syntaxKindConverter;
            }
        }

        private static EnumConverter<ConvertedControlKind> _controlKindConverter = null;
        public static EnumConverter<ConvertedControlKind> ControlKindConverter
        {
            get
            {
                if (_controlKindConverter == null)
                    _controlKindConverter = new EnumConverter<ConvertedControlKind>();
                return _controlKindConverter;
            }
        }

        private static EnumConverter<ConvertedChangeKind> _changeKindConverter = null;
        public static EnumConverter<ConvertedChangeKind> ChangeKindConverter
        {
            get
            {
                if (_changeKindConverter == null)
                    _changeKindConverter = new EnumConverter<ConvertedChangeKind>();
                return _changeKindConverter;
            }
        }

        private static EnumConverter<ConvertedActionKind> _actionKindConverter = null;
        public static EnumConverter<ConvertedActionKind> ActionKindConverter
        {
            get
            {
                if (_actionKindConverter == null)
                    _actionKindConverter = new EnumConverter<ConvertedActionKind>();
                return _actionKindConverter;
            }
        }


    }
}
