﻿// <copyright file="Application.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Runtime
{
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

        private readonly Dictionary<Assembly, Func<Type, IEventDispatcher>> eventDispatcherFactories = new Dictionary<Assembly, Func<Type, IEventDispatcher>>();
        private readonly Dictionary<Type, IEventDispatcher> eventDispatchers = new Dictionary<Type, IEventDispatcher>();
        private readonly Lazy<Domain> domain = new Lazy<Domain>(() => new Domain(), true);

        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
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

        // NOTE (Cameron): I decided not to implement a check to see if the object is disposed.
        // LINK (Cameron): http://stackoverflow.com/questions/18069521/should-objectdisposedexception-be-thrown-from-a-property-get
        internal Domain Domain
        {
            get { return this.domain.Value; }
        }

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

        internal IEventDispatcher GetEventDispatcher(Type type)
        {
            if (type == typeof(Domain))
            {
                return new DefaultEventDispatcher(type);
            }

            return this.domain.Value[type].EventDispatcher;
        }

        internal IEqualityComparer<object> GetEqualityComparer(Type type)
        {
            if (type == typeof(Domain))
            {
                return EqualityComparer<object>.Default;
            }

            return this.domain.Value[type].EqualityComparer;
        }
    }
}
