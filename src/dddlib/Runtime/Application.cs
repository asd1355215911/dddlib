﻿// <copyright file="Application.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime
{
    /*  TODO (Cameron): 
        Consider folding Runtime into Application.
        Apply 'is disposed' check when runtime is moved in.  */

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents an application.
    /// </summary>
    public sealed class Application : IDisposable
    {
        private static readonly Lazy<Application> DefaultApplication = new Lazy<Application>(() => new Application(), true);
        private static readonly List<Application> Applications = new List<Application>();
        private static readonly object SyncLock = new object();

        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
            : this(type => new DefaultTypeConfigurationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="configurationProviderFactory">The configuration provider factory.</param>
        public Application(Func<Type, ITypeConfigurationProvider> configurationProviderFactory)
        {
            this.Runtime = new Runtime(type => configurationProviderFactory(type));

            lock (SyncLock)
            {
                Applications.Add(this);
            }
        }

        /// <summary>
        /// Gets the ambient application instance.
        /// </summary>
        /// <value>The ambient application instance.</value>
        public static Application Current
        {
            get 
            {
                lock (SyncLock)
                {
                    // LINK (Cameron): http://stackoverflow.com/questions/1043039/does-listt-guarantee-insertion-order
                    return Applications.Any() ? Applications.Last() : DefaultApplication.Value;
                }
            }
        }

        internal Runtime Runtime { get; private set; }

        void IDisposable.Dispose()
        {
            if (object.ReferenceEquals(this, DefaultApplication.Value))
            {
                // NOTE (Cameron): We cannot allow the ambient application to be disposed.
                return;
            }

            lock (SyncLock)
            {
                if (this.isDisposed)
                {
                    return;
                }

                Applications.Remove(this);

                this.isDisposed = true;
            }
        }
    }
}
