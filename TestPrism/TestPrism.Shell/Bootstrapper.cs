using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Prism.Events;
using Infrastructure.DocumentModel;
using Infrastructure.Constants;
using TestPrism.Shell.DocumentModel;


//using Microsoft.Practices.Prism.MefExtensions
//IUnityContainer

namespace TestPrism.Shell
{
	class Bootstrapper : UnityBootstrapper
	{		
		protected override void ConfigureContainer()
		{
			RegisterServices();
			base.ConfigureContainer();
		}
		private void RegisterServices()
		{			
			Container.RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());		
			Container.RegisterInstance<IDocumentController>(Controllers.DocumentController,	new DocumentController());
			Container.RegisterInstance<IDocumentController>(Controllers.WorkspaceDocumentController, new DocumentController());
		}
		protected override DependencyObject CreateShell()
		{			
			ShellViewModel model = this.Container.Resolve<ShellViewModel>();
			Shell shellObj = new Shell(model);
			shellObj.DataContext = model;
			return shellObj;
		}
		protected override void InitializeShell()
		{
			base.InitializeShell();
			
			App.Current.MainWindow = (Window)this.Shell;
			App.Current.MainWindow.Show();

		}

		protected override IModuleCatalog CreateModuleCatalog()
		{
			//Use app.config to configure the modules
			return new ConfigurationModuleCatalog();			
		}
		
		//protected override void ConfigureModuleCatalog()
		//{
		//    base.ConfigureModuleCatalog();

		//    ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
		//    moduleCatalog.AddModule(typeof(HelloWorldModule.HelloWorldModule));
		//    moduleCatalog.AddModule(typeof(ViewOneModule.ViewOneModule));
		//}
	}
}
