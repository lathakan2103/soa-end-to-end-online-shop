using Core.Common.Contracts;
using Core.Common.Data;

namespace Demo.Data
{
    public abstract class DataRepositoryBase<T> : DataRepositoryBase<T, DemoContext>
        where T : class, IIdentifiableEntity, new()
    {
    }
}
