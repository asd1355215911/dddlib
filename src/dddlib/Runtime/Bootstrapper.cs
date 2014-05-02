﻿// <copyright file="Bootstrapper.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime
{
    using System;
    using System.Globalization;
    using System.Linq;
    using dddlib.Configuration;

    internal class Bootstrapper : 
        IConfigurationProvider<AggregateRootConfiguration>, 
        IConfigurationProvider<EntityConfiguration>, 
        IConfigurationProvider<ValueObjectConfiguration>
    {
        private readonly Func<Type, Action<IConfiguration>> bootstrapperProvider;

        public Bootstrapper()
            : this(GetBootstrapper)
        {
        }

        public Bootstrapper(Func<Type, Action<IConfiguration>> getBootstrapper)
        {
            Guard.Against.Null(() => getBootstrapper);

            this.bootstrapperProvider = getBootstrapper;
        }

        AggregateRootConfiguration IConfigurationProvider<AggregateRootConfiguration>.GetConfiguration(Type type)
        {
            var bootstrap = this.bootstrapperProvider(type);

            // create a config to run through the bootstrapper
            var configuration = new BootstrapperConfiguration();

            // bootstrap
            bootstrap(configuration);

            return ((IConfigurationProvider<AggregateRootConfiguration>)configuration).GetConfiguration(type);
        }

        EntityConfiguration IConfigurationProvider<EntityConfiguration>.GetConfiguration(Type type)
        {
            var bootstrap = this.bootstrapperProvider(type);

            // create a config to run through the bootstrapper
            var configuration = new BootstrapperConfiguration();

            // bootstrap
            bootstrap(configuration);

            return ((IConfigurationProvider<EntityConfiguration>)configuration).GetConfiguration(type);
        }

        ValueObjectConfiguration IConfigurationProvider<ValueObjectConfiguration>.GetConfiguration(Type type)
        {
            var bootstrap = this.bootstrapperProvider(type);

            // create a config to run through the bootstrapper
            var configuration = new BootstrapperConfiguration();

            // bootstrap
            bootstrap(configuration);

            return ((IConfigurationProvider<ValueObjectConfiguration>)configuration).GetConfiguration(type);
        }

        // TODO (Cameron): Consider BootstrapperProvider class.
        private static Action<IConfiguration> GetBootstrapper(Type type)
        {
            var bootstrapperTypes = type.Assembly.GetTypes().Where(assemblyType => typeof(IBootstrapper).IsAssignableFrom(assemblyType));
            if (!bootstrapperTypes.Any())
            {
                return config => { };
            }

            if (bootstrapperTypes.Count() > 1)
            {
                throw new RuntimeException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The assembly '{0}' has more than one bootstrapper defined.",
                        type.Assembly.GetName()));
            }

            var bootstrapperType = bootstrapperTypes.First();
            if (bootstrapperType.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new RuntimeException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The bootstrapper of type '{0}' cannot be instantiated as it does not have a default constructor.",
                        bootstrapperType));
            }

            var bootstrapper = default(IBootstrapper);
            try
            {
                bootstrapper = (IBootstrapper)Activator.CreateInstance(bootstrapperType);
            }
            catch (Exception ex)
            {
                throw new RuntimeException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The bootstrapper of type '{0}' threw an exception during instantiation.\r\nSee inner exception for details.",
                        bootstrapperType),
                    ex);
            }

            return bootstrapper.Bootstrap;
        }
    }
}
