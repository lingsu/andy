using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public interface ITestService
    {
        [TryInvokeAspect]
        void Test();

        [TryInvokeAspect]
        [TryInvoke1Aspect]
        void Test1(int a, string b);
    }
    public class TestService : ITestService
    {
        [TryInvokeAspect]
        public virtual string TestProp { get; set; }

        public void Test()
        {
            Console.WriteLine("test invoked");
        }

        public virtual void Test1(int a, string b)
        {
            Console.WriteLine($"a:{a}, b:{b}");
        }

        [TryInvoke1Aspect]
        public virtual string Test2()
        {
            return "Hello";
        }

        [TryInvokeAspect]
        public virtual int Test3()
        {
            return 1;
        }
    }
}
