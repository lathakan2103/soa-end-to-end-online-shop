namespace Core.Common.Contracts
{
    /// <summary>
    /// service factory
    /// </summary>
    public interface IServiceFactory
    {
        T CreateClient<T>() where T : IServiceContract;

        /// <summary>
        /// for dynamic endpoints
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        T CreateClient<T>(string endpoint) where T : IServiceContract;
    }
}