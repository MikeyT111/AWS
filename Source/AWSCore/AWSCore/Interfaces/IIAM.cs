using Amazon.IdentityManagement.Model;
using System.Threading.Tasks;

namespace AWSCore.Interfaces
{
    public interface IIAM
    {
        Task<ListRolesResponse> GetRoles();
    }
}
