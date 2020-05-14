using Proxy.One;
using System;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyOne.Run();
        }

        static void A()
        {
            ////Console.WriteLine("Hello World!");

            ////var testService = ProxyGenerator.Instance.CreateInterfaceProxy<ITestService>();
            ////Console.WriteLine("1111");
            ////testService.Test();

            ////var testService = ProxyGenerator.Instance.CreateInterfaceProxy<ITestService>();
            //var testService = ProxyGenerator.Instance.CreateInterfaceProxy<ITestService, TestService>();
            //// var testService = ProxyGenerator.Instance.CreateClassProxy<TestService>();
            //// testService.TestProp = "12133";
            //testService.Test();
            //Console.WriteLine();
            //testService.Test1(1, "str");

            //var a = testService.Test2();

            //var b = testService.Test3();
            //Console.WriteLine($"a:{a}, b:{b}");
            //Console.ReadLine();
        }
    }
}
