﻿namespace Caliburn.ShellFramework.Results
{
    using System;
    using PresentationFramework.RoutedMessaging;

    public abstract class OpenResultBase<TTarget> : IOpenResult<TTarget>
    {
        protected Action<TTarget> onConfigure;
        protected Action<TTarget> onClose;

        Action<TTarget> IOpenResult<TTarget>.OnConfigure
        {
            get { return onConfigure; }
            set { onConfigure = value; }
        }

        Action<TTarget> IOpenResult<TTarget>.OnClose
        {
            get { return onClose; }
            set { onClose = value; }
        }

        public abstract void Execute(ResultExecutionContext context);

        protected virtual void OnCompleted(Exception exception, bool wasCancelled)
        {
            Completed(this, new ResultCompletionEventArgs{ Error = exception, WasCancelled = wasCancelled});
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}