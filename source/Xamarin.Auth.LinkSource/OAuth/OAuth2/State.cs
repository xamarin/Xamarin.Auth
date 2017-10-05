using System;

namespace Xamarin.Auth.OAuth2
{
    /// <summary>
    /// OAuth2 Parameter - State.
    /// RECOMMENDED.  An opaque value used by the client to maintain state between the 
    /// request and callback.The authorization server includes this value when redirecting 
    /// the user-agent back to the client.The parameter SHOULD be used for preventing cross-site 
    /// request forgery as described in Section 10.12.
    /// </summary>
    /// <see cref="https://tools.ietf.org/html/rfc6749#section-4.1.1"/>
    /// <see cref="https://tools.ietf.org/html/draft-bradley-oauth-jwt-encoded-state-00"/>
    /// <see cref=""/>
    public partial class State
    {
        public State()
        {
            RandomStateGeneratorFunc = GenerateOAuth2StateRandom;
            this.random_string = RandomStateGeneratorFunc(StateStringLength);

            return;
        }

        public ulong StateStringLength
        {
        	get;
        	set;
        } = 16;

        private string random_string; 

        public string RandomString
        {
        	get
            {
                return random_string;
            }
            set
            {
                random_string = value;

                return;
            }
        }

        /// <summary>
        /// Gets or sets the OAuth2 random state generator func.
        /// </summary>
        /// <value>
        /// The OA uth2 random state generator func.
        /// </value>
        public Func<ulong, string> RandomStateGeneratorFunc
        {
        	get;
        	set;
        }

        public string GenerateOAuth2StateRandom(ulong number_of_characters = 16)
        {
        	//
        	// Generate a unique state string to check for forgeries
        	//
        	var chars = new char[number_of_characters];
        	var rand = new Random();
        	for (var i = 0; i < chars.Length; i++)
        	{
        		chars[i] = (char)rand.Next((int)'a', (int)'z' + 1);
        	}
        	string state_string = new string(chars);

        	return state_string;
        }

    }
}
