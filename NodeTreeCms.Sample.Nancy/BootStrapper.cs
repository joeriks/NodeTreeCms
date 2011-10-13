using System.Collections.Generic;
using Nancy;
using NodeTreeCms;
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
