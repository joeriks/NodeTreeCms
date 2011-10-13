using System;
using System.Collections.Generic;
namespace NodeTreeCms.Models
{
    public interface IDocumentNode
    {
        string Administrator { get; set; }
        string Author { get; set; }
        string Body { get; set; }
        List<IDocumentNode> Children { get; set; }
        DateTime CreatedDateTime { get; set; }
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        bool IsHidden { get; set; }
        int Level { get; set; }
        string Name { get; set; }
        IDocumentNode Parent { get; set; }
        Dictionary<string, object> Properties { get; set; }
        DateTime UpdatedDateTime { get; set; }
        string Url { get; set; }
        string UrlName { get; set; }
        string ViewPath { get; set; }
    }
}
