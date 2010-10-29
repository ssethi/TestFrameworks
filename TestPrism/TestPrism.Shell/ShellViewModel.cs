using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Infrastructure.DocumentModel;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Prism.Events;
using System.Windows;
using Infrastructure.Constants;

namespace TestPrism.Shell
{
	public class ShellViewModel : ViewModelBase
	{
		private IUnityContainer container;		
		public IEventAggregator EventAggregator { get; set; }
		public IDocumentController WorkspaceDocumentController { get; private set; }
		public IDocumentController DocumentController { get; private set; }
		public ShellViewModel(IUnityContainer container, IEventAggregator eventAggregator)
		{
			this.container = container;
			this.EventAggregator = eventAggregator;
			ShowAboutCommand = new DelegateCommand<object>(DoShowAboutExecuted);
			ExitApplicationCommand = new DelegateCommand<object>(DoExitApplicationExecuted);

			DocumentController = container.Resolve<IDocumentController>(Controllers.DocumentController);
			WorkspaceDocumentController = container.Resolve<IDocumentController>(Controllers.WorkspaceDocumentController);
						
			CloseCurrentDocumentCommand = new DelegateCommand<object>(CloseCurrentDocument, CanCloseCurrentDocument);
			CloseDocumentCommand = new DelegateCommand<DocumentBase>(DocumentController.CloseDocument);			
		}

		#region Commands
		public DelegateCommand<object> ShowAboutCommand	{ get;	private set; }
		public DelegateCommand<object> CloseCurrentDocumentCommand { get; private set;	}
		public DelegateCommand<DocumentBase> CloseDocumentCommand {	get; private set; }
		public DelegateCommand<object> ExitApplicationCommand {	get; private set; }
		private void DoShowAboutExecuted(object param)
		{
			MessageBox.Show("About --- TBD");
		}

		private void DoExitApplicationExecuted(object param)
		{
			App.Current.Shutdown();
		}
		private void CloseCurrentDocument(object arg)
		{
			DocumentController.CloseDocument(DocumentController.CurrentDocument);
		}

		private bool CanCloseCurrentDocument(object arg)
		{
			return DocumentController.CurrentDocument != null;
		}		
		# endregion

	}
}
