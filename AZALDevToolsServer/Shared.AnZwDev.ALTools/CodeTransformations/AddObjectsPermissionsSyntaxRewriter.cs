using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AddObjectsPermissionsSyntaxRewriter : ALSyntaxRewriter
    {

        public AddObjectsPermissionsSyntaxRewriter()
        {
        }

#if BC

        public override SyntaxNode VisitPermissionSet(PermissionSetSyntax node)
        {
            PropertyListSyntax properties = this.UpdateProperties(node.PropertyList);
            node = node.WithPropertyList(properties);

            return base.VisitPermissionSet(node);
        }

        public override SyntaxNode VisitPermissionSetExtension(PermissionSetExtensionSyntax node)
        {
            PropertyListSyntax properties = this.UpdateProperties(node.PropertyList);
            node = node.WithPropertyList(properties);
            return base.VisitPermissionSetExtension(node);
        }

#endif

        protected PropertyListSyntax UpdateProperties(PropertyListSyntax properties)
        {
            PropertySyntax permissionsPropertySyntax = properties.GetPropertyEntry("Permissions");
            if (permissionsPropertySyntax == null)
            {
                permissionsPropertySyntax = this.CreatePermissionsProperty();
                properties = properties.AddProperties(permissionsPropertySyntax);
            } 
            else
            {
                PropertySyntax newProperty = this.UpdatePermissionsProperty(permissionsPropertySyntax);
                properties = properties.WithProperties(properties.Properties.Replace(permissionsPropertySyntax, newProperty));
            }
            return properties;
        }

        protected PropertySyntax CreatePermissionsProperty()
        {
            PermissionPropertyValueSyntax propertyValueSyntax = this.CreatePermissionPropertyValue();
            return SyntaxFactory.Property("Permissions", propertyValueSyntax);
        }

        protected PropertySyntax UpdatePermissionsProperty(PropertySyntax propertySyntax)
        {
            PermissionPropertyValueSyntax propertyValueSyntax = propertySyntax.Value as PermissionPropertyValueSyntax;
            if (propertyValueSyntax != null)
                propertyValueSyntax = this.UpdatePermissionPropertyValue(propertyValueSyntax);
            else
                propertyValueSyntax = this.CreatePermissionPropertyValue();
            return propertySyntax.WithValue(propertyValueSyntax);            
        }

        protected PermissionPropertyValueSyntax UpdatePermissionPropertyValue(PermissionPropertyValueSyntax node)
        {
            //collect existing permissions
            HashSet<string> existingPermissions = new HashSet<string>();
            if (node.PermissionProperties != null)
            {
                foreach (PermissionSyntax permissionSyntax in node.PermissionProperties)
                {
                    if ((permissionSyntax.ObjectType != null) &&
                        (permissionSyntax.ObjectReference != null) &&
                        (permissionSyntax.ObjectReference.Identifier != null))
                    {
                        string objectUID = this.CreateObjectUID(
                            ALSyntaxHelper.DecodeName(permissionSyntax.ObjectType.ToString()),
                            ALSyntaxHelper.DecodeName(permissionSyntax.ObjectReference.Identifier.ToString()));
                        if (objectUID != null)
                            existingPermissions.Add(objectUID);
                    }
                }
            }
            List<PermissionSyntax> permissionSyntaxList = this.CreatePermissions(existingPermissions);
            return node.AddPermissionProperties(permissionSyntaxList.ToArray());
        }

        protected PermissionPropertyValueSyntax CreatePermissionPropertyValue()
        {
            List<PermissionSyntax> permissionSyntaxList = this.CreatePermissions(null);
            SeparatedSyntaxList<PermissionSyntax> permissions = new SeparatedSyntaxList<PermissionSyntax>();
            permissions = permissions.AddRange(permissionSyntaxList);
            return SyntaxFactory.PermissionPropertyValue(permissions);
        }

        protected string CreateObjectUID(string type, string name)
        {
            if ((String.IsNullOrWhiteSpace(type)) || (String.IsNullOrWhiteSpace(name)))
                return null;
            return type.ToLower() + "_" + name.ToLower();
        }

        protected List<PermissionSyntax> CreatePermissions(HashSet<string> existingPermissions)
        {
            HashSet<ALSymbolKind> includeObjects = new HashSet<ALSymbolKind>();
            includeObjects.AddObjectsWithPermissions();
            ObjectInformationProvider provider = new ObjectInformationProvider();
            List<ObjectInformation> objectsList = provider.GetProjectObjects(this.Project, includeObjects, false);

            SyntaxTriviaList leadingTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace("\r\n"), SyntaxFactory.WhiteSpace("        "));
            SyntaxTriviaList spaceTriviaList = SyntaxFactory.TriviaList(SyntaxFactory.WhiteSpace(" "));

#if BC
            SyntaxToken equalsToken = SyntaxFactoryHelper.Token(spaceTriviaList, "EqualsToken", spaceTriviaList);
#else
            SyntaxToken equalsToken = SyntaxFactoryHelper.Token("SyntaxKind.EqualsToken").WithLeadingTrivia(spaceTriviaList).WithTrailingTrivia(spaceTriviaList);
#endif

            SyntaxToken executePermissions = SyntaxFactory.Identifier("X");
            SyntaxToken tableDataPermissions = SyntaxFactory.Identifier("RMID");

            List<PermissionSyntax> permissionsList = new List<PermissionSyntax>();
            foreach (ObjectInformation objectInformation in objectsList)
            {
                this.AddPermission(existingPermissions, permissionsList, objectInformation.Type, objectInformation.Name, equalsToken, executePermissions, leadingTriviaList, spaceTriviaList);
                if ((objectInformation.Type != null) && (objectInformation.Type.Equals("table", StringComparison.CurrentCultureIgnoreCase)))
                    this.AddPermission(existingPermissions, permissionsList, "tabledata", objectInformation.Name, equalsToken, tableDataPermissions, leadingTriviaList, spaceTriviaList);
            }

            permissionsList.Sort(new PermissionComparer());
            return permissionsList;
        }

        protected void AddPermission(HashSet<string> existingPermissions, List<PermissionSyntax> permissionsList, string type, string name, SyntaxToken equalsToken, SyntaxToken permissionsToken, SyntaxTriviaList leadingTriviaList, SyntaxTriviaList spaceTriviaList)
        {
            string uid = this.CreateObjectUID(type, name);
            if ((uid == null) || ((existingPermissions != null) && (existingPermissions.Contains(uid))))
                return;

            SyntaxToken objectType = this.CreateObjectTypeToken(type);
            ObjectNameOrIdSyntax objectName = SyntaxFactory.ObjectNameOrId(SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(name)))
                .WithLeadingTrivia(spaceTriviaList);
#if BC
            PermissionSyntax permission = SyntaxFactory.Permission(objectType, objectName, default(SyntaxToken), equalsToken, permissionsToken)
                .WithLeadingTrivia(leadingTriviaList);
#else
            PermissionSyntax permission = SyntaxFactory.Permission(objectType, objectName, equalsToken, permissionsToken)
                .WithLeadingTrivia(leadingTriviaList);
#endif

            permissionsList.Add(permission);
        }

        protected SyntaxToken CreateObjectTypeToken(string type)
        {
            if (type != null)
            {
                type = type.ToLower();
                switch (type)
                {
                    case "table":
                        return SyntaxFactoryHelper.Token("TableKeyword");
                    case "tabledata":
                        return SyntaxFactoryHelper.Token("TableDataKeyword");
                    case "page":
                        return SyntaxFactoryHelper.Token("PageKeyword");
                    case "codeunit":
                        return SyntaxFactoryHelper.Token("CodeunitKeyword");
                    case "xmlport":
                        return SyntaxFactoryHelper.Token("XmlPortKeyword");
                    case "report":
                        return SyntaxFactoryHelper.Token("ReportKeyword");
                    case "query":
                        return SyntaxFactoryHelper.Token("QueryKeyword");
                }
            }
            return SyntaxFactoryHelper.Token("CodeunitKeyword");
        }

    }
}
