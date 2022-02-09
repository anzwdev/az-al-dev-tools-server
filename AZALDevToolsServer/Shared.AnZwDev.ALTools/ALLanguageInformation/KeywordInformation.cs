using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public static class KeywordInformation
    {

        private static readonly HashSet<string> _codeKeywords = new HashSet<string>(new string[] {
            "false", "true", "div", "modulo", "and", "or", "xor", "not", "exit", "begin",
            "case", "do", "downto", "else", "end", "for", "if", "in", "of", "repeat", "then",
            "to", "until", "with", "while", "program", "procedure", "function", "var", 
            "asserterror", "foreach", "break", "with" });

        public static bool IsCodeKeyword(string keyword)
        {
            if (keyword != null)
                return _codeKeywords.Contains(keyword.ToLower());
            return false;
        }

    }
}
