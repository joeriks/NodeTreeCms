using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlTags;

namespace NodeTreeCms.Models
{
    public class HtmlAdmin
    {

        public static HtmlString AdminForm(DocumentNode Model, string adminUrl, string divClassName = "")
        {
            var div = new HtmlTag("div");

            if (divClassName != "") div.AddClass(divClassName);

            var form = new FormTag().Method("post").Action("#");

            form.Append(HtmlBuilder.HtmlTagLabelInput("Name (header)", "name", Model.Name));
            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("name", "update").Attr("value", "Update"));
            form.Append(HtmlBuilder.HtmlTagLabelCheckbox("Hide header", "hideHeader", Model.HideHeader));
            form.Append(HtmlBuilder.HtmlTagLabelTextArea("Body text", "body", Model.Body));
            form.Append(HtmlBuilder.HtmlTagLabelTextArea("Extra content 1", "extraContent1", Model.ExtraContent1, 5));
            //form.Append(HtmlBuilder.HtmlTagLabelTextArea("Extra content 2", "extraContent2", Model.ExtraContent2, 5));
            //form.Append(HtmlBuilder.HtmlTagLabelTextArea("Extra content 3", "extraContent3", Model.ExtraContent3, 3));
            form.Append(HtmlBuilder.HtmlTagLabelInput("Author", "author", Model.Author));
            form.Append(HtmlBuilder.HtmlTagLabelInput("ViewPath", "viewPath", Model.ViewPath));
            form.Append(HtmlBuilder.HtmlTagLabelCheckbox("Hidden", "isHidden", Model.IsHidden));
            form.Append(HtmlBuilder.HtmlTagLabelCheckbox("Deleted", "isDeleted", Model.IsDeleted));
            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("name", "update").Attr("value", "Update"));

            if (!String.IsNullOrEmpty(Model.Url))
            {
                form.Append(new HtmlTag("p").Append(new HtmlTag("a").Attr("href", Model.Url).Text("View page")));
            }

            div.Append(form);
            return new HtmlString(div.ToHtmlString());
        }
        public static HtmlString AdminTree(IDocumentNode Model, string adminUrl, string divClassName = "")
        {

            var div = new HtmlTag("div");
            if (divClassName != "") div.AddClass(divClassName);

            var p = new HtmlTag("p").Append(new HtmlTag("a").Text("Root node").Attr("href", adminUrl));
            var tree = HtmlBuilder.ChildNodesRecursiveHtmlTag(Model, Model.AncestorAtLevel(0), 99, true, false, adminUrl);


            var pInfo = new HtmlTag("p").Text("Add new child");

            var form = new HtmlTag("form").Attr("method", "post").Attr("action", "#");

            form.Append(new HtmlTag("label").Attr("for", "new-name").Text("Header (name)"));
            form.Append(new HtmlTag("input").Id("new-name").Attr("name", "new-name").Attr("type", "text"));
            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("name", "insert-new").Attr("value", "Add"));

            div.Append(p).Append(tree).Append(pInfo).Append(form);

            return new HtmlString(div.ToHtmlString());

        }

    }
}