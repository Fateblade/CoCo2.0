using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DryIoc;
using Prism.Ioc;

namespace Fateblade.PersonManagementApp.CoCo.Core.DryIocPrismAdapter
{
    public class KernelContainer : IKernelContainer
    {
        private readonly IContainerExtension<IContainer> _kernel;
        private static readonly object _lock = new object();
        private static ICoCoKernel _innerKernel;

        public ICoCoKernel Kernel
        {
            get
            {
                lock (_lock)
                {
                    if (_innerKernel == null && _kernel != null)
                    {
                        _innerKernel = new KernelAdapter(_kernel);
                        _innerKernel.RegisterUnique(typeof(ICoCoKernel), _innerKernel);
                    }

                    return _innerKernel;
                }
            }
        }

        public KernelAdapter CastedKernel => Kernel as KernelAdapter;

        public KernelContainer(IContainerExtension<IContainer> kernel)
        {
            _kernel = kernel;
        }
    }
}
