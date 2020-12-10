﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Extensions.DI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceDescriptorAttribute : Attribute
    {
        public ServiceDescriptorAttribute() : this(null) { }

        public ServiceDescriptorAttribute(Type serviceType) : this(serviceType, ServiceLifetime.Scoped) { }

        public ServiceDescriptorAttribute(Type serviceType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
        }

        public Type ServiceType { get; }

        public ServiceLifetime Lifetime { get; }
    }
}
