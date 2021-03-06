﻿// <copyright file="MemoryEventPersistence.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Persistence.Tests.Feature
{
    using System;
    using dddlib.Configuration;
    using dddlib.Persistence;
    using dddlib.Persistence.Memory;
    using dddlib.Persistence.Tests.Sdk;
    using dddlib.Runtime;
    using FluentAssertions;
    using Xbehave;

    // As someone who uses dddlib [with event sourcing]
    // In order save state
    // I need to be able to persist an aggregate root
    public abstract class MemoryEventPersistence : Feature
    {
        public class UndefinedNaturalKey : MemoryEventPersistence
        {
            [Scenario]
            public void Scenario(EventStoreRepository repository, Subject instance, Action action)
            {
                "Given a repository"
                    .f(() => repository = new EventStoreRepository(new MemoryIdentityMap(), new MemoryEventStore()));

                "And an instance of an aggregate root"
                    .f(() => instance = new Subject());

                "When that instance is saved to the repository"
                    .f(() => action = () => repository.Save(instance));

                "Then a runtime exception is thrown"
                    .f(() => action.ShouldThrow<RuntimeException>());
            }

            public class Subject : AggregateRoot
            {
            }

            private class BootStrapper : IBootstrap<Subject>
            {
                public void Bootstrap(IConfiguration configure)
                {
                    // should be aggregate root
                    configure.AggregateRoot<Subject>()
                        .ToReconstituteUsing(() => new Subject());
                }
            }
        }
    }
}
