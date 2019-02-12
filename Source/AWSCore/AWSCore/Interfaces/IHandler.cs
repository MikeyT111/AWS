using System.Threading.Tasks;

namespace AWSCore.Interfaces
{
    /// <summary>
    /// Any handler that we create should implate the IHandler interface.
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Used to display options to the user.
        /// </summary>
        /// <returns></returns>
        Task Display();
    }
}
