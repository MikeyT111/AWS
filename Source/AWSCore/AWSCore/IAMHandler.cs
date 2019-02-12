using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSCore
{
    using Interfaces;
    
    public class IAMHandler : IHandler
    {
        private IIAM m_iam;
        private bool m_shouldContinue = false;
        public IAMHandler(IIAM iam)
        {
            m_iam = iam;
        }

        public async Task Display()
        {
            m_shouldContinue = true;

            while (m_shouldContinue)
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("1: Get roles");
                Console.WriteLine("9: Exit");

                var command = Utilities.DisplayMessageAndGetStringResult("Enter number:");

                switch (command)
                {
                    case "1":
                        await DisplayRoles();
                        break;
                    case "9":
                        m_shouldContinue = false;
                        break;
                }
            }
        }

        private async Task DisplayRoles()
        {
            var roles = await m_iam.GetRoles();
            if (roles.Roles.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Found Roles:");
                roles.Roles.ForEach(x => Console.WriteLine(x.RoleName));
                Console.WriteLine("");
            }
        }
    }
}
