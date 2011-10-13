using System;
using System.Web;
using NodeTreeCms.Models;
namespace NodeTreeCms
{
    public interface ICms
    {

        DataProviders.IDataProvider DbProvider { get; set; }
        IDocumentNode DocumentNodeByUrl(string url);
        IDocumentNode NodeTree { get; }
        Cms.NodeTreeCmsSettings Settings { get; }
        HtmlString AdminPage(string url, dynamic formPost = null);

    }
}
