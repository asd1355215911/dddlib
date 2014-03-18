﻿// <copyright file="Bootstrapper.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime
{
    using System;

    internal class Bootstrapper
    {
        public AggregateRootConfiguration GetAggregateRootConfiguration(Type type)
        {
            var x = new DefaultTypeConfigurationProvider();
            var y = x.GetConfiguration(type);
            return new AggregateRootConfiguration
            {
                Factory = y.AggregateRootFactory,
                ApplyMethodName = null,
            };
        }

        public EntityConfiguration GetEntityConfiguration(Type type)
        {
            return new EntityConfiguration
            {
            };
        }
    }
}
