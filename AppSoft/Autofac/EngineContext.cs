using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSoft.Autofac
{
    public class EngineContext
    {
        public static IEngine Create()
        {
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new Engine());
        }

        public static void Replace(IEngine engine) => Singleton<IEngine>.Instance = engine;
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }
                return Singleton<IEngine>.Instance;
            }
        }
    }
}
