using System;
using System.Collections.Generic;
using System.Text;

namespace AWSCore
{
    public class Utilities
    {
        public static bool DisplayMessageAndGetBoolResult(string message)
        {
            bool result = false;
            Console.WriteLine(message);
            string userResult = Console.ReadLine();
            if (userResult.ToLower() == "yes")
            {
                result = true; ;
            }

            return result;
        }

        public static string DisplayMessageAndGetStringResult(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
    }
}
