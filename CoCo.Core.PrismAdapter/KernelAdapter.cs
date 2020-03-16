using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;

namespace Fateblade.PersonManagementApp.CoCo.Core.PrismAdapter
{
    internal class KernelAdapter : ICoCoKernel
    {
        private readonly IContainerRegistry _containerRegistry;
        private readonly IContainerProvider _containerProvider;
        private readonly List<Type> _registeredConfigurations;

        public KernelAdapter(IContainerRegistry containerRegistry, IContainerProvider containerProvider)
        {
            _containerRegistry = containerRegistry;
            _containerProvider = containerProvider;
            _registeredConfigurations = new List<Type>();
        }

        public void Register<TContract, TImplementation>(RegisterScope scope = RegisterScope.PerInject) where TImplementation : TContract
        {
            if (scope == RegisterScope.PerContext
                || scope == RegisterScope.PerInject)
            {
                _containerRegistry.Register<TContract, TImplementation>();
            }
            else if(scope == RegisterScope.Unique)
            {
                _containerRegistry.RegisterSingleton<TContract, TImplementation>();
            }
        }

        public void Register(Type contract, Type implementation, RegisterScope scope = RegisterScope.PerInject)
        {
            if (scope == RegisterScope.PerContext
                || scope == RegisterScope.PerInject)
            {
                _containerRegistry.Register(contract, implementation);
            }
            else if (scope == RegisterScope.Unique)
            {
                _containerRegistry.RegisterSingleton(contract, implementation);
            }
        }

        public void RegisterToSelf<TImplementation>(RegisterScope scope = RegisterScope.PerInject)
        {
            if (scope == RegisterScope.PerContext
                || scope == RegisterScope.PerInject)
            {
                _containerRegistry.Register<TImplementation>();
            }
            else if (scope == RegisterScope.Unique)
            {
                _containerRegistry.RegisterSingleton<TImplementation>();
            }
        }

        public void RegisterComponent<TComponent>() where TComponent : IComponentActivator
        {
            _containerRegistry.Register<IComponentActivator, TComponent>();
        }

        public TContract Get<TContract>()
        {
            if (_registeredConfigurations.Contains(typeof(TContract)))
            {
                return _containerProvider.Resolve<IConfigObjectProvider>().Get< TContract>();
            }

            return _containerProvider.Resolve<TContract>();
        }

        public TContract Get<TContract>(params ConstructorParameter[] parameters)
        {
            throw new NotImplementedException("Not available in prism configuration"); //Todo: overload kernel? constructor parameter type with type instead of name??
        }

        public object Get(Type contractType)
        {
            if (_registeredConfigurations.Contains(contractType))
            {
                return _containerProvider.Resolve<IConfigObjectProvider>().Get(contractType);
            }

            return _containerProvider.Resolve(contractType);
        }

        public object Get(Type contractType, params ConstructorParameter[] parameters)
        {
            throw new NotImplementedException("Not available in prism configuration"); //Todo: overload kernel? constructor parameter type with type instead of name??
        }

        public void RegisterConfiguration<T>()
        {
            _registeredConfigurations.Add(typeof(T));
        }

        
    }
}
