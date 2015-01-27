﻿// <copyright file="IEntityConfigurationProvider.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Sdk.Configuration
{
    using System;
    using dddlib.Sdk;

    internal interface IEntityConfigurationProvider
    {
        EntityConfiguration GetConfiguration(Type type);
    }
}
