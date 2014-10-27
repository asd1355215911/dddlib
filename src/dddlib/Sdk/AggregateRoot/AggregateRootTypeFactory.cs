﻿// <copyright file="AggregateRootTypeFactory.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

    namespace dddlib.Runtime
{
    internal class AggregateRootTypeFactory : IAggregateRootTypeFactory
    {
        public AggregateRootType Create(AggregateRootConfiguration configuration)
        {
            Guard.Against.Null(() => configuration);

            var eventDispatcher = new DefaultEventDispatcher(configuration.RuntimeType);

            return new AggregateRootType(configuration.UninitializedFactory, eventDispatcher);
        }
    }
}
