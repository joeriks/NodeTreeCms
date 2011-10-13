using NodeTreeCms;
using Nancy;

public class ContentModule : NancyModule
{
    public ContentModule(ICms nodeTreeCms)
    {
        Get["/"] = Get["/{any}"] = _ =>
        {
            var node = nodeTreeCms.DocumentNodeByUrl(Request.Url.Path);
            return View[node.ViewPath, node];
        };
    }
}
