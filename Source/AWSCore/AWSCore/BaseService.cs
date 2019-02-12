namespace AWSCore
{
    using Enums;
    using Interfaces;

    /// <summary>
    /// Base Service information.
    /// </summary>
    public class BaseService : IService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="serviceType">The type of the service.</param>
        public BaseService(string name, ServiceTypes serviceType)
        {
            Name = name;
            ServiceType = serviceType;
        }

        /// <summary>
        /// The name of the service.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type of service.
        /// </summary>
        public ServiceTypes ServiceType { get; }

    }
}
