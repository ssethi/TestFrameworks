namespace Caliburn.PresentationFramework.Views
{
    using System;

    /// <summary>
    /// Indicates that a model should be made aware of its view.
    /// </summary>
    public interface IViewAware
    {
        /// <summary>
        /// Attaches a view to this instance.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="context">The context.</param>
        void AttachView(object view, object context);

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The view</returns>
        object GetView(object context);

        /// <summary>
        /// Raised when a view is attached.
        /// </summary>
        event EventHandler<ViewAttachedEventArgs> ViewAttached;
    }

    /// <summary>
    /// The event args for the <see cref="IViewAware.ViewAttached"/> event.
    /// </summary>
    public class ViewAttachedEventArgs : EventArgs
    {
        /// <summary>
        /// The view.
        /// </summary>
        public object View;

        /// <summary>
        /// The context.
        /// </summary>
        public object Context;
    }
}