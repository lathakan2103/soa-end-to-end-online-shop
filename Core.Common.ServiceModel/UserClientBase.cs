using System.ServiceModel;
using System.Threading;

namespace Core.Common.ServiceModel
{
    public abstract class UserClientBase<T> : ClientBase<T> where T : class
    {
        /// <summary>
        /// accessing soap header to transport username for our wcf services
        /// </summary>
        public UserClientBase()
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;
            MessageHeader<string> header = new MessageHeader<string>(userName);

            OperationContextScope contextScope =
                            new OperationContextScope(InnerChannel);

            OperationContext.Current.OutgoingMessageHeaders.Add(
                                      header.GetUntypedHeader("String", "System"));
        }
    }
}