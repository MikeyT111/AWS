namespace AWSCore.Interfaces
{
    using Enums;

    /// <summary>
    /// Interface to set out the basic information.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// The name of the service.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The service type.
        /// </summary>
        ServiceTypes ServiceType { get; }
    }
}
