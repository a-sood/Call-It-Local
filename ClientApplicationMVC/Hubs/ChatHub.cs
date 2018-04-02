using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ClientApplicationMVC.Models;
using Messages;
using Messages.DataTypes.Collections;
using Microsoft.AspNet.SignalR;

namespace ClientApplicationMVC.Hubs
{
    public class ChatHub : Hub
    {
        // Any public void Function here can be called by client using javascript

        // Registers the user in the list of active users
        public override Task OnConnected()
        {
            // Add User to the list of active users
            if (active_users == null)
                active_users = new ConnectionDictionary();
            string username = Context.QueryString["username"];
            active_users.Add(username, Context.ConnectionId);
            return base.OnConnected();
        }

        // Unregisters the user in the list of active users
        public override Task OnDisconnected(bool stopCalled)
        {
            // Remove User from the list of active users
            if(active_users != null && active_users.getUsername(Context.ConnectionId) != null)
                active_users.RemoveByConnectionID(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        // Called by the sender, invokes a function in the reciever's client
        public void SendMessage(string recipient, string userData)
        {
            // CHECK IF RECIPIENT IS ONLINE
            string recipient_conn_id = active_users.getConnectionID(recipient);

            if(recipient_conn_id != null)
            {
                // UPDATE MESSAGE RECIEVER'S CHAT IF THEY ARE AVAILABLE
                this.Clients.Client(recipient_conn_id).addTextToChatBox(userData, Context.QueryString["username"]);
            }
        }

        private static ConnectionDictionary active_users;
    }
}