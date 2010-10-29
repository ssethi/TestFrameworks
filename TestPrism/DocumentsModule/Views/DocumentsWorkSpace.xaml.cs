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
using DocumentsModule.DocumentModel;
using System.Windows.Controls.Primitives;
using Infrastructure.DocumentModel;

namespace DocumentsModule.Views
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class DocumentsWorkspace : UserControl
	{
		public DocumentsWorkspaceModel Model { get; set; }
		public DocumentsWorkspace(DocumentsWorkspaceModel model)
		{
			InitializeComponent();
			this.Model = model;
		}

		private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Selector sel = sender as Selector;
			if (sel != null)
			{
				Document doc = sel.SelectedItem as Document;
				if (doc != null)
				{
					Model.OpenDocumentCommand.Execute(doc);
				}
			}
		}

		private void AppDomain_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(AppDomain.CurrentDomain.FriendlyName);
		}		
	}
}
