﻿// <copyright file="RuntimeTests.NaturalKeyTest.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Tests.Acceptance
{
    using System.Diagnostics.CodeAnalysis;
    using dddlib.Runtime;
    using FluentAssertions;

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Not here.")]
    public partial class RuntimeTests
    {
        private class NaturalKeyTest : AggregateRoot
        {
            [NaturalKey]
            public string NaturalKey { get; set; }
        }

        private static void AssertNaturalKeyTest(AggregateRootType aggregateRootType)
        {
            // should provide equality comparison
            aggregateRootType.Options.PersistEvents.Should().BeFalse();
        }

        private static void AssertNaturalKeyTest(EntityType entityType)
        {
            // should provide equality comparison
            ////entityType.Options.PersistEvents.Should().BeFalse();
        }
    }
}
