using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog;

namespace Next.PCL.Infra
{
    public interface INaiveContainer
    {
        TService GetService<TService>();

        void AddSingleton<TService>() where TService : class;
        void AddSingleton<TService>(Func<TService> func) where TService : class;

        void AddSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface;
        void AddSingleton<TInterface, TImplementation>(Func<TImplementation> func) 
            where TImplementation : class, TInterface;
    }
    public class NaiveDIContainer : INaiveContainer
    {
        private readonly ConcurrentDictionary<Type, NaiveDiItem> _container;

        public NaiveDIContainer()
        {
            _container = new ConcurrentDictionary<Type, NaiveDiItem>();
        }

        public TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }
        internal object GetService(Type type)
        {
            if (!_container.TryGetValue(type, out NaiveDiItem item))
                throw new Exception("Service not registered to naive DI");

            switch (item.Lifetime)
            {
                case NaiveDiLifetime.Singleton: return item.Instance;
                case NaiveDiLifetime.Transient: return Activator.CreateInstance(type);
            }

            return default;
        }
        internal bool ContainerHas(Type type)
        {
            return _container.ContainsKey(type);
        }

        public void AddSingleton<TService>() where TService : class
        {
            var type = typeof(TService);
            if (!ContainerHas(type))
            {
                object instance = CreateInstanceOf<TService>();
                if (!_container.TryAdd(type, new NaiveDiItem(NaiveDiLifetime.Singleton, instance)))
                {
                    throw new Exception("Failed to add to naive DI");
                }
            }
            else
            {
                // silently ignore existing service.
            }
        }
        public void AddSingleton<TService>(Func<TService> func) 
            where TService : class
        {
            AddSingleton<TService, TService>(func);
        }
        public void AddSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            AddSingleton<TInterface, TImplementation>(() => CreateInstanceOf<TImplementation>());
        }
        public void AddSingleton<TInterface, TImplementation>(Func<TImplementation> func) 
            where TImplementation : class, TInterface
        {
            var type = typeof(TInterface);
            if (!ContainerHas(type))
            {
                object instance = func?.Invoke();
                if (!_container.TryAdd(type, new NaiveDiItem(NaiveDiLifetime.Singleton, instance)))
                {
                    throw new Exception("Failed to add to naive DI");
                }
            }
            else
            {
                // silently ignore existing service.
            }
        }

        public TService CreateInstanceOf<TService>() where TService : class
        {
            var type = typeof(TService);
            if (type.IsAbstract || type.IsSealed)
                throw new Exception("Can't create instance of an abstract/sealed class.");

            List<object> args = new List<object>();
            var constructors = type.GetConstructors()
                                  .Where(x => x.IsPublic)
                                  .ToArray();

            foreach (var constructor in constructors)
            {
                try
                {
                    object[] items = GetConstructorDependencies(constructor);
                    if (items.Length > args.Count)
                    {
                        args.Clear();
                        args.AddRange(items);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }

            return (TService)Activator.CreateInstance(typeof(TService), args.ToArray());
        }
        private object[] GetConstructorDependencies(ConstructorInfo constructor)
        {
            List<object> args = new List<object>();
            var parameters = constructor.GetParameters();

            foreach (var parameter in parameters)
            {
                if (ContainerHas(parameter.ParameterType))
                {
                    object val = GetService(parameter.ParameterType);
                    args.Add(val);
                }
                else if (parameter.HasDefaultValue)
                {
                    args.Add(parameter.DefaultValue);
                }
                else
                {
                    throw new Exception($"Invalid ctor. Don't know how to set parameter '{parameter.Name}'");
                }
            }
            return args.ToArray();
        }
    }
    public class NaiveDiItem
    {
        public NaiveDiItem()
        { }
        public NaiveDiItem(NaiveDiLifetime lifetime, object instance = default)
        {
            Lifetime = lifetime;
            Instance = instance;
        }
        public NaiveDiLifetime Lifetime { get; set; }
        public object Instance { get; set; }
    }
    public enum NaiveDiLifetime
    {
        Singleton   = 0,
        Transient   = 1
    }
}