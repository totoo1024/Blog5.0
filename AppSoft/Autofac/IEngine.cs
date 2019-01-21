using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSoft.Autofac
{
    public interface IEngine
    {
        T Resolve<T>() where T : class;
        object Resolve(Type type);
        IEnumerable<T> ResolveAll<T>();
        object ResolveUnregistered(Type type);
    }
}
