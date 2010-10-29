using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Constants;
using System.Collections.ObjectModel;

namespace Infrastructure.DocumentModel
{
	public class Document
	{		
		public DocumentType Type { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Description { get; set; }

		public Document(DocumentType type, string title, string content, string desp)
		{			
			this.Type = type;
			this.Title = title;
			this.Content = content;
			this.Description = desp;
		}
	}

	public class DocumentList : ObservableCollection<Document>
	{

	}
}
