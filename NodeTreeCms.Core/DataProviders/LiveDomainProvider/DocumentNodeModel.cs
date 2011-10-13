using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiveDomain.Core;

namespace NodeTreeCms.Models
{
    [Serializable]
    public class DocumentNodeModel : Model
    {
        public IDocumentNode RootNode { get; set; }
        public DocumentNodeModel()
        {            
        }
    }
}