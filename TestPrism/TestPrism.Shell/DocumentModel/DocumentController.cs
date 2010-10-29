using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using System.ComponentModel;
using Infrastructure.DocumentModel;
using System.Collections.ObjectModel;
using Infrastructure.Events;

namespace TestPrism.Shell.DocumentModel
{
	class DocumentController : IDocumentController, INotifyPropertyChanged, INotifyPropertyChanging
	{
		private DocumentBase currentDocument;		
		public DocumentController()
		{
			Documents = new ObservableCollection<DocumentBase>();
		}

		#region IDocumentController Members
		
		public event EventHandler<DocumentClosingEventArgs> DocumentClosing;
		protected void OnDocumentClosing(object sender, DocumentClosingEventArgs args)
		{
			if (DocumentClosing != null)
			{
				DocumentClosing(sender, args);
			}
		}

		public event EventHandler<DataEventArgs<DocumentBase>> DocumentClosed;
		protected void OnDocumentClosed(object sender, DataEventArgs<DocumentBase> args)
		{
			if (DocumentClosed != null)
			{
				DocumentClosed(sender, args);
			}
		}

		public event EventHandler<DataEventArgs<DocumentBase>> DocumentOpened;
		protected void OnDocumentOpened(object sender, DataEventArgs<DocumentBase> args)
		{
			if (DocumentOpened != null)
			{
				DocumentOpened(sender, args);
			}
		}

		
		public ObservableCollection<DocumentBase> Documents
		{
			get;
			private set;
		}

		
		public DocumentBase CurrentDocument
		{
			get { return currentDocument; }
			set
			{
				if (Object.Equals(currentDocument, value))
				{
					return;
				}
				currentDocument = value;
				OnPropertyChanged("CurrentDocument");
			}
		}

		
		public void OpenDocument(DocumentBase document)
		{			
			if(document == null)
				return;

			DocumentBase doc = GetOpenDocument(document); //HACK
			if (doc != null)
				CurrentDocument = doc;
			else
			{
				Documents.Add(document);
				CurrentDocument = document;
			}
			
			OnDocumentOpened(this, new DataEventArgs<DocumentBase>(document));
		}

		DocumentBase GetOpenDocument(DocumentBase document)
		{
			if (document == null || this.Documents == null || this.Documents.Count <= 0)
				return null;

			foreach (DocumentBase doc in this.Documents)
			{
				if (doc.DocumentTitle.Equals(document.DocumentTitle))
					return doc;
			}

			return null;
		}
		
		public void CloseDocument(DocumentBase document)
		{
			if (Documents.Contains(document))
			{
				DocumentClosingEventArgs closeArgs = new DocumentClosingEventArgs(document);

				OnDocumentClosing(this, closeArgs);
				if (closeArgs.Cancel)
				{					
					return;
				}

				if (document.Equals(CurrentDocument))
				{
					CurrentDocument = null;
				}
											
				Documents.Remove(document);
				document.Dispose();
				OnDocumentClosed(this, new DataEventArgs<DocumentBase>(document));
			}
		}

		#endregion

		#region INotifyPropertyChanging/ed
		public event PropertyChangingEventHandler PropertyChanging;
		protected virtual void OnPropertyChanging(string property)
		{
			PropertyChangingEventHandler handler = this.PropertyChanging;
			if (handler == null)
				return;
			handler(this, new PropertyChangingEventArgs(property));
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string property)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler == null)
				return;
			handler(this, new PropertyChangedEventArgs(property));
		}
		#endregion
	}
}
