using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses;
using Prism.Ioc;

namespace Fateblade.PersonManagementApp.CoCo.Core.PrismAdapter
{
    internal class KernelAdapter : ICoCoKernel
    {
        private readonly IContainerRegistry _containerRegistry;
        private readonly IContainerProvider _containerProvider;

        public KernelAdapter(IContainerRegistry containerRegistry, IContainerProvider containerProvider)
        {
            _containerRegistry = containerRegistry;
            _containerProvider = containerProvider;
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
            return _containerProvider.Resolve<TContract>();
        }

        public TContract Get<TContract>(params ConstructorParameter[] parameters)
        {
            throw new NotImplementedException("Not available in prism configuration"); //overload kernel? constructor parameter type with type instead of name??
        }

        public object Get(Type contractType)
        {
            return _containerProvider.Resolve(contractType);
        }

        public object Get(Type contractType, params ConstructorParameter[] parameters)
        {
            throw new NotImplementedException("Not available in prism configuration"); //overload kernel? constructor parameter type with type instead of name??
        }

        public void RegisterConfiguration<T>()
        {
            _containerRegistry.Register<T>(); //how to handle 
        }
    }
}
