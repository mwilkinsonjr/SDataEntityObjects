using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDataEntityObjects
{
    public static class ClientFactory
    {
        public static IClientContext GetContext(IClientContextFactory factory, IContextConfiguration configuration)
        {
            return factory.CreateContext(configuration);
        }
    }
}
