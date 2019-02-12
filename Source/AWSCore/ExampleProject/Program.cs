using System;
using System.Threading.Tasks;
using AWSCore;
using AWSCore.Interfaces;

namespace ExampleProject
{
    class Program
    {
        private static bool m_shouldBreak = false;

        private static ISQS m_sqsClient;
        private static ISNS m_snsClient;
        private static IDatabase m_databaseClient;
        private static ILambda m_lambdaClient;
        private static IS3 m_s3Client;
        private static IIAM m_iamClient;

        private static IHandler m_s3Handler;
        private static IHandler m_databaseHandler;
        private static IHandler m_lambdaHandler;
        private static IHandler m_queueHandler;
        private static IHandler m_notificationHandler;
        private static IHandler m_iamHandler;

        static async Task Main(string[] args)
        {
            Credentials cred = new Credentials("TestAccount", "Secret-Key-Goes-Here", "Public-Key-Goes-Here", Amazon.RegionEndpoint.USEast2);

            m_snsClient = new SNS(cred);
            m_sqsClient = new SQS(cred);
            m_databaseClient = new Database(cred);
            m_lambdaClient = new Lambda(cred);
            m_s3Client = new S3(cred);
            m_iamClient = new IAM(cred);

            m_s3Handler = new S3Handler(m_s3Client);
            m_databaseHandler = new DatabaseHandler(m_databaseClient);
            m_lambdaHandler = new LambdaHandler(m_lambdaClient, m_iamClient, m_s3Client);
            m_queueHandler = new QueueHandler(m_sqsClient);
            m_notificationHandler = new NotificationHandler(m_snsClient, m_sqsClient, m_lambdaClient);
            m_iamHandler = new IAMHandler(m_iamClient);

            while (!m_shouldBreak)
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("1: Notification Options");
                Console.WriteLine("2: Queue Options");
                Console.WriteLine("3: Database Options");
                Console.WriteLine("4: Lambda Options");
                Console.WriteLine("5: S3 Options");
                Console.WriteLine("6: IAM Options");

                var command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        await m_notificationHandler.Display();
                        break;
                    case "2":
                        await m_queueHandler.Display();
                        break;
                    case "3":
                        await m_databaseHandler.Display();
                        break;
                    case "4":
                        await m_lambdaHandler.Display();
                        break;
                    case "5":
                        await m_s3Handler.Display();
                        break;
                    case "6":
                        await m_iamHandler.Display();
                        break;
                    default:
                        Console.WriteLine("Invalid commmand");
                        break;
                }
            }
        }
    }
}
