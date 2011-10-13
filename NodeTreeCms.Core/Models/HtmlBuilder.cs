using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NodeTreeCms.Models;
using HtmlTags;
using System.Collections.Specialized;

namespace NodeTreeCms
{
    public static class HtmlBuilder
    {
        public static HtmlString BreadCrumb(this IDocumentNode IDocumentNode)
        {
            var tag = "";
            var nodeNavigator = IDocumentNode;
            while (nodeNavigator.Parent != null)
            {
                tag = "<a href=" + nodeNavigator.Url + ">" + nodeNavigator.Name + "</a> / " + tag;
                nodeNavigator = nodeNavigator.Parent;
            }

            tag = "<a href=\"/\">Home</a> / " + tag;
            return new HtmlString(tag);
        }
        public static HtmlString ChildNodes(this IDocumentNode IDocumentNode, int atLevel = 0, bool includeHidden = false, bool includeDeleted = false)
        {
            var ul = new HtmlTags.HtmlTag("ul");
            ul.AddClass("topnavigation");

            foreach (var c in IDocumentNode.AncestorAtLevel(atLevel).Children.Where(n => (includeDeleted || !n.IsDeleted) && (includeHidden || !n.IsHidden)))
            {
                var li = new HtmlTags.HtmlTag("li");
                if (IDocumentNode.IsDescendantOrSameAs(c)) li.AddClass("selected");
                li.Add("a").Attr("href", c.Url).Text(c.Name);
                ul.Children.Add(li);
            }
            return new HtmlString(ul.ToHtmlString());
        }
        public static HtmlString ChildNodesRecursive(this IDocumentNode currentNode, int fromLevel = 1, int allExpandToLevel = 2, bool includeHidden = false, bool includeDeleted = false, string addAdminPath = "")
        {
            var fromNode = currentNode.AncestorAtLevel(fromLevel);
            if (fromNode != null)
                return new HtmlString(ChildNodesRecursiveHtmlTag(fromNode, currentNode, allExpandToLevel, includeHidden, includeDeleted, addAdminPath).ToHtmlString());
            else
                return new HtmlString("");
        }
        public static HtmlTag ChildNodesRecursiveHtmlTag(this IDocumentNode currentNode, IDocumentNode IDocumentNode, int allExpandToLevel = 2, bool includeHidden = false, bool includeDeleted = false, string addAdminPath = "")
        {
            var ul = new HtmlTags.HtmlTag("ul");
            foreach (var c in IDocumentNode.Children.Where(n => (includeDeleted || !n.IsDeleted) && (includeHidden || !n.IsHidden)))
            {
                var li = new HtmlTags.HtmlTag("li");

                var path = addAdminPath + c.Url;

                li.Add("a").Attr("href", path).Text(c.Name);
                if (c == currentNode)
                {
                    li.AddClass("selected");
                }
                if (c.IsDescendantOrSameAs(currentNode)) li.AddClass("sel");
                if (c.Children.Count > 0 && (c.Level < allExpandToLevel || c.IsDescendantOrSameAs(currentNode) || currentNode.IsDescendantOrSameAs(c)))
                {
                    li.Children.Add(ChildNodesRecursiveHtmlTag(currentNode, c, allExpandToLevel, includeHidden, includeDeleted, addAdminPath));
                }
                ul.Children.Add(li);
            }
            return ul;
        }
        public static HtmlTag HtmlTagLabelCheckbox(string label, string name, bool value)
        {
            var d = new HtmlTag("div");
            d.Append(new HtmlTag("label").Attr("for", name).Text(label));
            if (value)
                d.Append(new HtmlTag("input").Attr("type", "checkbox").Id(name).Attr("name", name).Attr("checked", "checked"));
            else
                d.Append(new HtmlTag("input").Attr("type", "checkbox").Id(name).Attr("name", name).Attr("value", value));
            d.Append(new HtmlTag("input").Attr("type", "hidden").Id(name).Attr("name", name).Attr("value", ""));
            return d;
        }
        public static HtmlTag HtmlTagLabelInput(string label, string name, string value)
        {
            var d = new HtmlTag("div");
            d.Append(new HtmlTag("label").Attr("for", name).Text(label));
            d.Append(new HtmlTag("input").Attr("type", "text").Id(name).Attr("name", name).Attr("value", value));
            return d;
        }
        public static HtmlTag HtmlTagLabelTextArea(string label, string name, string value, int rows = 10)
        {
            var d = new HtmlTag("div");
            d.Append(new HtmlTag("label").Attr("for", name).Text(label));
            d.Append(new HtmlTag("textarea").Id(name).Attr("name", name).Attr("cols", "80").Attr("rows", rows.ToString()).Text(value));
            return d;
        }
    }
}