using Proxy.Aspect;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public class TryInvoke1Aspect : AbstractAspect
    {
        public override void Invoke(MethodInvocationContext methodInvocationContext, Action next)
        {
            Console.WriteLine($"begin invoke method {methodInvocationContext.ProxyMethod.Name} in {GetType().Name}...");
            try
            {
                next();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invoke {methodInvocationContext.ProxyMethod.DeclaringType?.FullName}.{methodInvocationContext.ProxyMethod.Name} exception");
                Console.WriteLine(e);
            }
            Console.WriteLine($"end invoke method {methodInvocationContext.ProxyMethod.Name} in {GetType().Name}...");
        }
    }
}
