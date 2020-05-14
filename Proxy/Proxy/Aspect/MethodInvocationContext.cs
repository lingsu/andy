using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Proxy.Aspect
{
    public class MethodInvocationContext
    {
        public MethodInfo ProxyMethod { get; }

        public MethodInfo MethodBase { get; }

        public object ProxyTarget { get; }

        public object Target { get; }

        public object[] Parameters { get; }

        public object ReturnValue { get; set; }

        public MethodInvocationContext(MethodInfo method, MethodInfo methodBase, object proxyTarget, object target, object[] parameters)
        {
            ProxyMethod = method;
            MethodBase = methodBase;
            ProxyTarget = proxyTarget;
            Target = target;
            Parameters = parameters;
        }
    }
}
