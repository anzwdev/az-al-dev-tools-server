using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class EnumExtensions
    {

        public static DestEnum Convert<SourceEnum, DestEnum>(this SourceEnum source) where DestEnum : Enum where SourceEnum : Enum
        {
            try
            {
                return (DestEnum)Enum.Parse(typeof(DestEnum), source.ToString(), true);
            }
            catch (Exception)
            {
                return default(DestEnum);
            }
        }

        internal static ConvertedSyntaxKind ConvertToLocalType(this SyntaxKind source)
        {
            return source.Convert<SyntaxKind, ConvertedSyntaxKind>();
        }

        internal static ConvertedControlKind ConvertToLocalType(this ControlKind source)
        {
            return source.Convert<ControlKind, ConvertedControlKind>();
        }

        internal static ConvertedChangeKind ConvertToLocalType(this ChangeKind source)
        {
            return source.Convert<ChangeKind, ConvertedChangeKind>();
        }

        internal static ConvertedActionKind ConvertToLocalType(this ActionKind source)
        {
            return source.Convert<ActionKind, ConvertedActionKind>();
        }

    }
}
