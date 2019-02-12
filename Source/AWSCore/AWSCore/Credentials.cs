using Amazon;

namespace AWSCore
{
    /// <summary>
    /// Class to store the basic credential information.
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="profileName">The name of the profile.</param>
        /// <param name="secretKey">Your secret key.</param>
        /// <param name="publicKey">Your public key.</param>
        /// <param name="endpoint">The region you want to connect to.</param>
        public Credentials(string profileName, string secretKey, string publicKey, RegionEndpoint endpoint)
        {
            ProfileName = profileName;
            SecretKey = secretKey;
            PublicKey = publicKey;
            EndPoint = endpoint;
        }

        /// <summary>
        /// The name of the profile.
        /// </summary>
        public string ProfileName { get; }

        /// <summary>
        /// Your secret key.
        /// </summary>
        public string SecretKey { get; }

        /// <summary>
        /// Your public key.
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        /// The region you want to connect to.
        /// </summary>
        public RegionEndpoint EndPoint { get; }
    }
}
