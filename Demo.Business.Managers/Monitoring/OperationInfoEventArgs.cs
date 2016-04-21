using System;

namespace Demo.Business.Managers.Monitoring
{
    public class OperationInfoEventArgs : EventArgs
    {
        public string ServiceName { get; set; }
        public string OperationName { get; set; }
        public DateTime Timestamp { get; private set; }
        public string Direction { get; set; }

        public OperationInfoEventArgs(string serviceName, string operationName, string direction)
        {
            this.ServiceName = serviceName;
            this.OperationName = operationName;
            this.Direction = direction;
            this.Timestamp = DateTime.Now;
        }
    }
}
