using Messages;
using Messages.Database;
using Messages.DataTypes;
using Messages.NServiceBus.Events;
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

        //TODO -- ADD THE METHODS HERE TO INTERACT WITH THE DATABASE
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
