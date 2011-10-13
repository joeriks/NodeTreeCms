using System;
using System.Collections.Generic;
using System.Linq;
using NodeTreeCms.Models;

namespace NodeTreeCms.Models
{
    public static class IDocumentNodeExtensionMethods
    {
        public static IDocumentNode DocumentNodeByGuid(this IDocumentNode node, Guid guid)
        {
            if (node.Id == guid) return node;
            else
                return node.GetDescendantNodes(n => n.Id == guid, true).FirstOrDefault();
        }

        public static IEnumerable<IDocumentNode> GetDescendantNodes(this IDocumentNode node, Func<IDocumentNode, bool> func, bool deepSearch = false)
        {
            foreach (DocumentNode child in node.Children)
            {
                if (func(child))
                {
                    yield return child;

                    foreach (var descendant in child.GetDescendantNodes(func, deepSearch))
                    {
                        yield return descendant;
                    }
                }
                else if (deepSearch)
                {
                    foreach (var descendant in child.GetDescendantNodes(func, deepSearch))
                    {
                        yield return descendant;
                    }
                }
            }
        }
        public static IDocumentNode AncestorAtLevel(this IDocumentNode documentNode, int level = 0)
        {
            if (documentNode.Level < level) return null;

            var navigator = documentNode;
            while (navigator.Parent != null && navigator.Level > level)
            {
                navigator = navigator.Parent;
            }
            return navigator;
        }

        public static bool IsDescendantOrSameAs(this IDocumentNode thisDocumentNode, IDocumentNode documentNode)
        {
            var result = false;
            var navigator = thisDocumentNode;
            if (navigator == documentNode) return true;
            while (navigator.Parent != null)
            {
                navigator = navigator.Parent;
                if (navigator == documentNode) result = true;
            }
            return result;
        }

        public static void UpdateContextData(this IDocumentNode documentNode, IDocumentNode parent = null)
        {
            var url = "";

            documentNode.Parent = parent;
            if (parent != null)
            {
                url = parent.Url;
                documentNode.Level = parent.Level + 1;
            }
            else
            {
                documentNode.Level = 0;
            }
            if (String.IsNullOrEmpty(documentNode.UrlName)) { documentNode.UrlName = documentNode.Name.ToLower().Replace(" ", "-"); }
            if (url == "" && documentNode.Name == "root")
            {
                documentNode.Url = "";
            }
            else
                documentNode.Url = url + "/" + documentNode.UrlName;
            foreach (var c in documentNode.Children)
            {
                c.UpdateContextData(documentNode);
            }
        }

    }
}