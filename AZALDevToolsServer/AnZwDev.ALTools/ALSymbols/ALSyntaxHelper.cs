using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSyntaxHelper
    {

        public static bool NameNeedsEcoding(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                for (int i = 0; i < name.Length; i++)
                {
                    char nameChar = name[i];
                    if (!(
                        ((nameChar >= 'a') && (nameChar <= 'z')) ||
                        ((nameChar >= 'A') && (nameChar <= 'Z')) ||
                        ((nameChar >= '0') && (nameChar <= '9')) ||
                        (nameChar == '_')))
                        return true;
                }
            }
            return false;
        }

        public static string DecodeName(string name)
        {
            if (name != null)
            {
                name = name.Trim();
                if (name.StartsWith("\""))
                {
                    name = name.Substring(1);
                    if (name.EndsWith("\""))
                        name = name.Substring(0, name.Length - 1);
                    name = name.Replace("\"\"", "\"");
                }
            }
            return name;
        }

        public static string EncodeName(string name)
        {
            if (NameNeedsEcoding(name))
                return "\"" + name.Replace("\"", "\"\"") + "\"";
            return name;
        }

        public static string EncodeNamesList(string[] names)
        {
            if ((names != null) && (names.Length > 0))
            {
                string list = names[0];
                for (int i=1; i<names.Length; i++)
                {
                    list = list + ", " + EncodeName(names[i]);
                }
                return list;
            }
            return "";
        }

        public static ALSymbolKind MemberAttributeToMethodKind(string name)
        {
            //events
            if (name.Equals("IntegrationEvent", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.IntegrationEventDeclaration;
            if (name.Equals("BusinessEvent", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.BusinessEventDeclaration;
            if (name.Equals("EventSubscriber", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.EventSubscriberDeclaration;
            //tests
            if (name.Equals("Test", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.TestDeclaration;
            if (name.Equals("ConfirmHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.ConfirmHandlerDeclaration;
            if (name.Equals("FilterPageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.FilterPageHandlerDeclaration;
            if (name.Equals("HyperlinkHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.HyperlinkHandlerDeclaration;
            if (name.Equals("MessageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.MessageHandlerDeclaration;
            if (name.Equals("ModalPageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.ModalPageHandlerDeclaration;
            if (name.Equals("PageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.PageHandlerDeclaration;
            if (name.Equals("ReportHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.ReportHandlerDeclaration;
            if (name.Equals("RequestPageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.RequestPageHandlerDeclaration;
            if (name.Equals("SendNotificationHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.SendNotificationHandlerDeclaration;
            if (name.Equals("SessionSettingsHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.SessionSettingsHandlerDeclaration;
            if (name.Equals("StrMenuHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.StrMenuHandlerDeclaration;

            return ALSymbolKind.Undefined;
        }


    }
}
