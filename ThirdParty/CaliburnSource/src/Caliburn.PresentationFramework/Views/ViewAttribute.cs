namespace Caliburn.PresentationFramework.Views
{
    using System;
    using System.Linq;
    using System.Windows;
    using Core.InversionOfControl;
    using Core.Logging;

    /// <summary>
    /// An implementation of <see cref="IViewLocator"/> that provides a basic lookup strategy for an attributed model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct, AllowMultiple = true)]
    public class ViewAttribute : Attribute, IViewStrategy
    {
        private static readonly ILog Log = LogManager.GetLog(typeof(ViewAttribute));
        private readonly Type key;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAttribute"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public ViewAttribute(Type key)
        {
            this.key = key;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public Type Key
        {
            get { return key; }
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public object Context { get; set; }

        /// <summary>
        /// Determines whether this strategy applies in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// true if it matches the context; false otherwise
        /// </returns>
        public bool Matches(object context)
        {
            if(Context == null)
                return context == null;

            return Context.Equals(context);
        }

        /// <summary>
        /// Locates the View for the specified model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="displayLocation">The display location.</param>
        /// <param name="context">The context.</param>
        /// <returns>The view.</returns>
        public DependencyObject Locate(Type modelType, DependencyObject displayLocation, object context)
        {
            var instances = IoC.GetAllInstances(key);

            var view = instances.Count() > 0
                ? instances.First()
                : Activator.CreateInstance(key);

            Log.Info("Custom view {0} located for {1}.", view, modelType);
            return (DependencyObject)view;
        }
    }
}