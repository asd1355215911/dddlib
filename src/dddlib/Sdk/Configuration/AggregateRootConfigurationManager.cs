﻿// <copyright file="AggregateRootConfigurationManager.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Sdk.Configuration
{
    using dddlib.Sdk;

    internal class AggregateRootConfigurationManager : IConfigurationManager<AggregateRootConfiguration>
    {
        public AggregateRootConfiguration Merge(AggregateRootConfiguration typeConfiguration, AggregateRootConfiguration baseTypeConfiguration)
        {
            // this merges the base type configuration
            // there is logic required in here
            return new AggregateRootConfiguration
            {
                UninitializedFactory = typeConfiguration.UninitializedFactory,
                ////ApplyMethodName = typeConfiguration.ApplyMethodName,
                ////NaturalKeySelector = typeConfiguration.NaturalKeySelector ?? baseTypeConfiguration.NaturalKeySelector,
                ////NaturalKeyEqualityComparer = typeConfiguration.NaturalKeyEqualityComparer ?? baseTypeConfiguration.NaturalKeyEqualityComparer,
            };
        }
    }
}
