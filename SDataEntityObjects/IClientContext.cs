using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDataEntityObjects
{
    public interface IClientContext : IDisposable
    {
        IQueryable<T> CreateQuery<T>();

        T GetById<T>(object id, string attributes);

        T GetById<T>(object id);

        T CreateNew<T>();
    }
}
