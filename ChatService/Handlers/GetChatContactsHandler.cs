using Messages;
using Messages.NServiceBus.Commands;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.ServiceBusRequest.Chat.Responses;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatService.Handlers 
{
    class GetChatContactsHandler : IHandleMessages<GetChatContactsRequest>
    {
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<GetChatContactsRequest>();

        public async Task Handle(GetChatContactsRequest message, IMessageHandlerContext context)
        {
            await HandleAsync(message, context);
        }

        /// <summary>
        /// Saves the echo to the database, reverses the data, and returns it back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public async Task HandleAsync(GetChatContactsRequest message, IMessageHandlerContext context)
        {
            Debug.consoleMsg("GET CONTACTS MESSAGE FOR: " + message.getCommand.usersname);

            try
            {
                // TODO: IMPLEMENT GET CHAT CONTACTS HERE

                GetChatContacts contacts = message.getCommand; // Mirroring back the message we got from the client
                contacts.contactNames = new List<string>();

                /** Test Data (to avoid null ptr errors) **/
                contacts.contactNames.Add("Microsoft");
                contacts.contactNames.Add("Apple");
                /** End of Test Data **/

                GetChatContactsResponse response = new GetChatContactsResponse(false, "History Fetched Successfully", contacts);
                await context.Reply(response);
            } catch(Exception e)
            {
                GetChatContacts contacts2 = message.getCommand; // Mirroring back the message we got from the client

                /** Test Data (to avoid null ptr errors) **/
                contacts2.contactNames.Add("Microsoft");
                contacts2.contactNames.Add("Apple");
                /** End of Test Data **/

                GetChatContactsResponse response = new GetChatContactsResponse(false, e.Message, contacts2);
            }
        }
    }
}
