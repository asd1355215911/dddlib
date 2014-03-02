﻿// <copyright file="ConfigureEntity.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime.Configuration
{
    using System;

    internal class ConfigureEntity<T> : IConfigureEntity<T>
        where T : Entity
    {
        public ConfigureEntity(RuntimeConfiguration configuration)
        {
        }

        public IConfigureEntity<T> ToUseNaturalKey(Func<T, object> naturalKeySelector)
        {
            return this;
        }
    }
}
