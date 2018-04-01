using ChatService.Database;
using Messages.NServiceBus.Commands;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.ServiceBusRequest.Chat.Responses;
using Messages.ServiceBusRequest.Echo.Requests;

using NServiceBus;
using NServiceBus.Logging;

using System;
using System.Threading.Tasks;

namespace ChatService.Handlers
{
    public class GetChatHistoryHandler : IHandleMessages<GetChatHistoryRequest>
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
        public Task HandleAsync(GetChatHistoryRequest request, IMessageHandlerContext context)
        {
            ChatServiceDatabase db = ChatServiceDatabase.getInstance();

            GetChatHistoryResponse response = db.GetChatHistory(request);

            return context.Reply(response);
        }
    }
}
