﻿// <copyright file="AggregateRootAnalyzer.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime
{
    using System;

    internal class AggregateRootAnalyzer
    {
        public AggregateRootConfiguration GetConfiguration(Type type)
        {
            return new AggregateRootConfiguration
            {
                Factory = null, // cannot specify factory on type
                ApplyMethodName = "Handle", // default
            };
        }
    }
}
