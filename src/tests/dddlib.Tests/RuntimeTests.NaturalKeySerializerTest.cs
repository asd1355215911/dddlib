﻿// <copyright file="RuntimeTests.NaturalKeySerializerTest.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Tests.Acceptance
{
    using System.Diagnostics.CodeAnalysis;
    using dddlib.Sdk.Configuration.Model;
    using FluentAssertions;

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Not here.")]
    public partial class RuntimeTests
    {
        private class NaturalKeySerializerTest : AggregateRoot
        {
            [dddlib.NaturalKey]
            public string NaturalKey { get; set; }
        }

        private static void AssertNaturalKeySerializerTest(AggregateRootType aggregateRootType)
        {
            // should provide equality comparison
            aggregateRootType.PersistEvents.Should().BeFalse();
        }
    }
}
