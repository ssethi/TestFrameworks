﻿namespace ContactManager.Presenters
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Caliburn.Core.IoC;
    using Caliburn.PresentationFramework.ApplicationModel;
    using Caliburn.PresentationFramework.Screens;
    using Caliburn.Silverlight.ApplicationFramework;
    using Interfaces;
    using Microsoft.Practices.ServiceLocation;
    using Services.Interfaces;

    [Singleton(typeof(IShellPresenter))]
    public class ShellPresenter : ScreenConductor<IScreen>, IShellPresenter
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly ISettings _settings;
        private readonly HistoryManager<IHomePresenter> _historyManager;
        private IScreen _dialogModel;

        public ShellPresenter(IServiceLocator serviceLocator, IStateManager stateManager, ISettings settings)
        {
            _serviceLocator = serviceLocator;
            _settings = settings;
            _historyManager = new HistoryManager<IHomePresenter>(this, stateManager, serviceLocator);
        }

        public IScreen DialogModel
        {
            get { return _dialogModel; }
            set
            {
                _dialogModel = value; 
                NotifyOfPropertyChange(() => DialogModel);
            }
        }

        public void Open<T>() where T : IScreen
        {
            var presenter = _serviceLocator.GetInstance<T>();
            this.OpenScreen(presenter);
        }

        public void GoHome()
        {
            Open<IHomePresenter>();
        }

        public void ShowDialog<T>(T presenter)
            where T : IScreenEx
        {
            presenter.WasShutdown +=
                delegate { DialogModel = null; };

            DialogModel = presenter;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _settings.Load();
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            _historyManager.Initialize(Assembly.GetExecutingAssembly());
        }

        protected override void ExecuteShutdownModel(ISubordinate model, Action completed)
        {
            var dialogPresenter = _serviceLocator.GetInstance<IQuestionPresenter>();

            var composite = model as ISubordinateComposite;
            if(composite != null)
            {
                dialogPresenter.Setup(composite.GetChildren().Cast<Question>(), completed);
                ShowDialog(dialogPresenter);
            }
            else
            {
                var question = (Question)model;
                dialogPresenter.Setup(new[] {question}, completed);
                ShowDialog(dialogPresenter);
            }
        }
    }
}