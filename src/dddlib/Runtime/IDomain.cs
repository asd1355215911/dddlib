﻿// <copyright file="IDomain.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Exposes the public members of the domain.
    /// </summary>
    public interface IDomain
    {
        /// <summary>
        /// Sets the runtime mode for the domain model contained within this assembly.
        /// </summary>
        /// <param name="mode">The runtime mode.</param>
        void SetRuntimeMode(RuntimeMode mode);

        /// <summary>
        /// Registers the specified factory for creating an uninitialized instance of an aggregate of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of aggregate.</typeparam>
        /// <param name="aggregateFactory">The factory for the aggregate.</param>
        void RegisterUninitializedAggregateRootFactory<T>(Func<T> aggregateFactory) where T : AggregateRoot;

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not my call.")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GetType", Justification = "This is it.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not visible in editor.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not visible in editor.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not visible in editor.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "Not my call")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not visible in editor.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }
}
