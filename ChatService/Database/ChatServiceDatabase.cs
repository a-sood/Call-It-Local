using Messages;
using Messages.Database;
using Messages.DataTypes;
using Messages.DataTypes.Database.Chat;
using Messages.NServiceBus.Commands;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.ServiceBusRequest.Chat.Responses;
using Messages.ServiceBusRequest.Echo.Requests;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Database
{
    public partial class ChatServiceDatabase : AbstractDatabase
    {
        private ChatServiceDatabase() { }

        public static ChatServiceDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new ChatServiceDatabase();
            }
            return instance;
        }

        public GetChatContactsResponse GetChatContacts(GetChatContactsRequest request)
        {
            bool result = false;
            string message = "";
            GetChatContacts contacts = new GetChatContacts();
            contacts.contactNames = new List<string>();
            if (openConnection() == true)
            {
                string query = @"(SELECT DISTINCT receiver FROM " + databaseName + @".chats WHERE sender = '" + request.getCommand.usersname + @"') UNION " +
                    @"(SELECT DISTINCT sender FROM " + databaseName + @".chats WHERE receiver = '" + request.getCommand.usersname + @"');";

                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = command.ExecuteReader();

                    result = true;
                    message = "no contacts exists for this username";
                    while (dataReader.Read())
                    { 
                        contacts.contactNames.Add(dataReader.GetString("receiver"));
                    }
                    if(contacts.contactNames.Count != 0)
                    {
                        message = "contacts are read successfully.";
                    }
                    dataReader.Close();
                }
                catch (MySqlException e)
                {
                    Messages.Debug.consoleMsg("Unable to complete read company name from the database" +
                        " Error :" + e.Number + e.Message);
                    Messages.Debug.consoleMsg("The query was:" + query);
                    message = e.Message;
                    closeConnection();

                    return new GetChatContactsResponse(result, message, contacts);
                }
                catch (Exception e)
                {
                    Messages.Debug.consoleMsg("Unable to complete read company name from the database." +
                        " Error:" + e.Message);
                    message = e.Message;
                    closeConnection();
                    return new GetChatContactsResponse(result, message, contacts);
                }

                closeConnection();
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }

            return new GetChatContactsResponse(result, message, contacts);
        }

        public GetChatHistoryResponse GetChatHistory(GetChatHistoryRequest request)
        {
            bool result = false;
            string message = "";
            GetChatHistory historyCommand = new GetChatHistory();
            historyCommand.history = new ChatHistory();

            historyCommand.history.user1 = request.getCommand.history.user1;
            historyCommand.history.user2 = request.getCommand.history.user2;

            historyCommand.history.messages = new List<ChatMessage>();

            if (openConnection() == true)
            {
                string query = @"SELECT * FROM " + databaseName + @".chats WHERE (sender = '"
                               + request.getCommand.history.user1 + @"' AND receiver = '" + request.getCommand.history.user2
                               + @"') OR (sender = '" + request.getCommand.history.user2 + @"' AND receiver = '" + request.getCommand.history.user1 + @"');";
                  
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = command.ExecuteReader();

                    result = true;
                    message = "no history exists for this pair";
                    while (dataReader.Read())
                    {
                        ChatMessage chatMessage = new ChatMessage();
                        chatMessage.sender = dataReader.GetString("sender");
                        chatMessage.receiver = dataReader.GetString("receiver");
                        chatMessage.unix_timestamp = dataReader.GetInt32("timestamp");
                        chatMessage.messageContents = dataReader.GetString("message");

                        historyCommand.history.messages.Add(chatMessage);
                    }
                    if (historyCommand.history.messages.Count != 0)
                    {
                        message = "chat histroy read successfully.";
                    }
                    dataReader.Close();
                }
                catch (MySqlException e)
                {
                    Messages.Debug.consoleMsg("Unable to complete read company name from the database" +
                        " Error :" + e.Number + e.Message);
                    Messages.Debug.consoleMsg("The query was:" + query);
                    message = e.Message;
                    closeConnection();

                    return new GetChatHistoryResponse(result, message, historyCommand);
                }
                catch (Exception e)
                {
                    Messages.Debug.consoleMsg("Unable to complete read company name from the database." +
                        " Error:" + e.Message);
                    message = e.Message;
                    closeConnection();
                    return new GetChatHistoryResponse(result, message, historyCommand);
                }

                closeConnection();
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }

            return new GetChatHistoryResponse(result, message, historyCommand);
        }

        public ServiceBusResponse SendMessage(SendMessageRequest request)
        {
            bool result = false;
            string message = "";

            if (openConnection() == true)
            {
                string query = @"INSERT INTO " + databaseName + @".chats(timestamp, sender, receiver, message) VALUES("
                               + request.message.unix_timestamp + @", '" + request.message.sender + @"', '"
                               + request.message.receiver + @"', '" + request.message.messageContents + @"');";

                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    result = true;
                    message = "message sent successfully";
                    
                }
                catch (MySqlException e)
                {
                    Messages.Debug.consoleMsg("Unable to complete insert into the database" +
                        " Error :" + e.Number + e.Message);
                    Messages.Debug.consoleMsg("The query was:" + query);
                    message = e.Message;
                    closeConnection();
                    return new ServiceBusResponse(result, message);
                }
                catch (Exception e)
                {
                    Messages.Debug.consoleMsg("Unable to complete read company name from the database." +
                        " Error:" + e.Message);
                    message = e.Message;
                    closeConnection();
                    return new ServiceBusResponse(result, message);
                }

                closeConnection();
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }

            return new ServiceBusResponse(result, message);
        }
    }


    public partial class ChatServiceDatabase : AbstractDatabase
    {
        private const String dbname = "chatservicedb";
        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton isntance of the database
        /// </summary>
        protected static ChatServiceDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
            (
                dbname,
                "chats",
                new Column[]
                {
                    new Column
                    (
                        "id", "INT(64)",
                        new string[]
                        {
                            "NOT NULL",
                            "UNIQUE",
                            "AUTO_INCREMENT"
                        }, true
                    ),
                    new Column
                    (
                        "timestamp", "INT(32)",
                        new string[]
                        {
                            "NOT NULL",
                        }, false
                    ),
                    new Column
                    (
                        "sender", "VARCHAR(50)",
                        new string[] 
                        {
                            "NOT NULL",
                        },
                        false
                    ),
                    new Column
                    (
                        "receiver", "VARCHAR(50)",
                        new string[]
                        {
                            "NOT NULL",
                        },
                        false
                    ),
                    new Column
                    (
                        "message", "VARCHAR(300)",
                        new string[]
                        {
                            "NOT NULL",
                        },
                        false
                    ),
                }
            )          
        };
    }
}
