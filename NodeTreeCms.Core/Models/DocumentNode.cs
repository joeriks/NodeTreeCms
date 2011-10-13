using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiveDomain.Core;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.IO;

namespace NodeTreeCms.Models
{
    [Serializable]
    public class DocumentNode : IDocumentNode
    {
        public Guid Id { get; set; }
        public bool IsHidden { get; set; }        
        public string UrlName { get; set; }
        public string Name { get; set; }        
        public string Body { get; set; }
        public string ExtraContent1 { get; set; }
        public string ExtraContent2 { get; set; }
        public string ExtraContent3 { get; set; }        
        public bool HideHeader { get; set; }

        public string Author { get; set; }
        public string Administrator { get; set; }
        
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public Dictionary<string, object> Properties { get; set; }
        public List<IDocumentNode> Children { get; set; }
        public string Url { get; set; }
        public IDocumentNode Parent { get; set; }
        public int Level { get; set; }
        public bool IsDeleted { get; set; }
        public string ViewPath { get; set; }


        public DocumentNode(string name)
        {
            Children = new List<IDocumentNode>();
            Id = Guid.NewGuid();
            Name = name;
            Url = "";

            CreatedDateTime = DateTime.Now;
            UpdatedDateTime = DateTime.Now;
        }

    }

}