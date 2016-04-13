namespace Core.Common.Contracts
{
    /// <summary>
    /// service factory
    /// </summary>
    public interface IServiceFactory
    {
        T CreateClient<T>() where T : IServiceContract;
    }
}