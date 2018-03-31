using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.DataTypes.Collections
{
    /// <summary>
    /// This class is used for the bonus marks section of milestone 5 to save username - connectionID pairs.
    /// When you connect to a SignalR "hub" on the server, you do not have access to the HttpContext.Current.Session object
    /// because that object is tied to the controllers. Because of this, the Globals.getUser() function will not work.
    /// This is where this class comes in. Because we dont have the HttpContext to keep track of who is online for us, 
    /// we will use this class instead. When a user first connects to the hub by initializing the connection on he client side,
    /// the connectionid can be found by using the function "this.Context.ConnectionId;" in a class that extends the "Hub" class.
    /// Once you add the connectionID and username pair to this class, you can get the connection id of a user if they are online.
    /// This connectionID can be used by SignalR to call a javascript function on the users machine.
    /// </summary>
    public partial class ConnectionDictionary
    {
        public ConnectionDictionary()
        {
            connectionKey = new Dictionary<string, string>();
            userKey =  new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds the given pair to the dictionary if it does not already e
        /// </summary>
        /// <param name="Username">The username of the user</param>
        /// <param name="ConnectionID">The connectionID of the user</param>
        public void Add(string Username, string ConnectionID)
        {
            try
            {
                userKey.Add(Username, ConnectionID);
                connectionKey.Add(ConnectionID, Username);
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Removes a key pair based on the given username
        /// </summary>
        /// <param name="Username">The user to remove</param>
        public void RemoveByUser(string Username)
        {
            string ConnectionID = this.getConnectionID(Username);

            if (ConnectionID != null)
            {
                connectionKey.Remove(ConnectionID);
                userKey.Remove(Username);
            }
        }

        /// <summary>
        /// Removes a key pair based on the given ConnectionID
        /// </summary>
        /// <param name="ConnectionID">The connectionID to remove</param>
        public void RemoveByConnectionID(string ConnectionID)
        {
            string Username = this.getUsername(ConnectionID);

            if (Username != null)
            {
                connectionKey.Remove(ConnectionID);
                userKey.Remove(Username);
            }
        }

        /// <summary>
        /// Returns the ConnectionID pair of the given username
        /// </summary>
        /// <param name="Username">The username to get the connectionID for</param>
        /// <returns>The connectionID if it exists, null if it does not</returns>
        public string getConnectionID(string Username)
        {
            string ConnectionID = null;
            if(userKey.TryGetValue(Username, out ConnectionID) == false)
            {
                return null;
            }
            return ConnectionID;
        }

        /// <summary>
        /// Returns the Username pair of the given username
        /// </summary>
        /// <param name="ConnectionID">The ConnectionID to get the Username for</param>
        /// <returns>The connectionID if it exists, null if it does not</returns>
        public string getUsername(string ConnectionID)
        {
            string Username = null;
            if(connectionKey.TryGetValue(ConnectionID, out Username) == false)
            {
                return null;
            }
            return Username;
        }
    }

    public partial class ConnectionDictionary
    {
        /// <summary>
        /// This Dictionary contains the ConnectionID's as Keys and the Username's as values
        /// </summary>
        private static Dictionary<string, string> connectionKey;

        /// <summary>
        /// This Dictionary contains the Username's as Keys and the ConnectionID's as values
        /// </summary>
        private static Dictionary<string, string> userKey;
    }
}
