using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC
    public class RemoveUnusedVariablesSyntaxRewriter : ALSyntaxRewriter
    {
        public SemanticModel SemanticModel { get; set; }
        public bool RemoveGlobalVariables { get; set; }
        public bool RemoveLocalVariables { get; set; }
        public bool RemoveLocalMethodParameters { get; set; }

        public RemoveUnusedVariablesSyntaxRewriter()
        {
            this.RemoveGlobalVariables = true;
            this.RemoveLocalVariables = true;
            this.RemoveLocalMethodParameters = true;
        }

        #region Visit objects to update variables

        public override SyntaxNode VisitCodeunit(CodeunitSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxList<MemberSyntax> newMembers, bool changed) = this.RemoveUnusedVariablesFromNode(node, node.Members);
                if (changed)
                    node = node.WithMembers(newMembers);
            }
            return base.VisitCodeunit(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxList<MemberSyntax> newMembers, bool changed) = this.RemoveUnusedVariablesFromNode(node, node.Members);
                if (changed)
                    node = node.WithMembers(newMembers);
            }
            return base.VisitPage(node);
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxList<MemberSyntax> newMembers, bool changed) = this.RemoveUnusedVariablesFromNode(node, node.Members);
                if (changed)
                    node = node.WithMembers(newMembers);
            }
            return base.VisitPageExtension(node);
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxList<MemberSyntax> newMembers, bool changed) = this.RemoveUnusedVariablesFromNode(node, node.Members);
                if (changed)
                    node = node.WithMembers(newMembers);
            }
            return base.VisitTable(node);
        }

        public override SyntaxNode VisitTableExtension(TableExtensionSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxList<MemberSyntax> newMembers, bool changed) = this.RemoveUnusedVariablesFromNode(node, node.Members);
                if (changed)
                    node = node.WithMembers(newMembers);
            }
            return base.VisitTableExtension(node);
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxList<MemberSyntax> newMembers, bool changed) = this.RemoveUnusedVariablesFromNode(node, node.Members);
                if (changed)
                    node = node.WithMembers(newMembers);
            }
            return base.VisitReport(node);
        }

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxList<MemberSyntax> newMembers, bool changed) = this.RemoveUnusedVariablesFromNode(node, node.Members);
                if (changed)
                    node = node.WithMembers(newMembers);
            }
            return base.VisitXmlPort(node);
        }

        public override SyntaxNode VisitQuery(QuerySyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxList<MemberSyntax> newMembers, bool changed) = this.RemoveUnusedVariablesFromNode(node, node.Members);
                if (changed)
                    node = node.WithMembers(newMembers);
            }
            return base.VisitQuery(node);
        }

        #endregion

        #region Remove usused variables from object members

        protected (SyntaxList<MemberSyntax>, bool) RemoveUnusedVariablesFromNode(SyntaxNode node, SyntaxList<MemberSyntax> members)
        {
            bool updated = false;               
                
            //prepare list of members
            List<MemberSyntax> membersList = members.ToList();

            //remove global variables
            if (this.RemoveGlobalVariables)
            {
                GlobalVarSectionSyntax globalVariables = members.Where(p => (p.Kind.ConvertToLocalType() == ConvertedSyntaxKind.GlobalVarSection)).FirstOrDefault() as GlobalVarSectionSyntax;
                (GlobalVarSectionSyntax newGlobalVariables, bool globalsUpdated) = this.RemoveUnusedGlobalVariablesFromNode(node, globalVariables);
                if (globalsUpdated)
                {
                    if (newGlobalVariables == null)
                        membersList.Remove(globalVariables);
                    else
                        membersList[members.IndexOf(globalVariables)] = newGlobalVariables;
                    updated = true;
                }
            }

            //remove local variables
            if ((this.RemoveLocalVariables) || (this.RemoveLocalMethodParameters))
            {
                for (int idx=0; idx<membersList.Count; idx++)
                {
                    if (membersList[idx] is MethodDeclarationSyntax methodDeclaration)
                    {
                        (MethodDeclarationSyntax newMethodDeclaration, bool methodUpdated) = this.RemoveUnusedLocalVariablesFromMethod(methodDeclaration);
                        if (methodUpdated)
                        {
                            membersList[idx] = newMethodDeclaration;
                            updated = true;
                        }
                    }
                }
            }

            //return results
            if (updated)
                return (SyntaxFactory.List(membersList), true);

            return (members, false);
        }


        protected (GlobalVarSectionSyntax, bool) RemoveUnusedGlobalVariablesFromNode(SyntaxNode node, GlobalVarSectionSyntax globalVariables)
        {
            //collect global variables names
            HashSet<string> deleteVariables = this.CollectVariableNames(globalVariables);

            //remove used global variables from the list
            this.RemoveReferencedGlobalVariablesFromNamesSet(deleteVariables, node);

            //remove unused global variables from the global variables section
            if (deleteVariables.Count > 0)
            {
                GlobalVarSectionSyntax newGlobalVarSection = this.RemoveGlobalVarSectionVariables(globalVariables, deleteVariables);
                return (newGlobalVarSection, true);
            }

            return (globalVariables, false);
        }

        protected (MethodDeclarationSyntax, bool) RemoveUnusedLocalVariablesFromMethod(MethodDeclarationSyntax methodDeclaration)
        {
            string returnValueName = null;
            SymbolInfo info = this.SemanticModel.GetSymbolInfo(methodDeclaration);
            IMethodSymbol symbol = SemanticModel.GetDeclaredSymbol(methodDeclaration) as IMethodSymbol;
            if ((symbol != null) && (symbol.LocalVariables != null))
            {
                bool removeMethodParameters = (this.RemoveLocalMethodParameters) && 
                    (symbol.IsLocal) && 
                    (symbol.MethodKind.ConvertToLocalType() != ConvertedMethodKind.Trigger) && 
                    (!symbol.IsEventSubscriber() &&
                    (!symbol.IsEvent) &&
                    (symbol.Parameters != null) &&
                    (methodDeclaration.ParameterList != null) &&
                    (methodDeclaration.ParameterList.Parameters != null));

                //collect local variables                
                Dictionary<string, ISymbol> deleteVariables = new Dictionary<string, ISymbol>();
                foreach (IVariableSymbol variableSymbol in symbol.LocalVariables)
                {
                    if (!variableSymbol.IsSynthesized)
                        deleteVariables.Add(variableSymbol.Name.ToLower(), variableSymbol);
                }

                //add return variable
                if (symbol.IsLocal)
                {
                    if ((symbol.ReturnValueSymbol != null) && (!symbol.ReturnValueSymbol.IsSynthesized) && (symbol.ReturnValueSymbol.IsNamed))
                    {
                        returnValueName = symbol.ReturnValueSymbol.Name.ToLower();
                        if (!String.IsNullOrWhiteSpace(returnValueName))
                            deleteVariables.Add(returnValueName, symbol.ReturnValueSymbol);
                    }

                    //remove parameters
                    if (removeMethodParameters)
                    {
                        foreach (ISymbol parameterSymbol in symbol.Parameters)
                        {
                            if (!parameterSymbol.IsSynthesized)
                            {
                                string parameterName = parameterSymbol.Name.ToLower();
                                if (!deleteVariables.ContainsKey(parameterName))
                                    deleteVariables.Add(parameterName, parameterSymbol);
                            }
                        }
                    }
                }

                //analyze variables
                if (methodDeclaration.Body != null)
                    this.RemoveReferencedVariables(deleteVariables, methodDeclaration.Body);

                //remove unused variables
                bool updated = false;
                if (deleteVariables.Count > 0)
                {
                    //remove variables
                    HashSet<string> deleteVariableNames = new HashSet<string>();
                    foreach (string name in deleteVariables.Keys)
                    {
                        deleteVariableNames.Add(name);
                    }

                    if (this.RemoveLocalVariables)
                    {
                        if ((methodDeclaration.Variables != null) && (methodDeclaration.Variables.Variables != null))
                        {
                            VarSectionSyntax varSectionSyntax = this.RemoveVarSectionVariables(methodDeclaration.Variables, deleteVariableNames);
                            methodDeclaration = methodDeclaration.WithVariables(varSectionSyntax);
                            updated = true;
                        }

                        //remove return value name
                        if ((!String.IsNullOrWhiteSpace(returnValueName)) && (deleteVariables.ContainsKey(returnValueName)))
                        {
                            methodDeclaration = methodDeclaration.WithReturnValue(methodDeclaration.ReturnValue.WithName(null));
                            updated = true;
                        }
                    }

                    //remove parameters
                    if (removeMethodParameters)
                    {
                        (SeparatedSyntaxList<ParameterSyntax> newParametersSyntax, bool parametersUpdated) = this.RemoveParametersVariables(methodDeclaration.ParameterList.Parameters, deleteVariableNames);
                        if (parametersUpdated)
                        {
                            methodDeclaration = methodDeclaration.WithParameterList(
                                methodDeclaration.ParameterList.WithParameters(newParametersSyntax));
                            updated = true;
                        }
                    }

                    return (methodDeclaration, updated);
                }
            }

            return (methodDeclaration, false);
        }

        #endregion

        #region Collect variable names from var section

        protected HashSet<string> CollectVariableNames(VarSectionBaseSyntax varSection)
        {
            string name;
            HashSet<string> namesCollection = new HashSet<string>();

            if (varSection.Variables != null)
            {
                foreach (VariableDeclarationBaseSyntax baseVariableDeclaration in varSection.Variables)
                {
                    switch (baseVariableDeclaration)
                    {
                        case VariableDeclarationSyntax variableDeclaration:
                            name = variableDeclaration.Name.Unquoted()?.ToLower();
                            if (!String.IsNullOrWhiteSpace(name))
                                namesCollection.Add(name);
                            break;
                        case VariableListDeclarationSyntax variableListDeclaration:
                            if (variableListDeclaration.VariableNames != null)
                            {
                                foreach (VariableDeclarationNameSyntax variableNameSyntax in variableListDeclaration.VariableNames)
                                {
                                    name = variableNameSyntax.Name.Unquoted()?.ToLower();
                                    if (!String.IsNullOrWhiteSpace(name))
                                        namesCollection.Add(name);
                                }
                            }
                            break;
                    }
                }
            }
            return namesCollection;
        }

        #endregion

        #region Remove variables from method parameters list

        protected (SeparatedSyntaxList<ParameterSyntax>, bool) RemoveParametersVariables(SeparatedSyntaxList<ParameterSyntax> parametersListSyntax, HashSet<string> deleteVariables)
        {
            bool updated = false;
            SeparatedSyntaxList<ParameterSyntax> newParametersListSyntax = parametersListSyntax;
            int idx = 0;

            while (idx < newParametersListSyntax.Count)
            {
                ParameterSyntax parameterSyntax = newParametersListSyntax[idx];
                string name = parameterSyntax.Name.Unquoted()?.ToLower();
                if ((!String.IsNullOrWhiteSpace(name)) && (deleteVariables.Contains(name)))
                {
                    updated = true;
                    newParametersListSyntax = newParametersListSyntax.RemoveAt(idx);
                }
                else
                    idx++;
            }

            return (newParametersListSyntax, updated);
        }

        #endregion

        #region Remove variables from var sections

        protected GlobalVarSectionSyntax RemoveGlobalVarSectionVariables(GlobalVarSectionSyntax node, HashSet<string> deleteVariables)
        {
            SyntaxList<VariableDeclarationBaseSyntax>? newVariablesSyntax = this.RemoveVarSectionVariables(node.Variables, deleteVariables);
            if (newVariablesSyntax == null)
                return null;
            return node.WithVariables(newVariablesSyntax.Value);
        }

        protected VarSectionSyntax RemoveVarSectionVariables(VarSectionSyntax node, HashSet<string> deleteVariables)
        {
            SyntaxList<VariableDeclarationBaseSyntax>? newVariablesSyntax = this.RemoveVarSectionVariables(node.Variables, deleteVariables);
            if (newVariablesSyntax == null)
                return null;
            return node.WithVariables(newVariablesSyntax.Value);
        }

        protected SyntaxList<VariableDeclarationBaseSyntax>? RemoveVarSectionVariables(SyntaxList<VariableDeclarationBaseSyntax> variablesSyntax, HashSet<string> deleteVariables)
        {
            List<VariableDeclarationBaseSyntax> keepVariables = new List<VariableDeclarationBaseSyntax>();
            foreach (VariableDeclarationBaseSyntax variableSyntax in variablesSyntax)
            {
                switch (variableSyntax)
                {
                    case VariableListDeclarationSyntax variableListSyntax:
                        variableListSyntax = this.RemoveVariableListVariables(variableListSyntax, deleteVariables);
                        if (variableListSyntax != null)
                            keepVariables.Add(variableListSyntax);
                        break;
                    case VariableDeclarationSyntax variableDeclarationSyntax:
                        string name = variableDeclarationSyntax.Name.Unquoted()?.ToLower();
                        if (!deleteVariables.Contains(name))
                            keepVariables.Add(variableSyntax);
                        break;
                }
            }

            if (keepVariables.Count == 0)
                return null;

            return SyntaxFactory.List(keepVariables);
        }

        protected VariableListDeclarationSyntax RemoveVariableListVariables(VariableListDeclarationSyntax variableList, HashSet<string> deleteVariables)
        {
            List<VariableDeclarationNameSyntax> keepNames = new List<VariableDeclarationNameSyntax>();
            bool listChanged = false;
            foreach (VariableDeclarationNameSyntax variableNameSyntax in variableList.VariableNames)
            {
                string name = variableNameSyntax.Name.Unquoted()?.ToLower();
                if (deleteVariables.Contains(name))
                    listChanged = true;
                else
                    keepNames.Add(variableNameSyntax);
            }

            if (listChanged)
            {
                if (keepNames.Count == 0)
                    return null;

                var newNamesList = new SeparatedSyntaxList<VariableDeclarationNameSyntax>();
                newNamesList = newNamesList.AddRange(keepNames);
                return variableList.WithVariableNames(newNamesList);
            }

            return variableList;
        }

        #endregion

        #region Process syntax nodes and remove referenced variables from the list of variables

        protected void RemoveReferencedVariables(Dictionary<string, ISymbol> deleteVariables, SyntaxNode node)
        {
            if (node is IdentifierNameSyntax identifierNameSyntax)
            {
                string name = identifierNameSyntax.Unquoted()?.ToLower();
                if (deleteVariables.ContainsKey(name))
                {
                    SymbolInfo symbolInfo = this.SemanticModel.GetSymbolInfo(identifierNameSyntax);
                    if ((symbolInfo != null) && (symbolInfo.Symbol != null) && (deleteVariables[name] == symbolInfo.Symbol))
                        deleteVariables.Remove(name);
                }
            }

            IEnumerable<SyntaxNode> syntaxNodes = node.ChildNodes();
            foreach (SyntaxNode childNode in syntaxNodes)
            {
                this.RemoveReferencedVariables(deleteVariables, childNode);
            }
        }

        protected void RemoveReferencedGlobalVariablesFromNamesSet(HashSet<string> namesCollection, SyntaxNode node)
        {
            ConvertedSyntaxKind nodeKind = node.Kind.ConvertToLocalType();
            switch (nodeKind)
            {
                case ConvertedSyntaxKind.GlobalVarSection:
                case ConvertedSyntaxKind.VarSection:
                case ConvertedSyntaxKind.VariableDeclaration:
                case ConvertedSyntaxKind.ParameterList:
                case ConvertedSyntaxKind.ReturnValue:
                    return;
            }

            if (node is IdentifierNameSyntax identifierNameSyntax)
            {
                string name = identifierNameSyntax.Unquoted()?.ToLower();
                if (namesCollection.Contains(name))
                {
                    SymbolInfo symbolInfo = this.SemanticModel.GetSymbolInfo(identifierNameSyntax);
                    if ((symbolInfo != null) && (symbolInfo.Symbol != null) && (symbolInfo.Symbol.Kind.ConvertToLocalType() == ConvertedSymbolKind.GlobalVariable))
                        namesCollection.Remove(name);
                }
            }

            IEnumerable<SyntaxNode> syntaxNodes = node.ChildNodes();
            foreach (SyntaxNode childNode in syntaxNodes)
            {
                this.RemoveReferencedGlobalVariablesFromNamesSet(namesCollection, childNode);
            }
        }

        #endregion

    }

#endif
}
