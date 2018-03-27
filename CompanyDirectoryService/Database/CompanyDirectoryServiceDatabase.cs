using Messages;
using Messages.Database;
using Messages.DataTypes;
using Messages.DataTypes.Database.CompanyDirectory;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.Echo.Requests;

using MySql.Data.MySqlClient;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        /// <summary>
        /// Saves the company to the database
        /// </summary>
        /// <param name="account">New Account Information</param>
        public void saveCompany(AccountCreated account)
        {
            try
            {
                Debug.consoleMsg("Name:" + account.name + " Email:" + account.email);
                if (account.type != AccountType.business)
                {
                    Debug.consoleMsg("Not a business account");
                    return;
                }
                if (openConnection() == true)
                {
                    string query = @"INSERT INTO company(companyName, phonenumber, email)" +
                        @"VALUES('" + account.name +
                        @"', '" + account.phonenumber + @"', '" + account.email + @"');";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();

                    query = @"INSERT INTO location(companyName, location)" +
                        @"VALUES('" + account.name +
                        @"', '" + account.address + @"');";

                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();

                    closeConnection();
                }
                else
                {
                    Debug.consoleMsg("Unable to connect to database");
                }
            } catch(MySqlException e)
            {
                Debug.consoleMsg("SQL Exception Occurred:" + e.Message);
            }
        }

        /// <summary>
        /// Saves the company to the database
        /// </summary>
        /// <param name="request">Information about the search request</param>
        public CompanyList searchCompany(CompanySearchRequest request)
        {
            CompanyList result = new CompanyList();
            if (openConnection() == true)
            {
                string query = @"SELECT * FROM company" +
                    @"WHERE companyName "+
                    @" LIKE '%" + request.searchDeliminator + @"%' );";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                ArrayList companies = new ArrayList();
                while (reader.Read())
                {
                    companies.Add(reader.GetString("companyName"));
                }
                
                result.companyNames = (string[])companies.ToArray();
                closeConnection();
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }
            return result;
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
        private const String dbname = "companydirectoryservicedb";
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
                    new Column
                        (
                            "companyName", "VARCHAR(100)",
                            new string[]
                            {
                                "NOT NULL",
                                "UNIQUE"
                            }, false
                        ),
                    new Column
                        (
                            "phonenumber", "VARCHAR(10)",
                            new string[]
                            {
                                "NOT NULL"
                            }, false
                        ),
                    new Column
                    (
                            "email", "VARCHAR(100)",
                            new string[]
                            {
                                "NOT NULL",
                                "UNIQUE"
                            }, false
                     ),
                }
            ),
             new Table
            (
                dbname,
                "location",
                new Column[]
                {
                    new Column
                    (
                            "companyName", "VARCHAR(100)",
                            new string[]
                            {
                                "NOT NULL",
                            }, true
                    ),
                     new Column
                    (
                            "location", "VARCHAR(150)",
                            new string[]
                            {
                                "NOT NULL",
                            }, true
                    ),
                }
             )
        };
    }
}