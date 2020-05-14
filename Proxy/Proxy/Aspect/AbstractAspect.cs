using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Aspect
{
    public abstract class AbstractAspect : Attribute
    {
        public abstract void Invoke(MethodInvocationContext methodInvocationContext, Action next);
    }
}
