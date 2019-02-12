using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;

namespace AWSCore
{
    using Interfaces;
    using Enums;

    public class IAM : BaseService, IIAM
    {
        private AmazonIdentityManagementServiceClient m_client;

        public IAM(Credentials credentials) : base("IAM", ServiceTypes.IAM)
        {
            m_client = new AmazonIdentityManagementServiceClient(credentials.PublicKey, credentials.SecretKey, credentials.EndPoint);
        }

        public async Task<ListRolesResponse> GetRoles()
        {
            ListRolesRequest request = new ListRolesRequest();
            ListRolesResponse response = await m_client.ListRolesAsync(request);
            return response;
        }
    }
}
