using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using Ninject.Web.Common;
using System;
using System.Linq;
using Prism.Ioc;

namespace Fateblade.PersonManagementApp.CoCo.Core.NinjectPrismAdapter
{
    public class KernelAdapter : DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.ICoCoKernel,
        IContainerExtension<IKernel>
    {
        private readonly IKernel _innerKernel;

        public IKernel Instance => _innerKernel;
        
        public KernelAdapter(IKernel innerKernel)
        {
            _innerKernel = innerKernel;
        }

        public void Register<TContract, TImplementation>(RegisterScope scope = RegisterScope.PerInject)
            where TImplementation : TContract
        {
            var registration = _innerKernel.Bind<TContract>().To<TImplementation>();
            ApplyScope(registration, scope);
        }

        public void Register(Type contract, Type implementation, RegisterScope scope = RegisterScope.PerInject)
        {
            var registration = _innerKernel.Bind(contract).To(implementation);
            ApplyScope(registration, scope);
        }

        public void RegisterUnique<TContract, TImplementation>(TImplementation implementation) where TImplementation : TContract
        {
            var registration = _innerKernel.Bind<TContract>().ToConstant(implementation);
            ApplyScope(registration, RegisterScope.Unique);
        }

        public void RegisterUnique(Type type, object implementation)
        {
            var registration = _innerKernel.Bind(type).ToConstant(implementation);
            ApplyScope(registration, RegisterScope.Unique);
        }

        public void RegisterToSelf<TImplementation>(RegisterScope scope = RegisterScope.PerInject)
        {
            var registration = _innerKernel.Bind<TImplementation>().ToSelf();
            ApplyScope(registration, scope);
        }

        public void RegisterComponent<TComponent>() where TComponent : IComponentActivator
        {
            _innerKernel.Bind<IComponentActivator>().To<TComponent>();
        }

        public TContract Get<TContract>() => _innerKernel.Get<TContract>();

        public TContract Get<TContract>(params ConstructorParameter[] parameters)
        {
            var ninjectParameters = parameters.Select(p => new ConstructorArgument(p.Name, p.Value));
            var implementation = _innerKernel.Get<TContract>(ninjectParameters.ToArray());
            return implementation;
        }

        public object Get(Type contractType)
        {
            var implementation = _innerKernel.Get(contractType);
            return implementation;
        }

        public object Get(Type contractType, params ConstructorParameter[] parameters)
        {
            var ninjectParameters = parameters.Select(p => new ConstructorArgument(p.Name, p.Value));
            var implementation = _innerKernel.Get(contractType, ninjectParameters.ToArray());
            return implementation;
        }

        public void RegisterConfiguration<T>()
        {
            _innerKernel.Bind<T>().ToMethod(c => c.Kernel.Get<IConfigObjectProvider>().Get<T>());
        }

        private void ApplyScope<T>(IBindingWhenInNamedWithOrOnSyntax<T> registration, RegisterScope scope)
        {
            switch (scope)
            {
                case RegisterScope.PerInject:
                    registration.InTransientScope();
                    break;
                case RegisterScope.PerContext:
                    registration.InRequestScope();
                    break;
                case RegisterScope.Unique:
                    registration.InSingletonScope();
                    break;
            }
        }

        public object Resolve(Type type)
        {
            return Instance.Get(type);
        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            var overrides = parameters.Select(p => new TypeMatchingConstructorArgument(p.Type, (c, t) => p.Instance)).ToArray();
            return Instance.Get(type, overrides);
        }

        public object Resolve(Type type, string name)
        {
            return Instance.Get(type, name);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            var overrides = parameters.Select(p => new TypeMatchingConstructorArgument(p.Type, (c, t) => p.Instance)).ToArray();
            return Instance.Get(type, name, overrides);
        }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.Bind(type).ToConstant(instance);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.Bind(type).ToConstant(instance).Named(name);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.Bind(from).To(to).InSingletonScope();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.Bind(from).To(to).InSingletonScope().Named(name);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.Bind(from).To(to).InTransientScope();
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.Bind(from).To(to).InTransientScope().Named(name);
            return this;
        }

        public bool IsRegistered(Type type)
        {
            return Instance.GetBindings(type).Any();
        }

        public bool IsRegistered(Type type, string name)
        {
            return Instance.GetBindings(type).Any(t=>t.Metadata.Name==name);
        }

        public void FinalizeExtension() { }
    }
}
