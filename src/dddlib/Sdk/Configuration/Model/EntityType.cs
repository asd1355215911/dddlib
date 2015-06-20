﻿// <copyright file="EntityType.cs" company="dddlib contributors">
//  Copyright (c) dddlib contributors. All rights reserved.
// </copyright>

namespace dddlib.Sdk.Configuration.Model
{
    using System;
    using System.Globalization;
    using dddlib.Sdk.Configuration.Services.TypeAnalyzer;

    /// <summary>
    /// Represents an entity type.
    /// </summary>
    public class EntityType : Entity
    {
        private static readonly ITypeAnalyzerService DefaultTypeAnalyzerService = new DefaultTypeAnalyzerService();

        private readonly NaturalKey baseEntityNaturalKey;

        private NaturalKey entityNaturalKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityType"/> class.
        /// </summary>
        /// <param name="runtimeType">The runtime type.</param>
        /// <param name="typeAnalyzerService">The type analyzer service.</param>
        public EntityType(Type runtimeType, ITypeAnalyzerService typeAnalyzerService)
             : base(new NaturalKey(typeof(EntityType), "RuntimeType", typeof(Type), DefaultTypeAnalyzerService))
        {
            Guard.Against.Null(() => runtimeType);
            Guard.Against.Null(() => typeAnalyzerService);

            if (!typeAnalyzerService.IsValidEntity(runtimeType))
            {
                throw new BusinessException(
                    string.Format(CultureInfo.InvariantCulture, "The specified runtime type '{0}' is not an entity.", runtimeType));
            }

            this.RuntimeType = runtimeType;
            this.entityNaturalKey = typeAnalyzerService.GetNaturalKey(runtimeType);
            this.Mappings = new MapperCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityType"/> class.
        /// </summary>
        /// <param name="runtimeType">Type of the runtime.</param>
        /// <param name="entityAnalyzerService">The entity analyzer service.</param>
        /// <param name="baseEntity">The base entity.</param>
        public EntityType(Type runtimeType, ITypeAnalyzerService entityAnalyzerService, EntityType baseEntity)
            : this(runtimeType, entityAnalyzerService)
        {
            Guard.Against.Null(() => runtimeType);
            Guard.Against.Null(() => baseEntity);

            if (baseEntity.RuntimeType != runtimeType.BaseType)
            {
                throw new BusinessException(
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        "The specified base entity runtime type '{0}' does not match the runtime type base type '{1}'.", 
                        baseEntity.RuntimeType,
                        runtimeType.BaseType));
            }

            this.baseEntityNaturalKey = baseEntity.NaturalKey;
        }

        /// <summary>
        /// Gets the runtime type for this entity type.
        /// </summary>
        /// <value>The runtime type.</value>
        [dddlib.NaturalKey]
        public Type RuntimeType { get; private set; }

        /// <summary>
        /// Gets the natural key for this entity type.
        /// </summary>
        /// <value>The natural key.</value>
        public NaturalKey NaturalKey
        {
            get { return this.entityNaturalKey ?? this.baseEntityNaturalKey; }
        }

        /// <summary>
        /// Gets the mappings.
        /// </summary>
        /// <value>The mappings.</value>
        //// TODO (Cameron): This is not right. Law of Dementer and all that.
        public MapperCollection Mappings { get; private set; }

        /// <summary>
        /// Configures the natural key for this entity type.
        /// </summary>
        /// <param name="naturalKey">The natural key.</param>
        public void ConfigureNaturalKey(NaturalKey naturalKey)
        {
            Guard.Against.Null(() => naturalKey);

            if (this.entityNaturalKey == naturalKey)
            {
                return;
            }

            if (naturalKey.RuntimeType != this.RuntimeType)
            {
                throw new BusinessException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Invalid natural key specified. The natural key must have a declaring type of '{0}'.",
                        this.RuntimeType));
            }

            if (this.entityNaturalKey != null)
            {
                throw new BusinessException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Cannot configure the natural key for the entity '{0}' to be the property '{1}' as the natural key is already configured to be the property '{2}'.",
                        this.RuntimeType,
                        naturalKey.PropertyName,
                        this.NaturalKey.PropertyName));
            }

            this.entityNaturalKey = naturalKey;
        }
    }
}
