namespace ContactManager.Presenters.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Caliburn.PresentationFramework.Screens;
    using Caliburn.Silverlight.ApplicationFramework;

    public interface IQuestionPresenter : IScreenEx
    {
        void Setup(IEnumerable<Question> questions, Action completed);
    }
}