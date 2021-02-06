using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using Ninject;

namespace Fateblade.PersonManagementApp.CoCo.Core.NinjectPrismAdapter
{
    public class KernelContainer : IKernelContainer
    {
        private readonly IKernel _kernel;
        private static readonly object _lock = new object();
        private static ICoCoKernel _innerKernel;

        public ICoCoKernel Kernel 
        {
            get
            {
                lock (_lock)
                {
                    if (_innerKernel == null && _kernel != null )
                    {
                        _innerKernel = new KernelAdapter(_kernel);
                        _innerKernel.RegisterUnique(typeof(ICoCoKernel), this);
                    }

                    return _innerKernel;
                }
            }
        }

        public KernelAdapter CastedKernel => Kernel as KernelAdapter;

        public KernelContainer(IKernel kernel)
        {
            _kernel = kernel;
        }

        public KernelContainer() 
            : this(new StandardKernel())
        {

        }
    }
}
