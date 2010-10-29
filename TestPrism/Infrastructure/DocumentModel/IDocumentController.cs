using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using System.Collections.ObjectModel;
using Infrastructure.Events;

namespace Infrastructure.DocumentModel
{
	public interface IDocumentController
	{
		event EventHandler<DocumentClosingEventArgs> DocumentClosing;		
		event EventHandler<DataEventArgs<DocumentBase>> DocumentClosed;		
		event EventHandler<DataEventArgs<DocumentBase>> DocumentOpened;		
		ObservableCollection<DocumentBase> Documents { get;	}		
		DocumentBase CurrentDocument { get;	set; }		
		void OpenDocument(DocumentBase document);		
		void CloseDocument(DocumentBase document);
	}
}
