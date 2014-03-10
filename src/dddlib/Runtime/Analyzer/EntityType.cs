﻿// <copyright file="EntityType.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime
{
    using System.Collections.Generic;

    internal class EntityType : RuntimeType
    {
        public IEqualityComparer<object> EqualityComparer { get; internal set; }
    }
}
