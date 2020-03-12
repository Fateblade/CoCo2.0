using System;
using System.Collections.Generic;
using System.Text;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;

namespace Fateblade.PersonManagementApp.CoCo.Core.PrismAdapter
{
    public class KernelContainer : IKernelContainer
    {
        private static ICoCoKernel _innerKernel;
        private static readonly object _lock = new object();

        public ICoCoKernel Kernel 
        {
            get
            {
                lock (_lock)
                {
                    if (_innerKernel == null)
                    {

                    }

                    return _innerKernel;
                }
            }
        }
    }
    }
}
