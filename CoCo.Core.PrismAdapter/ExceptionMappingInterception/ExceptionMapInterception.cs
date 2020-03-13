using Castle.Core.Internal;
using Castle.DynamicProxy;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Aspects;
using System;
using System.Linq;

namespace Fateblade.PersonManagementApp.CoCo.Core.PrismAdapter.ExceptionMappingInterception
{
    public class ExceptionMapInterception : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var inteceptedType = invocation.TargetType;
            var interfaceWithMappingsAttributes =
                inteceptedType.GetInterfaces().FirstOrDefault(i => i.GetAttribute<MapExceptionAttribute>()!=null);
            if (interfaceWithMappingsAttributes != null)
            {
                var attribute = interfaceWithMappingsAttributes.GetAttribute<MapExceptionAttribute>();
                var typeMessage = attribute.Message;
                var targetExceptionType = attribute.TargetException;

                try
                {
                    invocation.Proceed();
                }
                catch (Exception e) when (e.GetType() != targetExceptionType)
                {
                    var methodMessage = invocation.Method.GetAttribute<ExceptionMessageAttribute>()?.Message;

                    if (methodMessage != null)
                    {
                        methodMessage = string.Format(methodMessage, invocation.Arguments);
                    }

                    var exceptionInstance =
                        (Exception) Activator.CreateInstance(targetExceptionType, methodMessage ?? typeMessage, e);
                    throw exceptionInstance;
                }

            }
        }
    }
}
