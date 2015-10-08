using System;
using System.Reflection;

namespace DotNetSpy
{
    public class DynamicInvokeMember : MarshalByRefObject
    {
        private static readonly string DomainName = Guid.NewGuid().ToString();

        public static object InvokeMember(
            string file,
            string type,
            string member,
            BindingFlags invokeAttr,
            Binder binder,
            params object[] args
            )
        {
            object result = null;

            try
            {
                AppDomain domain = null;
                try
                {
                    domain = AppDomain.CreateDomain(DomainName);
                    DynamicInvokeMember loader = (DynamicInvokeMember)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DynamicInvokeMember).FullName);
                    result = loader.InvokeMemberInternal(file, type, member, invokeAttr, binder, args);
                }
                catch { throw; }
                finally
                {
                    if (domain != null)
                    {
                        AppDomain.Unload(domain);
                    }
                }
            }
            catch { throw; }

            return result;
        }

        private object InvokeMemberInternal(
            string file,
            string type,
            string member,
            BindingFlags invokeAttr,
            Binder binder,
            object[] args)
        {
            Type objType = Assembly.LoadFile(file).GetType(type);

            object instance = null;
            if (!objType.IsAbstract)
            {
                instance = Activator.CreateInstance(objType, true);
            }
            return objType.InvokeMember(member, invokeAttr, binder, instance, args);
        }
    }
}