using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDataEntityObjects
{
    public interface IClientContextFactory
    {
        IClientContext CreateContext(IContextConfiguration configuration);
    }
}
