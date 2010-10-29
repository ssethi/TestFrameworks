using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Infrastructure.DocumentModel;

namespace Infrastructure.Events
{
	public class DocumentClosingEventArgs : CancelEventArgs
	{
		public DocumentClosingEventArgs(DocumentBase doc)
			: this(doc, false)
		{
		}

		public DocumentClosingEventArgs(DocumentBase doc, bool cancel)
			: base(cancel)
		{
			this.Document = doc;
		}

		public DocumentBase Document
		{
			get;
			private set;
		}
	}
}
