using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Threading;
using System.Diagnostics;
using Infrastructure.Constants;

namespace Infrastructure.DocumentModel
{
	public class ViewModelBase : DisposableBase, INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
	{
		private DocumentState state;
		public DocumentState State
		{
			get 
			{				
				return state;
			}
			set
			{				
				if (value != state)
				{
					OnPropertyChanging("State");
					state = value;
					OnPropertyChanged("State");
				}
			}
		}

		
		#region INotifyPropertyChanging/ed
		public event PropertyChangingEventHandler PropertyChanging;
		protected virtual void OnPropertyChanging(string property)
		{
			PropertyChangingEventHandler handler = this.PropertyChanging;
			if (handler == null)
				return;
			handler(this, new PropertyChangingEventArgs(property));
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string property)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler == null)
				return;
			handler(this, new PropertyChangedEventArgs(property));
		}
		#endregion		
	}
}
