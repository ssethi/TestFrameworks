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

namespace StartPageModule.Views
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>	
	public partial class StartPage : UserControl
	{
		public StartPage()
		{
			InitializeComponent();
		}

		private void startpagebrowser_Loaded(object sender, RoutedEventArgs e)
		{
			startpagebrowser.Navigate("http://www.mandiant.com/news_events/");
		}

		private void CheckAppDomain_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(AppDomain.CurrentDomain.FriendlyName);
		}
	}
}
