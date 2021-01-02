using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SyntaxNodesGroupsTree<T> where T: SyntaxNode
    {

        public SyntaxNodesGroup<T> Root { get; set; }

        public SyntaxNodesGroupsTree()
        {
            this.Root = null;
        }

        #region Add nodes to the tree

        public bool AddNodes(IEnumerable<T> nodesCollection)
        {
            this.Root = new SyntaxNodesGroup<T>();
            SyntaxNodesGroup<T> group = this.Root;
            foreach (T node in nodesCollection)
            {
                group = this.AddNode(group, node);
                if (group == null)
                {
                    this.Root = null;
                    return false;
                }
            }
            return true;
        }

        protected SyntaxNodesGroup<T> AddNode(SyntaxNodesGroup<T> group, T node)
        {
            SyntaxTriviaList triviaList = node.GetLeadingTrivia();

            if ((triviaList != null) && (triviaList.Count > 0))
            {
                //collect regions
                List<SyntaxTrivia> triviaCache = new List<SyntaxTrivia>();
                bool hasGroups = false;

                foreach (SyntaxTrivia trivia in triviaList)
                {
                    triviaCache.Add(trivia);
                    switch (trivia.Kind)
                    {
                        case SyntaxKind.RegionDirectiveTrivia:
                            SyntaxNodesGroup<T> childGroup = new SyntaxNodesGroup<T>();
                            childGroup.LeadingTrivia = triviaCache;
                            group.AddGroup(childGroup);
                            group = childGroup;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                        case SyntaxKind.EndRegionDirectiveTrivia:
                            group.TrailingTrivia = triviaCache;
                            group = group.ParentGroup;
                            if (group == null)
                                return null;
                            triviaCache = new List<SyntaxTrivia>();
                            hasGroups = true;
                            break;
                    }
                }

                if (hasGroups)
                    node = node.WithLeadingTrivia(triviaCache);
            }

            group.SyntaxNodes.Add(node);
            return group;
        }

        #endregion

        public SyntaxList<T> CreateSyntaxList()
        {
            List<T> nodesList = new List<T>();
            this.Root.GetSyntaxNodes(nodesList);
            return SyntaxFactory.List<T>(nodesList);
        }

        public List<SyntaxNodesGroup<T>> GetAllGroups()
        {
            List<SyntaxNodesGroup<T>> list = new List<SyntaxNodesGroup<T>>();
            if (this.Root != null)
                this.Root.GetAllGroups(list);
            return list;
        }

    }
}
