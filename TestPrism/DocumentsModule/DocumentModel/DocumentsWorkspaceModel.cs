using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.DocumentModel;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Commands;
using DocumentsModule.Views;
using Microsoft.Practices.Unity;
using Infrastructure.Constants;
using DocumentsModule.DocumentModel;

namespace DocumentsModule.DocumentModel
{
	public class DocumentsWorkspaceModel : DocumentBase
	{		
		public IEventAggregator EventAggregator { get; set; }
		public DocumentsWorkspaceModel(IUnityContainer container, IEventAggregator eventAggregator)
		{
			OpenDocumentCommand = new DelegateCommand<Document>(OpenDocument);
			DeleteSelectedDocumentCommand = new DelegateCommand<object>(DeleteSelectedDocument, CanDeleteSelectedDocument);				
			CreateNewDocumentCommand = new DelegateCommand<object>(CreateNewDocument);

			this.EventAggregator = eventAggregator;
			view = new DocumentsWorkspace(this);			
			documentController = container.Resolve<IDocumentController>(Controllers.DocumentController);

			Documents = new DocumentList();
			LoadAllDocuments();
		}

		private IDocumentController documentController;
		public DocumentList Documents { get; set; }
		

		private FrameworkElement view;
		public override FrameworkElement View
		{
			get { return view; }
		}
		
		private Document selectedDocument;
		public Document SelectedDocument
		{
			get { return selectedDocument; }
			set
			{
				if (!Object.Equals(SelectedDocument, value))
				{
					OnPropertyChanging("SelectedDocument");
					selectedDocument = value;
					OnPropertyChanged("SelectedDocument");
				}
			}
		}

		public DelegateCommand<Document> OpenDocumentCommand {	get; private set; }
		public DelegateCommand<object> DeleteSelectedDocumentCommand { get;	private set; }
		public DelegateCommand<object> CreateNewDocumentCommand	{ get;	private set; }
		private void OpenDocument(Document doc)
		{
			documentController.OpenDocument(new DocumentViewModel(doc, this.EventAggregator)); 
		}

		private bool CanDeleteSelectedDocument(object arg)
		{
			return selectedDocument != null;
		}

		private void DeleteSelectedDocument(object arg)
		{
			if (selectedDocument != null)
			{
				//TBD
			}
		}

		private void CreateNewDocument(object arg)
		{
			//TBD
		}


		private void LoadAllDocuments()
		{
			this.Documents = Properties.Settings.Default.AllDocuments;
			if (this.Documents == null)
			{
				this.Documents = new DocumentList();
				Document testScriptDoc = new Document(DocumentType.Script, "Test Script", "Script Text", "Tooltip Script");
				Document testJobtDoc = new Document(DocumentType.Job, "Test Job", "Job Text", "Tooltip Job");
				Document testHostDoc = new Document(DocumentType.Host, "Test Host", "Job Host", "Tooltip Host");

				this.Documents.Add(testScriptDoc);
				this.Documents.Add(testJobtDoc);
				this.Documents.Add(testHostDoc);
			}			
		}

		public override string DocumentTitle
		{
			get { return "Documents"; }
		}

		
		public override string Description
		{
			get { return "Document Workspace"; }
		}		
	}
}
