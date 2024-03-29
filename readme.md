NodeTreeCms - version 0.1
=========================

NodeTreeCms is a _really_ basic CMS - the goal is to have one dll to drop into any WebPages or Nancy app to add a simple
way to handle content. With a possibility to edit content in a admin UI, serve content pages
(by requested url) and easily build and display navigation trees.


Demo
----

Demo currently available at http://nodetreecms.xapp.se


Works in current version
------------------------

* Content nodes "NodeDocuments" in a tree structure, representing url structure
* Content properties in each node, like name, bodytext, extracontent and hide in navigation
* Each node has also a ViewPath property which can be used to specify which View (f ex Razor)
* In the initialization it's possible to select database path, default view path and admin path
* Admin UI with a possibility to add / update / delete nodes
* Works with [NancyFx](http://nancyfx.org)

Work in progress:
-----------------

* Security for the admin UI
* A simple install (nuget) with a single, or very few files
* Should work in Asp.Net WebPages


The database
------------

Currently the CMS is using the very efficient in-memory no-sql database [LiveDb](http://livedb.devrex.se) 

It works seamlessly and requires no effort to be installed or setup.


Usage in Nancy
--------------

In Nancy the CMS content serving is easy to setup with just a module like this:

    public ContentModule(ICms nodeTreeCms)
    {
        Get["/"] = Get["/{any}"] = _ =>
        {
            var node = nodeTreeCms.DocumentNodeByUrl(Request.Url.Path);
            return View[node.ViewPath, node];
        };
    }

The DocumentNodeByUrl retrives the node at the requested Url.

That's all there is to it to make it work. The module for the Admin UI is included in the core dll.


Defining settings in Nancy
--------------------------

By adding a Nancy bootstrapper it's possible to define the settings (otherwise it's using defaults) :

    public class NancyBootStrapper : DefaultNancyBootstrapper
    {
        protected override void RegisterInstances(TinyIoC.TinyIoCContainer container, IEnumerable<Nancy.Bootstrapper.InstanceRegistration> instanceRegistrations)
        {
            base.RegisterInstances(container, instanceRegistrations);
            var settings = new NodeTreeCms.Cms.NodeTreeCmsSettings()
            {
                DbPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/NodeTreeCmsLiveDb"),
                AdminUrl = "/admin",
                DefaultViewPath = "Views/Default.cshtml"
            };

            var cms = new Cms(settings);
            container.Register<ICms, Cms>(cms);

        }
    }



Navigation sample
-----------------

Top navigation (from the sample files included)

    <ul class="topnavigation">
    @foreach(var c in documentNode.AncestorAtLevel(0).Children)
    {
        <li>
            @if (documentNode.IsDescendantOrSameAs(c))
            {
                <a href="@c.Url" class="selected">@c.Name</a>
            }
            else
            {
                <a href="@c.Url">@c.Name</a>
            }
        </li>
    }
    </ul>


Usage in WebPages
-----------------

The easiest ootb way is to add a "controller-like" razor file which serves the correct view this way:

    @{
      var node = nodeTreeCms.DocumentNodeByUrl(Request.Url.Path);
    }
    @RenderPartial(node.ViewPath, node)

And then the in view use the model (document node) like this:

    <h1>@Model.Name</h1>

    @Html.Raw(Model.Body)


... or simply include the code in one and the same razor file:

   
    @{
      var node = nodeTreeCms.DocumentNodeByUrl(Request.Url.Path);
    }
    
    <h1>@Model.Name</h1>

    @Html.Raw(Model.Body)



Contact me at @joeriks