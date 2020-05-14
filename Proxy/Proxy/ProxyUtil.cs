using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Linq;
using Proxy.Aspect;

namespace Proxy
{
    internal class ProxyUtil
    {
        private const string ProxyAssemblyName = "Aop.DynamicGenerated";
        private static readonly ModuleBuilder _moduleBuilder;
        private static readonly ConcurrentDictionary<string, Type> _proxyTypes = new ConcurrentDictionary<string, Type>();
        static ProxyUtil()
        {
            // 定义一个动态程序集
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(ProxyAssemblyName), AssemblyBuilderAccess.Run);
            // 创建一个动态模块，后面创建动态代理类通过这个来创建
            _moduleBuilder = asmBuilder.DefineDynamicModule("Default");
        }

        public static Type CreateInterfaceProxy(Type interfaceType)
        {
            var proxyName = $"{ProxyAssemblyName}.{interfaceType.FullName}";

            var type = _proxyTypes.GetOrAdd(proxyName, name =>
            {
                var typeBuilder = _moduleBuilder.DefineType(proxyName, TypeAttributes.Public, typeof(object), new[] { interfaceType });

                typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

                var methods = interfaceType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

                foreach (var method in methods)
                {
                    // 在动态类中定义方法，方法名称，返回值和签名与接口方法保持一致
                    var methodBuilder = typeBuilder.DefineMethod(method.Name,
                        MethodAttributes.Public | MethodAttributes.Virtual,
                        method.CallingConvention,
                        method.ReturnType,
                        method.GetParameters().Select(x => x.ParameterType).ToArray());

                    // 获取 ILGenerator，通过 Emit 实现方法体
                    var ilGenerator = methodBuilder.GetILGenerator();
                    ilGenerator.EmitWriteLine($"method [{method.Name}] is invoking...");
                    ilGenerator.Emit(OpCodes.Ret);

                    // 定义方法实现
                    typeBuilder.DefineMethodOverride(methodBuilder, method);
                }


                return typeBuilder.CreateType();
            });
            return type;
        }
        public static Type CreateInterfaceProxy(Type interfaceType, Type implementationType)
        {
            var proxyName = $"{ProxyAssemblyName}.{interfaceType.FullName}";

            var type = _proxyTypes.GetOrAdd(proxyName, name =>
            {
                var typeBuilder = _moduleBuilder.DefineType(proxyName, TypeAttributes.Public, typeof(object), new[] { interfaceType });

                typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

                var methods = interfaceType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

                foreach (var method in methods)
                {
                    // 在动态类中定义方法，方法名称，返回值和签名与接口方法保持一致
                    var methodBuilder = typeBuilder.DefineMethod(method.Name,
                        MethodAttributes.Public | MethodAttributes.Virtual,
                        method.CallingConvention,
                        method.ReturnType,
                        method.GetParameters().Select(x => x.ParameterType).ToArray());

                    // 获取 ILGenerator，通过 Emit 实现方法体
                    var il = methodBuilder.GetILGenerator();
                    
                    var localAspectInvocation = il.DeclareLocal(typeof(MethodInvocationContext));
                    //il.Emit(OpCodes.Ldloc, localCurrentMethod);
                    //il.Emit(OpCodes.Ldloc, localMethodBase);

                    // 定义方法实现
                    typeBuilder.DefineMethodOverride(methodBuilder, method);
                }


                return typeBuilder.CreateType();
            });
            return type;
        }
    }
    public class ProxyGenerator
    {
        public static readonly ProxyGenerator Instance = new ProxyGenerator();

        public object CreateInterfaceProxy(Type interfaceType)
        {
            var type = ProxyUtil.CreateInterfaceProxy(interfaceType);
            return Activator.CreateInstance(type);
        }
        public object CreateInterfaceProxy(Type interfaceType, Type implementationType)
        {
            var type = ProxyUtil.CreateInterfaceProxy(interfaceType, implementationType);
            return Activator.CreateInstance(type);
        }
    }
    // 定义泛型扩展
    public static class ProxyGeneratorExtensions
    {
        public static TInterface CreateInterfaceProxy<TInterface>(this ProxyGenerator proxyGenerator) =>
            (TInterface)proxyGenerator.CreateInterfaceProxy(typeof(TInterface));
    }
}
