using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Infrastructure.Constants;

namespace Infrastructure.DocumentModel
{
	public abstract class DocumentBase : ViewModelBase
	{
		//public abstract DocumentType DocumentType { get; }
		public abstract string DocumentTitle {	get; }
		public abstract FrameworkElement View {	get; }
		public abstract string Description { get; }		
	}
}
