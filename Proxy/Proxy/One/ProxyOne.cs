using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Linq;

namespace Proxy.One
{
    public static class ProxyOne
    {
        public static void Run()
        {
            // 定义一个动态程序集
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("MyClass123"), AssemblyBuilderAccess.Run);
            // 创建一个动态模块，后面创建动态代理类通过这个来创建
            var moduleBuilder = asmBuilder.DefineDynamicModule("Default");

            //定义一个类
            var classBuilder = moduleBuilder.DefineType("abc", TypeAttributes.Public);

            //定义一个方法
            var methodBuilder = classBuilder.DefineMethod("mymethod", MethodAttributes.Public, null, null);

            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "生成的第一个程序");
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.Emit(OpCodes.Ret);

            var type = classBuilder.CreateType();
            var instance = Activator.CreateInstance(type);
            var me = type.GetMethod("mymethod");
            me.Invoke(instance, null);
            Console.WriteLine("aa");
           
            //instance

        }
    }
}
