using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.DocumentModel;
using DocumentsModule.Views;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Infrastructure.Events;

namespace DocumentsModule.DocumentModel
{
	public class DocumentViewModel : DocumentBase
	{
		private readonly IEventAggregator eventAggregator;
		public DocumentViewModel(Document doc, IEventAggregator eventAggregator)
		{
			this.Document = doc;
			this.view = new DocumentView();
			view.DataContext = this;
			this.eventAggregator = eventAggregator;
			if (this.eventAggregator != null)
			{
				this.eventAggregator.GetEvent<ChangeTextEvent>().Subscribe(this.ViewTextChanged);
			}
		}
		public Document Document { get; set; }
		private DocumentView view;
		public override FrameworkElement View
		{
			get { return view; }
		}
				
		public override string DocumentTitle
		{
			get { return this.Document.Title; }
		}
		public override string Description
		{
			get { return this.Document.Description; }
		}
		public void ViewTextChanged(string text)
		{
			this.Document.Title = text;
			this.Document.Content = string.Format("{0}{1}{2}", this.Document.Content, Environment.NewLine, text);

			//HACK-----Add it to the property
			OnPropertyChanged("Document");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				view.Dispose();
			}

			base.Dispose(disposing);
		}		
	}
}
