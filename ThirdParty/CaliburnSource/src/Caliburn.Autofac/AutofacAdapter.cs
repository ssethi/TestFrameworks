﻿namespace Caliburn.Autofac
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.Behaviors;
    using Core.InversionOfControl;
    using global::Autofac;
    using global::Autofac.Builder;
    using global::Autofac.Core;
    using IContainer=global::Autofac.IContainer;

    /// <summary>
    /// An adapter allowing an <see cref="IContainer"/> to plug into Caliburn via <see cref="IServiceLocator"/> and <see cref="IRegistry"/>.
    /// </summary>
    public class AutofacAdapter : ContainerBase
    {
        private readonly IContainer container;
        private ContainerUpdater updater;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacAdapter"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public AutofacAdapter(IContainer container)
        {
            this.container = container;

            AddRegistrationHandler<Singleton>(HandleSingleton);
            AddRegistrationHandler<PerRequest>(HandlePerRequest);
            AddRegistrationHandler<Instance>(HandleInstance);

            Register(new[]
            {
                new Instance{ Service = typeof(IServiceLocator), Implementation = this},
                new Instance{ Service = typeof(IRegistry), Implementation = this },
                new Instance{ Service = typeof(Core.InversionOfControl.IContainer), Implementation = this },
                new Instance{ Service = typeof(IContainer), Implementation = this.container },
            });
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IContainer Container
        {
            get { return container; }
        }

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of resolving
        /// the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        public override object GetInstance(Type serviceType, string key)
        {
            try
            {
                return key != null
                    ? container.Resolve(key, serviceType ?? typeof(object))
                    : container.Resolve(serviceType);
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of
        /// resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        public override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);

            object instance;
            return container.TryResolve(type, out instance)
                ? ((IEnumerable)instance).Cast<object>()
                : Enumerable.Empty<object>();
        }

        public override void BuildUp(object instance)
        {
            container.InjectProperties(instance);
        }

        /// <summary>
        /// Configures the container using the provided component registrations.
        /// </summary>
        /// <param name="registrations">The component registrations.</param>
        public override void Register(IEnumerable<Core.InversionOfControl.IComponentRegistration> registrations)
        {
            updater = new ContainerUpdater();

            base.Register(registrations);

            updater.Update(container);
            updater = null;
        }

        /// <summary>
        /// Installs a proxy factory.
        /// </summary>
        /// <typeparam name="T">The type of the proxy factory.</typeparam>
        /// <returns>
        /// A container with an installed proxy factory.
        /// </returns>
        public override Core.InversionOfControl.IContainer WithProxyFactory<T>()
        {
            Register(new[] { new Singleton { Service = typeof(IProxyFactory), Implementation = typeof(T) } });

            var factory = this.GetInstance<IProxyFactory>();

            Container.ComponentRegistry.Registered += (s, e) =>{
                var implementation = e.ComponentRegistration.Activator.LimitType;

                if(!implementation.ShouldCreateProxy())
                    return;

                e.ComponentRegistration.Activating += (s2, e2) =>{
                    e2.Instance = factory.CreateProxy(
                        implementation,
                        implementation.GetAttributes<IBehavior>(true).ToArray(),
                        DetermineConstructorArgs(implementation)
                        );
                };
            };

            return this;
        }

        private void HandleSingleton(Singleton singleton)
        {
            if (!singleton.HasName())
                updater.Register(x => x.Register(RegistrationBuilder.ForType(singleton.Implementation).As(singleton.Service).SingleInstance().CreateRegistration()));
            else if (!singleton.HasService())
                updater.Register(x => x.Register(RegistrationBuilder.ForType(singleton.Implementation).As(typeof(object)).Named(singleton.Name, typeof(object)).SingleInstance().CreateRegistration()));
            else updater.Register(x => x.Register(RegistrationBuilder.ForType(singleton.Implementation).As(singleton.Service).Named(singleton.Name, singleton.Service).SingleInstance().CreateRegistration()));
        }

        private void HandlePerRequest(PerRequest perRequest)
        {
            if (!perRequest.HasName())
                updater.Register(x => x.Register(RegistrationBuilder.ForType(perRequest.Implementation).As(perRequest.Service).InstancePerDependency().CreateRegistration()));
            else if (!perRequest.HasService())
                updater.Register(x => x.Register(RegistrationBuilder.ForType(perRequest.Implementation).As(typeof(object)).Named(perRequest.Name, typeof(object)).InstancePerDependency().CreateRegistration()));
            else updater.Register(x => x.Register(RegistrationBuilder.ForType(perRequest.Implementation).As(perRequest.Service).Named(perRequest.Name, perRequest.Service).InstancePerDependency().CreateRegistration()));
        }

        private void HandleInstance(Instance instance)
        {
            if (!instance.HasName())
                updater.Register(x => x.Register(RegistrationBuilder.ForDelegate(instance.Service, (c, p) => instance.Implementation).CreateRegistration()));
            else if (!instance.HasService())
                updater.Register(x => x.Register(RegistrationBuilder.ForDelegate(typeof(object), (c, p) => instance.Implementation).Named(instance.Name, typeof(object)).CreateRegistration()));
            else updater.Register(x => x.Register(RegistrationBuilder.ForDelegate(instance.Service, (c, p) => instance.Implementation).Named(instance.Name, instance.Service).CreateRegistration()));
        }

        class ContainerUpdater
        {
            readonly ICollection<Action<IComponentRegistry>> _configurationActions = new List<Action<IComponentRegistry>>();

            public void Register(Action<IComponentRegistry> configurationAction)
            {
                _configurationActions.Add(configurationAction);
            }

            public void Update(IContainer container)
            {
                foreach (var action in _configurationActions)
                    action(container.ComponentRegistry);
            }
        }
    }
}