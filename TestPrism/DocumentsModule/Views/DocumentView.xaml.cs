using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DocumentsModule.Views
{
	/// <summary>
	/// Interaction logic for DocumentView.xaml
	/// </summary>
	public partial class DocumentView : UserControl, IDisposable
	{
		public DocumentView()
		{
			InitializeComponent();
		}

		List<DocumentView> testViews = new List<DocumentView>();
		private void label1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show("Testing Memory");
			for (int i = 0; i < 10000; i++)
			{
				DocumentView view = new DocumentView();
				testViews.Add(view);
			}
		}

		private void CheckAppDomain_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(AppDomain.CurrentDomain.FriendlyName);			
		}

		
		#region IDisposable Members

		bool disposed = false;
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
			GC.Collect();
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					testViews.Clear();					
				}				
			}
			disposed = true;			
		}
		~DocumentView()
		{
			Dispose(false);
		}
		#endregion
	}
}
