using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiveDomain.Core;
using NodeTreeCms.Models;
using System.IO;

namespace NodeTreeCms.DataProviders
{
    public class LiveDbProvider : IDataProvider
    {
        private Engine liveDbEngine;
        private bool nodeTreeIsUpdated;

        public LiveDbProvider(string path)
        {

            if (Directory.Exists(path) == false)
            {
                var model = new DocumentNodeModel();
                var settings = new EngineSettings(path);

                LiveDomain.Core.Engine.Create(model, settings);

            }
            liveDbEngine = LiveDomain.Core.Engine.Load<DocumentNodeModel>(path);
            nodeTreeIsUpdated = true;

        }
        public void UpdateDatabaseImage()
        {
            liveDbEngine.WriteBaseImage();
        }
        public void Close()
        {
            liveDbEngine.WriteBaseImage();
            liveDbEngine.Close();
        }
        public IDocumentNode DocumentNode_Insert(IDocumentNode documentNode, IDocumentNode parentNode)
        {
            var cmd = new AddOrUpdateDocumentNode() { DocumentNode = documentNode, ParentNode = parentNode };
            nodeTreeIsUpdated = true;
            return (IDocumentNode)liveDbEngine.Execute(cmd);

        }
        public IDocumentNode DocumentNode_Delete(IDocumentNode documentNode)
        {
            nodeTreeIsUpdated = true;
            throw new NotImplementedException();
        }
        public IDocumentNode DocumentNode_Update(IDocumentNode documentNode)
        {
            var cmd = new AddOrUpdateDocumentNode() { DocumentNode = documentNode };
            nodeTreeIsUpdated = true;
            return (IDocumentNode)liveDbEngine.Execute(cmd);

        }     
        public bool NodeTreeIsUpdated
        {
            get { return nodeTreeIsUpdated; }
        }
        public IDocumentNode DocumentNode_NodeTree()
        {
            nodeTreeIsUpdated = false; 
            return liveDbEngine.Execute<DocumentNodeModel, IDocumentNode>(n => n.RootNode);
            
        }
    }
}