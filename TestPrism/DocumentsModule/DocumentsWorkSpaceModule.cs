using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Infrastructure.DocumentModel;
using Infrastructure.Constants;
using DocumentsModule.DocumentModel;

namespace DocumentsModule.Modules
{
	[Module(ModuleName = "DocumentsWorkSpaceModule")]
	public class DocumentsWorkSpaceModule : IModule
	{
		private readonly IUnityContainer container;
		private readonly IDocumentController workspaceDocumentController;

		public DocumentsWorkSpaceModule(IUnityContainer container)
		{
			this.container = container;
			workspaceDocumentController = container.Resolve<IDocumentController>(Controllers.WorkspaceDocumentController);				

		}
		#region IModule Members

		public void Initialize()
		{
			container.RegisterType<DocumentsWorkspaceModel>(new ContainerControlledLifetimeManager());
			DocumentsWorkspaceModel model = container.Resolve<DocumentsWorkspaceModel>();
			workspaceDocumentController.OpenDocument(model);
		}

		#endregion
	}
}
