using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses;
using DryIoc;
using Prism.Ioc;

namespace Fateblade.PersonManagementApp.CoCo.Core.DryIocPrismAdapter
{
    public class KernelAdapter :
        DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.ICoCoKernel,
        IContainerExtension<IContainer>
    {
        private readonly IContainerExtension<IContainer> _innerContainer;

        public IContainer Instance => _innerContainer.Instance;

        public KernelAdapter(IContainerExtension<IContainer> existingContainerToWrap)
        {
            _innerContainer = existingContainerToWrap;
        }

        public void Register<TContract, TImplementation>(RegisterScope scope = RegisterScope.PerInject)
            where TImplementation : TContract
        {
            _innerContainer.Register<TContract, TImplementation>();
        }

        public void Register(Type contract, Type implementation, RegisterScope scope = RegisterScope.PerInject)
        {
            _innerContainer.Register(contract, implementation);
        }

        public void RegisterUnique<TContract, TImplementation>(TImplementation implementation)
            where TImplementation : TContract
        {
            _innerContainer.RegisterSingleton<TContract, TImplementation>();
        }

        public void RegisterUnique(Type type, object implementation)
        {
            _innerContainer.Instance.RegisterInstance(type, implementation);
        }

        public void RegisterToSelf<TImplementation>(RegisterScope scope = RegisterScope.PerInject)
        {
            throw new NotImplementedException();
        }

        public void RegisterComponent<TComponent>() where TComponent : IComponentActivator
        {
            _innerContainer.Register<IComponentActivator, TComponent>();
        }

        public TContract Get<TContract>() => _innerContainer.Resolve<TContract>();

        public TContract Get<TContract>(params ConstructorParameter[] parameters) => _innerContainer.Resolve<TContract>(
            parameters.Select(t => (t.Value.GetType(), t.Value)).ToArray());

        public object Get(Type contractType) => _innerContainer.Resolve(contractType);

        public object Get(Type contractType, params ConstructorParameter[] parameters)
            => _innerContainer.Resolve(contractType, parameters.Select(t => (t.Value.GetType(), t.Value)).ToArray());


        public void RegisterConfiguration<T>()
            => _innerContainer.Instance.RegisterInitializer<T>((_, resolverContext) =>
                resolverContext.Resolve<IConfigObjectProvider>().Get<T>());
        //Probably(?) equivalent to: _innerKernel.Bind<T>().ToMethod(c => c.Kernel.Get<IConfigObjectProvider>().Get<T>());

        public void Unbind<TContract>()
        {
            _innerContainer.Instance.Unregister<TContract>();
        }

        public object Resolve(Type type) =>
            _innerContainer.Resolve(type);

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters) =>
            _innerContainer.Resolve(type, parameters);

        public object Resolve(Type type, string name) =>
            _innerContainer.Resolve(type, name);

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters) =>
            _innerContainer.Resolve(type, name, parameters);

        public IScopedProvider CreateScope() => _innerContainer.CreateScope();

        public IScopedProvider CurrentScope => _innerContainer.CurrentScope;

        public IContainerRegistry RegisterInstance(Type type, object instance) =>
            _innerContainer.RegisterInstance(type, instance);

        public IContainerRegistry RegisterInstance(Type type, object instance, string name) =>
            _innerContainer.RegisterInstance(type, instance, name);

        public IContainerRegistry RegisterSingleton(Type from, Type to) =>
            _innerContainer.RegisterSingleton(from, to);

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name) =>
            _innerContainer.RegisterSingleton(from, to, name);

        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod) =>
            _innerContainer.RegisterSingleton(type, factoryMethod);

        public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod) =>
            _innerContainer.RegisterSingleton(type, factoryMethod);

        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes) =>
            _innerContainer.RegisterManySingleton(type, serviceTypes);

        public IContainerRegistry Register(Type from, Type to) =>
            _innerContainer.Register(from, to);

        public IContainerRegistry Register(Type from, Type to, string name) =>
            _innerContainer.Register(from, to, name);

        public IContainerRegistry Register(Type type, Func<object> factoryMethod) =>
            _innerContainer.Register(type, factoryMethod);

        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod) =>
            _innerContainer.Register(type, factoryMethod);

        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes) =>
            _innerContainer.RegisterMany(type, serviceTypes);

        public IContainerRegistry RegisterScoped(Type from, Type to) =>
            _innerContainer.RegisterScoped(from, to);

        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod) =>
            _innerContainer.RegisterScoped(type, factoryMethod);

        public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod) =>
            _innerContainer.RegisterScoped(type, factoryMethod);

        public bool IsRegistered(Type type) =>
            _innerContainer.IsRegistered(type);

        public bool IsRegistered(Type type, string name) =>
            _innerContainer.IsRegistered(type, name);

        public void FinalizeExtension() =>
            _innerContainer.FinalizeExtension();
    }
}
