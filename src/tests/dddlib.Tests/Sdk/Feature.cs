﻿// <copyright file="Feature.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Tests.Sdk
{
    using System;
    using System.Linq;
    using System.Reflection;
    using dddlib.Configuration;
    using dddlib.Runtime;
    using FakeItEasy;
    using Xbehave;

    public abstract class Feature
    {
        protected interface IBootstrap<T>
        {
            void Bootstrap(IConfiguration configure);
        }

        [Background]
        public void Background()
        {
            "Given a new application"
                .Given(() =>
                {
                    var mapper = new Mapper();

                    new Application(
                        t => CreateAggregateRootType(this.Bootstrap, mapper, t),
                        t => CreateEntityType(this.Bootstrap, mapper, t),
                        t => CreateValueObjectType(this.Bootstrap, mapper, t),
                        mapper)
                        .Using();
                });
        }

        private static AggregateRootType CreateAggregateRootType(Func<Type, Action<IConfiguration>> getBootstrapper, Mapper mapper, Type type)
        {
            var bootstrapper = new Bootstrapper(getBootstrapper, mapper);
            var typeAnalyzer = new AggregateRootAnalyzer();
            var manager = new AggregateRootConfigurationManager();
            var configProvider = new AggregateRootConfigurationProvider(bootstrapper, typeAnalyzer, manager);
            var configuration = configProvider.GetConfiguration(type);
            return new AggregateRootTypeFactory().Create(configuration);
        }

        private static EntityType CreateEntityType(Func<Type, Action<IConfiguration>> getBootstrapper, Mapper mapper, Type type)
        {
            var bootstrapper = new Bootstrapper(getBootstrapper, mapper);
            var typeAnalyzer = new EntityAnalyzer();
            var manager = new EntityConfigurationManager();
            var configProvider = new EntityConfigurationProvider(bootstrapper, typeAnalyzer, manager);
            var configuration = configProvider.GetConfiguration(type);
            return new EntityTypeFactory().Create(configuration);
        }

        private static ValueObjectType CreateValueObjectType(Func<Type, Action<IConfiguration>> getBootstrapper, Mapper mapper, Type type)
        {
            var bootstrapper = new Bootstrapper(getBootstrapper, mapper);
            var typeAnalyzer = new ValueObjectAnalyzer();
            var manager = new ValueObjectConfigurationManager();
            var configProvider = new ValueObjectConfigurationProvider(bootstrapper, typeAnalyzer, manager);
            var configuration = configProvider.GetConfiguration(type);
            return new ValueObjectTypeFactory().Create(configuration);
        }

        // TODO (Cameron): This is all a bit of a hack.
        private Action<IConfiguration> Bootstrap(Type type)
        {
            var bootstrappers = this.GetType().GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                .Where(t => t.GetInterfaces()
                    .Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IBootstrap<>)))
                .ToArray();

            var bootstrapperType = bootstrappers.SingleOrDefault(t => t.GetInterfaces()[0].GetGenericArguments()[0] == type);
            if (bootstrapperType == null)
            {
                return c => { };
            }

            var bootstrapper = Activator.CreateInstance(bootstrapperType);
            var method = bootstrapperType.GetMethod("Bootstrap");

            Action<IConfiguration> action = (Action<IConfiguration>)Delegate.CreateDelegate(typeof(Action<IConfiguration>), bootstrapper, method);

            return action; //// (Action<IConfiguration>)bootstrapper.Bootstrap;
        }
    }
}
