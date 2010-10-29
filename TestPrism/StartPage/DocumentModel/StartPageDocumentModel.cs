using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.DocumentModel;
using System.Windows;
using Infrastructure.Constants;
using StartPageModule.Views;

namespace StartPageModule.DocumentModel
{
	public class StartPageDocumentModel : DocumentBase
	{
		public StartPageDocumentModel()
		{
			view = new StartPage();
			view.DataContext = this;
		}
		private StartPage view;
		public override FrameworkElement View
		{
			get { return view; }
		}
		public override string DocumentTitle
		{
			get { return "Start Page"; }
		}
		public override string Description
		{
			get { return "Test Prism 4.0!"; }
		}
		//public override DocumentType DocumentType
		//{
		//    get { return DocumentType.Info; }
		//}		
	}
}
