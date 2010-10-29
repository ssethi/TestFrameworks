using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Infrastructure.DocumentModel;
using Infrastructure.Constants;
using StartPageModule.DocumentModel;

namespace StartPageModule.Modules
{
	[Module(ModuleName = "StartPageModule")]
	public class StartPageModule : IModule
	{
		private readonly IDocumentController documentController;
		private readonly IUnityContainer container;
		public StartPageModule(IUnityContainer container) 
		 {
            this.container = container;
            documentController = container.Resolve<IDocumentController>(Controllers.DocumentController);
        }

		#region IModule Members

		public void Initialize()
		{
			documentController.OpenDocument(new StartPageDocumentModel());
		}

		#endregion
	}
}
