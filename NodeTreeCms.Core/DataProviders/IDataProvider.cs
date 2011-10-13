using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodeTreeCms.Models;

namespace NodeTreeCms.DataProviders
{
    public interface IDataProvider
    {
        void Close();
        void UpdateDatabaseImage();
        bool NodeTreeIsUpdated { get; }
        IDocumentNode DocumentNode_NodeTree();
        IDocumentNode DocumentNode_Insert(IDocumentNode documentNode, IDocumentNode parentNode);
        IDocumentNode DocumentNode_Delete(IDocumentNode documentNode);
        IDocumentNode DocumentNode_Update(IDocumentNode documentNode);
        
    }
}
