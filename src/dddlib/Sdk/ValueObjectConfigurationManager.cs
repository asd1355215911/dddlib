﻿// <copyright file="ValueObjectConfigurationManager.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime
{
    internal class ValueObjectConfigurationManager
    {
        public ValueObjectConfiguration Merge(ValueObjectConfiguration typeConfiguration, ValueObjectConfiguration baseTypeConfiguration)
        {
            // this merges the base type configuration
            // there is logic required in here
            return new ValueObjectConfiguration
            {
                Mapper = typeConfiguration.Mapper,
                ////NaturalKeySelector = typeConfiguration.NaturalKeySelector ?? baseTypeConfiguration.NaturalKeySelector,
                ////NaturalKeyEqualityComparer = typeConfiguration.NaturalKeyEqualityComparer ?? baseTypeConfiguration.NaturalKeyEqualityComparer,
            };
        }
    }
}
