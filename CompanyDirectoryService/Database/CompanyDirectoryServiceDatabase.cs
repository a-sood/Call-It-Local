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

/** TODO: COMPLETE DATABASE SCHEMA **/
/** TODO: ADD DATABASE MODIFYING FUNCTIONS **/

namespace CompanyDirectoryService.Database
{
    /// <summary>
    /// This portion of the class contains methods and functions
    /// </summary>
    public partial class CompanyDirectoryServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// Private default constructor to enforce the use of the singleton design pattern
        /// </summary>
        private CompanyDirectoryServiceDatabase() { }

        /// <summary>
        /// Gets the singleton instance of the database
        /// </summary>
        /// <returns>The singleton instance of the database</returns>
        public static CompanyDirectoryServiceDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new CompanyDirectoryServiceDatabase();
            }
            return instance;
        }

    }

    /// <summary>
    /// This portion of the class contains the properties and variables 
    /// </summary>
    public partial class CompanyDirectoryServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>
        private const String dbname = "echoservicedb";
        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton instance of the database
        /// </summary>
        protected static CompanyDirectoryServiceDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>

        protected override Table[] tables { get; } =
        {
            new Table
            (
                dbname,
                "company",
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
                }
            )
        };
    }
}
