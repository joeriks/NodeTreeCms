using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using System.IO;
using System.Reflection;
using NodeTreeCms.Models;
namespace NodeTreeCms
{
    public class AdminModule : NancyModule
    {
        public AdminModule(ICms nodeTreeCms)
            : base(nodeTreeCms.Settings.AdminUrl)
        {

            Get["/"] = Get["/{root}"] = parameters =>
            {
                return nodeTreeCms.AdminPage(Request.Url.Path).ToString();
            };
            Post["/"] = Post["/{root}"] = parameters =>
            {
                return nodeTreeCms.AdminPage(Request.Url.Path, Request.Form).ToString();
            };

        }
    }
}