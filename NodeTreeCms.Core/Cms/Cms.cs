using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiveDomain.Core;
using System.IO;
using NodeTreeCms.Models;
using NodeTreeCms.DataProviders;
namespace NodeTreeCms
{
    public class Cms : ICms
    {
        public class NodeTreeCmsSettings
        {
            /// <summary>
            /// Url to admin page, default "/admin"
            /// </summary>
            public string AdminUrl { get; set; }
            ///// <summary>
            ///// Url to root content, default "", is being added to all content automatically
            ///// and removed from the url when looking for content node.
            ///// </summary>
            //public string ContentRootUrl { get; set; }
            /// <summary>
            /// Path to database, default App_Code/CmsDb
            /// </summary>
            public string DbPath { get; set; }
            /// <summary>
            /// Path to default content View, default Default.cshtml
            /// </summary>
            public string DefaultViewPath { get; set; }
        }

        private NodeTreeCmsSettings settings;
        public NodeTreeCmsSettings Settings { get { return settings; } }

        private IDataProvider dbProvider;
        private IDocumentNode nodeTree;
        
        public IDocumentNode NodeTree
        {
            get
            {
                if (DbProvider.NodeTreeIsUpdated)
                    nodeTree = dbProvider.DocumentNode_NodeTree();
                return nodeTree;
            }
        }
        public Cms(NodeTreeCmsSettings settings = null)
        {
            Initialize(settings);
        }

        public IDataProvider DbProvider
        {
            get
            {
                if (dbProvider == null)
                    Initialize(settings);
                return dbProvider;
            }
            set
            {
                dbProvider = value;
            }
        }
                
        public void Initialize(NodeTreeCmsSettings settings)
        {

            // Control settings

            if (settings == null) settings = new NodeTreeCmsSettings();

            if (String.IsNullOrEmpty(settings.DbPath))
                settings.DbPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Cms");

            if (String.IsNullOrEmpty(settings.AdminUrl))
                settings.AdminUrl="/admin";

            if (!settings.AdminUrl.StartsWith("/")) settings.AdminUrl = "/" + settings.AdminUrl;

            if (String.IsNullOrEmpty(settings.DefaultViewPath))
                settings.DefaultViewPath = "Default.cshtml";

            //if (String.IsNullOrEmpty(settings.ContentRootUrl))
            //    settings.ContentRootUrl = "/";    

            this.settings = settings;

            var path = settings.DbPath;
            dbProvider = new LiveDbProvider(path);

            nodeTree = dbProvider.DocumentNode_NodeTree();

            if (nodeTree == null)
                InitializeDocumentNodes();

            dbProvider.UpdateDatabaseImage();
        }

        private void InitializeDocumentNodes()
        {
            var rootNode = new DocumentNode("root");

            rootNode.HideHeader = true;
            rootNode.Body = "<h1>Welcome to NodeTreeCms.</h1><p>Add content by going to <a href=\"" + settings.AdminUrl + "\">the admin section.</a></p>";
            rootNode.ExtraContent3 = "This is a minimalistic CMS by @joeriks";
            rootNode.Author = "Jonas Eriksson";

            dbProvider.DocumentNode_Insert(rootNode, null);
        }

        public IDocumentNode DocumentNodeByUrl(string url)
        {

            if (url.StartsWith(this.Settings.AdminUrl)) url = url.Remove(0, this.Settings.AdminUrl.Length);

            var node = this.NodeTree.GetDescendantNodes(n => (n.UrlName == "root" || url.StartsWith(n.Url))).LastOrDefault();
            if (node == null) node = this.NodeTree;
            if (String.IsNullOrEmpty(node.ViewPath)) node.ViewPath = settings.DefaultViewPath;
            return node;
        }

        public HtmlString AdminPage(string url, dynamic formPost = null)
        {
            if (url.StartsWith(this.Settings.AdminUrl)) url = url.Remove(0, this.Settings.AdminUrl.Length);

            //NameValueCollection

            var doc = new System.Text.StringBuilder();
            doc.Append(@"<style type=""text/css"">
                div.admin-tree { width:300px; float:left }
                div.admin-form { width:600px; float:left }
                </style>
            ");

            var node = this.DocumentNodeByUrl(url);

            if (formPost != null)
            {
                if (formPost["insert-new"] != null && formPost["new-name"] != null)
                {
                    this.DbProvider.DocumentNode_Insert(new DocumentNode(formPost["new-name"]), node);
                }

                if (formPost["update"] != null)
                {
                    node = ModelHelper.UpdateModelFirst(node, formPost);
                    this.DbProvider.DocumentNode_Update(node);

                    if (node.IsDeleted)
                    {
                        node = node.Parent;
                    }
                }
            }

            doc.Append(HtmlAdmin.AdminTree((DocumentNode)node, settings.AdminUrl, "admin-tree").ToString());
            doc.Append(HtmlAdmin.AdminForm((DocumentNode)node,settings.AdminUrl, "admin-form").ToString());

            return new HtmlString(doc.ToString());

        }

    }
}