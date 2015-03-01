﻿// <copyright file="AggregateRootTypeFactory.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Sdk.Configuration
{
    using System;
    using System.Linq;
    using dddlib.Sdk.Configuration.Model;
    using dddlib.Sdk.Configuration.Model.BootstrapperService;

    internal class AggregateRootTypeFactory
    {
        private readonly ITypeAnalyzerService typeAnalyzerService;
        private readonly IBootstrapperProvider bootstrapperProvider;

        public AggregateRootTypeFactory(ITypeAnalyzerService typeAnalyzerService, IBootstrapperProvider bootstrapperProvider)
        {
            Guard.Against.Null(() => typeAnalyzerService);
            Guard.Against.Null(() => bootstrapperProvider);

            this.typeAnalyzerService = typeAnalyzerService;
            this.bootstrapperProvider = bootstrapperProvider;
        }

        public AggregateRootType Create(Type type)
        {
            var entityType = default(EntityType);

            foreach (var subType in type.GetTypeHierarchyUntil(typeof(object)).Reverse())
            {
                if (entityType == null)
                {
                    entityType = new EntityType(subType, this.typeAnalyzerService);
                    continue;
                }

                if (this.typeAnalyzerService.IsValidAggregateRoot(subType))
                {
                    break;
                }

                entityType = new EntityType(subType, this.typeAnalyzerService, entityType);
            }

            var aggregateRootType = default(AggregateRootType);

            foreach (var subType in type.GetTypeHierarchyUntil(typeof(Entity)).Reverse())
            {
                aggregateRootType = new AggregateRootType(subType, this.typeAnalyzerService, aggregateRootType ?? entityType);
            }

            var configuration = new BootstrapperConfiguration(aggregateRootType, this.typeAnalyzerService);
            var bootstrapper = this.bootstrapperProvider.GetBootstrapper(type);

            bootstrapper.Invoke(configuration);

            return aggregateRootType;
        }
    }
}