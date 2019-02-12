using System;
using System.Collections.Generic;
using System.Text;

namespace AWSCore
{

    public class Topic
    {
        public Topic()
        {
            Name = string.Empty;
            ARN = string.Empty;
        }

        public Topic(string name)
        {
            Name = name;
            ARN = string.Empty;
        }

        public Topic(string name, string arn)
        {
            Name = name;
            ARN = arn;
        }

        public string Name { get; }
        public string ARN { get; }
    }
}
