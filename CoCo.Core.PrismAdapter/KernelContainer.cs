using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using Prism.Ioc;

namespace Fateblade.PersonManagementApp.CoCo.Core.PrismAdapter
{
    public class KernelContainer : IKernelContainer
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IContainerRegistry _containerRegistry;
        private static readonly object _lock = new object();
        private static ICoCoKernel _innerKernel;

        public ICoCoKernel Kernel 
        {
            get
            {
                lock (_lock)
                {
                    if (_innerKernel == null && _containerRegistry!=null && _containerProvider!=null)
                    {
                        _innerKernel = new KernelAdapter(_containerRegistry, _containerProvider);
                        _innerKernel.Register<ICoCoKernel, KernelAdapter>();
                    }

                    return _innerKernel;
                }
            }
        }

        public KernelContainer(IContainerRegistry containerRegistry, IContainerProvider containerProvider)
        {
            _containerRegistry = containerRegistry;
            _containerProvider = containerProvider;
        }
    }
}
