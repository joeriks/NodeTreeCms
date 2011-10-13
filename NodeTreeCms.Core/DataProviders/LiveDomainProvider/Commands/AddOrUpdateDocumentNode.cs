using System;
using System.Collections.Generic;
using System.Linq;
using LiveDomain.Core;
using NodeTreeCms.Models;
using NodeTreeCms;

[Serializable]
public class AddOrUpdateDocumentNode : CommandWithResult<DocumentNodeModel, IDocumentNode>
{
    public IDocumentNode DocumentNode { get; set; }
    public IDocumentNode ParentNode { get; set; }

    protected bool updateDocumentNodeRecursive(DocumentNodeModel model, IDocumentNode documentNode)
    {        
        var retval = false;

        Action<IDocumentNode> traverse = null;
        traverse = (n) =>
        {
            var i = n.Children.FindIndex(u => u.Id == documentNode.Id);

            if (i>-1)
            {
                n.Children[i] = documentNode;
                retval = true;
            }
            else
            {
                n.Children.ForEach(traverse);
            }
        };

        traverse(model.RootNode);
        return retval;
    }

    protected override IDocumentNode Execute(DocumentNodeModel model)
    {

        if (model.RootNode == null)
        {
            DocumentNode.UpdateContextData();
            model.RootNode = DocumentNode;
        }
        else
        {
            var existingDocumentNode = model.RootNode.DocumentNodeByGuid(DocumentNode.Id);

            if (existingDocumentNode == null)
            {
                if (ParentNode == null)
                {
                    ParentNode = model.RootNode;
                }
                else
                {
                    ParentNode =  model.RootNode.DocumentNodeByGuid(ParentNode.Id);
                    DocumentNode.UpdateContextData(ParentNode);
                    ParentNode.Children.Add(DocumentNode);
                }
            }
            else
            {
                DocumentNode.UpdateContextData(existingDocumentNode.Parent);
                if (model.RootNode.Id == DocumentNode.Id)
                {
                    model.RootNode = DocumentNode;
                }
                else
                    updateDocumentNodeRecursive(model, DocumentNode);
            }
        }
        return DocumentNode;

    }
}
