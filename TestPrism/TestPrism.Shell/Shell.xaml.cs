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
using Infrastructure;
using Microsoft.Practices.Prism.Events;
using Infrastructure.DocumentModel;
using Infrastructure.Events;

namespace TestPrism.Shell
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class Shell : Window
	{
		public ShellViewModel Model { get; set; }
		public Shell(ShellViewModel model)
		{
			InitializeComponent();
			Model = model;
		}
		private void DoCloseDocumentButtonClicked(object sender, RoutedEventArgs e)
		{
			ICommandSource src = sender as ICommandSource;
			if (src != null)
			{
				DocumentBase doc = src.CommandParameter as DocumentBase;
				if (doc != null)
				{
					Model.CloseDocumentCommand.Execute(doc);
				}
			}
		}

		private void CahangeTextMenuItem_Click(object sender, RoutedEventArgs e)
		{
			this.Model.EventAggregator.GetEvent<ChangeTextEvent>().Publish("Change Event");
		}

		private void CheckAppDomain_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(AppDomain.CurrentDomain.FriendlyName);
		}		
	}
}
