using Messages;
using Messages.DataTypes.Database.Chat;
using Messages.NServiceBus.Commands;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.ServiceBusRequest.Chat.Responses;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Handlers
{
    class GetChatHistoryHandler : IHandleMessages<GetChatHistoryRequest>
    {
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<GetChatHistoryRequest>();

        public async Task Handle(GetChatHistoryRequest message, IMessageHandlerContext context)
        {
            await HandleAsync(message, context);
        }

        /// <summary>
        /// Saves the echo to the database, reverses the data, and returns it back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public async Task HandleAsync(GetChatHistoryRequest message, IMessageHandlerContext context)
        {
            Debug.consoleMsg("GET HISTORY MESSAGE \nBETWEEN: " + message.getCommand.history.user1 + " AND " + message.getCommand.history.user1);

            // TODO: IMPLEMENT GET CHAT HISTORY HERE
            
            GetChatHistory history = message.getCommand; // Mirroring back the message from the client 

            /** Test Data (to avoid null ptr errors) **/
            ChatMessage m1 = new ChatMessage();
            ChatMessage m2 = new ChatMessage();
            m1.sender = m2.receiver = message.getCommand.history.user1;
            m2.sender = m1.receiver = message.getCommand.history.user2;
            m1.unix_timestamp = 11111111;
            m2.unix_timestamp = 11111122;
            m1.messageContents = "Message 1";
            m2.messageContents = "Message 2";

            history.history.messages.Add(m1);
            history.history.messages.Add(m2);
            /** End of Test Data **/

            GetChatHistoryResponse response = new GetChatHistoryResponse(false, "History Fetched Successfully", history);
            await context.Reply(response);
        }
    }
}
