﻿#if SILVERLIGHT

namespace Caliburn.ShellFramework.History
{
    using System;

    public interface IHistoryCoordinator
    {
        void Start(Action<HistoryConfiguration> configurator);
        void Refresh();
    }
}

#endif