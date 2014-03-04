﻿// <copyright file="IConfiguration.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Exposes the public members of the configuration.
    /// </summary>
    //// NOTE (Cameron): Set to never be visible in the editor. Not sue if this is a sensible design choice... but it is a design choice.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IConfiguration : IFluentExtensions
    {
        /// <summary>
        /// Gets the aggregate roots configuration options.
        /// </summary>
        /// <value>The aggregate roots configuration options.</value>
        IConfigureAggregateRoots AggregateRoots { get; }

        /// <summary>
        /// Gets the aggregate root configuration options for the specified type of aggregate root.
        /// </summary>
        /// <typeparam name="T">The type of aggregate root.</typeparam>
        /// <returns>The aggregate root configuration options.</returns>
        IConfigureAggregateRoot<T> AggregateRoot<T>() where T : AggregateRoot;

        /// <summary>
        /// Gets the entity configuration options for the specified type of entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The entity configuration options.</returns>
        IConfigureEntity<T> Entity<T>() where T : Entity;
    }
}
